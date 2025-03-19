using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIResumePortfolio.AIAgents
{
    public class HuggingfaceAIService
    {
        private readonly HttpClient _httpClient;
        private const string HuggingFaceApiUrl = "https://api-inference.huggingface.co/models/";
        private const string ApiKey = "YOUR_OPENAI_API_KEY"; // Free API Key

        public HuggingfaceAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Agent 1: Extract and format resume into market-standard JSON using Hugging Face
        public async Task<ResumeJsonModel> GenerateStructuredResume(string resumeText)
        {
            var requestBody = new { inputs = resumeText };
            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(HuggingFaceApiUrl + "bigscience/bloom-560m", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<List<GeneratedTextResponse>>(responseContent);

            if (parsedResponse == null || parsedResponse.Count == 0 || string.IsNullOrWhiteSpace(parsedResponse[0].GeneratedText))
            {
                throw new Exception("AI model did not return structured data.");
            }

            // Attempt to parse response text into structured JSON
            try
            {
                var structuredResume = JsonSerializer.Deserialize<ResumeJsonModel>(parsedResponse[0].GeneratedText);
                return structuredResume ?? throw new Exception("Parsed JSON is null.");
            }
            catch (Exception)
            {
                // Fallback: Rule-based extraction (basic)
                return ExtractResumeManually(resumeText);
            }
        }

        // Fallback Method: Basic Rule-based Resume Extraction
        private ResumeJsonModel ExtractResumeManually(string resumeText)
        {
            var structuredResume = new ResumeJsonModel
            {
                Name = ExtractBetween(resumeText, "Name:", "Summary:")?.Trim() ?? "Unknown",
                Summary = ExtractBetween(resumeText, "Summary:", "Skills:")?.Trim() ?? "Not Available",
                Skills = ExtractBetween(resumeText, "Skills:", "Experience:")?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>(),
                Experience = new List<ExperienceModel>(),
                Education = new List<EducationModel>(),
                Projects = new List<ProjectModel>()
            };
            return structuredResume;
        }

        private string ExtractBetween(string text, string start, string end)
        {
            int startIndex = text.IndexOf(start) + start.Length;
            int endIndex = text.IndexOf(end);
            if (startIndex < start.Length || endIndex == -1 || startIndex >= endIndex) return null;
            return text[startIndex..endIndex];
        }

        // Agent 2: Generate modern HTML resume from JSON using Hugging Face
        public async Task<string> GenerateResumeHtml(ResumeJsonModel structuredResume)
        {
            var requestBody = new { inputs = JsonSerializer.Serialize(structuredResume) };
            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(HuggingFaceApiUrl + "philschmid/t5-small-html-generator", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }

    // Model for AI Response
    public class GeneratedTextResponse
    {
        public string GeneratedText { get; set; }
    }
}
