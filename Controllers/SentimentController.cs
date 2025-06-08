using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

// ⬇️  Namespace real de los tipos generados (revísalo en el .cs)
using In  = SentimentModel.ConsoleApp.SentimentModel.ModelInput;
using Out = SentimentModel.ConsoleApp.SentimentModel.ModelOutput;

namespace EvaluadorInteligente.Controllers
{
    [Route("[controller]")]
    public class SentimentController : Controller
    {
        private readonly PredictionEngine<In, Out> _predEngine;

        // El PredictionEngine lo crea el servicio registrado en Program.cs
        public SentimentController(PredictionEngine<In, Out> predEngine)
            => _predEngine = predEngine;

        // ------------ VISTA --------------
        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpPost("")]
        public IActionResult Index(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError("", "Escribe una opinión para analizar.");
                return View();
            }

            var r = _predEngine.Predict(new In { Text = text });

            ViewBag.Text      = text;
            ViewBag.Predicted = r.PredictedLabel ? "Positivo 😀" : "Negativo 🙁";
            ViewBag.Score     = r.Score;

            return View();
        }

        // ------------ API --------------
        [HttpPost("/api/sentiment")]
        public IActionResult Analyze([FromBody] SentimentRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest("Se requiere el campo 'text'.");

            var r = _predEngine.Predict(new In { Text = dto.Text });
            return Ok(new { text = dto.Text, label = r.PredictedLabel, score = r.Score });
        }

        // DTO que recibe el JSON { "text": "..." }
        public class SentimentRequest
        {
            public string Text { get; set; } = string.Empty;
        }
    }
}
