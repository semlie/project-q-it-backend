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
                if (file == null || file.Length == 0) return BadRequest("No file uploaded");

                string extractedText = "";
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (extension == ".pdf") extractedText = await ExtractTextFromPdf(file);
                else if (extension == ".pptx") extractedText = await ExtractTextFromPptx(file);
                else if (extension == ".txt")
                {
                    using var reader = new StreamReader(file.OpenReadStream());
                    extractedText = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(extractedText)) return BadRequest("No text extracted");

                var apiKey = _configuration["GeminiApiKey"];
                var httpClient = _httpClientFactory.CreateClient();

                // ----------------------------------------------------------------------------------
                // שימוש במודל gemini-2.5-flash - כפי שמופיע ברשימה ששלחת בתמונה
                // ----------------------------------------------------------------------------------
                var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new {
                            parts = new[] {
                                new { text = $@"צור 5 שאלות אמריקאיות בעברית על הטקסט הבא. החזר אך ורק JSON נקי. 
                                                מבנה: [{{""question"":""..."",""options"":[""..."",""..."",""..."",""...""],""correctAnswer"":""...""}}] 
                                                טקסט: {extractedText.Substring(0, Math.Min(extractedText.Length, 5000))}" }
                            }
                        }
                    },
                    generationConfig = new {
                        response_mime_type = "application/json"
                    }
                };

                var response = await httpClient.PostAsync(apiUrl, new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // אם מופיעה שגיאה 429, זה אומר שצריך לחכות דקה.
                    return StatusCode((int)response.StatusCode, $"API Error: {responseString}");
                }

                using var doc = JsonDocument.Parse(responseString);
                var aiText = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

                return Content(aiText, "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        private async Task<string> ExtractTextFromPdf(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var pdfReader = new PdfReader(stream);
            using var pdfDoc = new PdfDocument(pdfReader);
            var text = new StringBuilder();
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) text.AppendLine(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
            return text.ToString();
        }

        private async Task<string> ExtractTextFromPptx(IFormFile file)
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
                        foreach (var t in slide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>()) text.AppendLine(t.Text);
                    }
                }
            }
            return text.ToString();
        }
    }
}