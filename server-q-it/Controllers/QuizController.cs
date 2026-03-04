using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;
using Microsoft.AspNetCore.Mvc;
//using UglyToad.PdfPig;
using System.Text;
using System.Net.Http.Json;
namespace webApiProject.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly string _apiKey = "AIzaSyDFhyvraCA3Lw7nv1s5JrWk2l1zo6UkiLI";
    private readonly HttpClient _httpClient;

    public QuizController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateQuiz(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        try
        {
            // 1. קריאת תוכן הקובץ (למשל אם זה קובץ טקסט פשוט)
            using var reader = new StreamReader(file.OpenReadStream(), leaveOpen: false);
            var fileContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(fileContent))
            {
                return BadRequest("Uploaded file is empty or unreadable as text.");
            }

            // 2. בניית הבקשה ל-Gemini
            var requestBody = new
            {
                contents = new[] {
                    new { parts = new[] { 
                        new { text = $"נתח את הטקסט הבא וצור 5 שאלות אמריקאיות בעברית בפורמט JSON: {fileContent}" } 
                    } }
                }
            };

            // 3. שליחה ל-API של Gemini
            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}";
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5) // הגדל ל-5 דקות
            };
            var response = await httpClient.PostAsJsonAsync(apiUrl, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }

            var result = await response.Content.ReadFromJsonAsync<dynamic>();

            if (result == null)
            {
                return StatusCode(502, "Gemini returned an empty response.");
            }

            return Ok(result);
        }
        catch (TaskCanceledException ex)
        {
            return StatusCode(408, "Request timeout - please try with a smaller file or try again later");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
}