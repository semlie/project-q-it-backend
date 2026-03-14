using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuizController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuiz(IFormFile file)
        {
            try
            {
                // 1. בדיקת קובץ
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                string extractedText = "";
                var extension = Path.GetExtension(file.FileName).ToLower();

                // 2. חילוץ טקסט (PDF, PPTX, TXT)
                if (extension == ".pdf") extractedText = await ExtractTextFromPdfAsync(file);
                else if (extension == ".pptx") extractedText = await ExtractTextFromPptxAsync(file);
                else if (extension == ".txt")
                {
                    using var reader = new StreamReader(file.OpenReadStream());
                    extractedText = await reader.ReadToEndAsync();
                }
                else return BadRequest("Unsupported file type. Use PDF, PPTX or TXT.");

                if (string.IsNullOrWhiteSpace(extractedText))
                    return BadRequest("No text extracted from the file.");

                // 3. הגדרות API
                var apiKey = _configuration["GeminiApiKey"];
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromMinutes(3);

                // שימוש במודל שביקשת: gemini-3.1-flash-lite-preview
                var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite-preview:generateContent?key={apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new {
                            parts = new[] {
                                new { text = $@"צור 5 שאלות אמריקאיות בעברית על בסיס הטקסט הבא.

חובה להחזיר JSON תקין בלבד בפורמט הבא:
{{
  ""quizTitle"": ""כותרת המבחן"",
  ""questions"": [
    {{
      ""questionId"": 1,
      ""level"": 1||2||3, // 1-קל, 2-בינוני, 3-קשה
      ""questionText"": ""טקסט השאלה"",
      ""options"": [
        {{""optionId"": ""A"", ""optionText"": ""תשובה א""}},
        {{""optionId"": ""B"", ""optionText"": ""תשובה ב""}},
        {{""optionId"": ""C"", ""optionText"": ""תשובה ג""}},
        {{""optionId"": ""D"", ""optionText"": ""תשובה ד""}}
      ],
      ""correctAnswerId"": ""A"",
      ""explanation"": ""הסבר לתשובה הנכונה""
    }}
  ]
}}
דרישות:
1. questionId - מספר סידורי מ-1 עד 5
2. optionId - A, B, C, או D
3. correctAnswerId - חייב להיות אחד מה-optionId
4. כל השדות חובה 
5. ללא טקסט נוסף, רק JSON
הטקסט:
{extractedText.Substring(0, Math.Min(extractedText.Length, 5000))}"}
                            }
                        }
                    },
                    generationConfig = new {
                        response_mime_type = "application/json" // מבטיח קבלת JSON תקין מהמודל
                    }
                };

                // 4. שליחת הבקשה
                var response = await httpClient.PostAsync(apiUrl, new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, $"Gemini API Error: {responseString}");
                }

                // 5. עיבוד התשובה וחילוץ ה-JSON
                using var doc = JsonDocument.Parse(responseString);
                var aiTextResponse = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrEmpty(aiTextResponse))
                    return StatusCode(500, "Empty response from AI");

                // ניקוי Markdown במידה והמודל הוסיף (למרות הגדרת JSON Mode)
                var cleanJson = aiTextResponse.Trim();
                if (cleanJson.StartsWith("```json")) cleanJson = cleanJson.Replace("```json", "");
                if (cleanJson.EndsWith("```")) cleanJson = cleanJson.Substring(0, cleanJson.Length - 3);

                return Content(cleanJson.Trim(), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        // --- פונקציות עזר לחילוץ טקסט ---

        private async Task<string> ExtractTextFromPdfAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var pdfReader = new PdfReader(stream);
            using var pdfDoc = new PdfDocument(pdfReader);
            var text = new StringBuilder();
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                text.AppendLine(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
            }
            return text.ToString();
        }

        private async Task<string> ExtractTextFromPptxAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            var text = new StringBuilder();
            using (var doc = PresentationDocument.Open(stream, false))
            {
                var part = doc.PresentationPart;
                if (part?.Presentation?.SlideIdList != null)
                {
                    foreach (var slideId in part.Presentation.SlideIdList.Elements<DocumentFormat.OpenXml.Presentation.SlideId>())
                    {
                        var slide = (SlidePart)part.GetPartById(slideId.RelationshipId!);
                        foreach (var t in slide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
                        {
                            text.AppendLine(t.Text);
                        }
                    }
                }
            }
            return text.ToString();
        }
    }
}