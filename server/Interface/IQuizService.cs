using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Interface
{
    public interface IQuizService
    {
        public class GenerateQuizRequest
        {
            public IFormFile? File { get; set; }
            public int NumberOfQuestions { get; set; } = 5;
            public int Level { get; set; } = 0; // 0 = all levels, 1 = easy, 2 = medium, 3 = hard
            public string? AdditionalInstructions { get; set; }
        }
       Task<string> ExtractTextFromPdfAsync(IFormFile file);
       Task<string> ExtractTextFromPptxAsync(IFormFile file);
        Task<IActionResult> GenerateQuiz([FromForm] GenerateQuizRequest request);

    }
}