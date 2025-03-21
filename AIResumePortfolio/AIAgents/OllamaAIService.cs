using System.Text;
using System.Text.Json;

namespace AIResumePortfolio.AIAgents
{
    public class OllamaAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _modelName = "gemma3:latest"; // Change to "llama3" if needed

        public OllamaAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResumeModel> ProcessResumeTextAsync(string resumeText)
        {
            string prompt = $@"
                Extract structured details from the following resume:
                {resumeText}
                
                Format:
                {{
                    ""FullName"": ""John Doe"",
                    ""Summary"": ""Experienced software engineer specializing in .NET and Azure"",
                    ""Skills"": [""C#"", "".NET"", ""Azure"", ""Microservices""],
                    ""Experience"": [
                        {{
                            ""JobTitle"": ""Software Engineer"",
                            ""Company"": ""ABC Corp"",
                            ""Duration"": ""2019-2024""
                        }}
                    ],
                    ""Education"": [""B.Tech in Computer Science""]
                }}";

            var requestBody = new
            {
                model = _modelName,
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonDocument.Parse(responseString);
            string extractedJson = jsonResponse.RootElement.GetProperty("response").GetString();

            return JsonSerializer.Deserialize<ResumeModel>(extractedJson);
        }
    }

    public class ResumeModel
    {
        public string FullName { get; set; }
        public string Summary { get; set; }
        public List<string> Skills { get; set; }
        public List<ExperienceModel> Experience { get; set; }
        public List<string> Education { get; set; }
    }

   
}
