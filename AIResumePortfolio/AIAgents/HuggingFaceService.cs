using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIResumePortfolio.AIAgents
{
    public class HuggingFaceService
    {
        private readonly HttpClient _httpClient;
        private const string HuggingFaceApiUrl = "https://api-inference.huggingface.co/v1/completions";
        private const string ApiKey = ""; // Free API Key

        public HuggingFaceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Agent 1: Extract text from resume using Hugging Face's sentence-transformers
        public async Task<string> ExtractResumeText(string resumeText)
        {
            var requestBody = new
            {
                model = "sentence-transformers/all-MiniLM-L6-v2",
                inputs = resumeText
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(HuggingFaceApiUrl, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        // Agent 2: Convert extracted text into structured JSON using T5-base model
        public async Task<ResumeJsonModel> GenerateStructuredResume(string extractedText)
        {
            var requestBody = new
            {
                model = "t5-base",
                messages = new[]
                {
                new
                {
                    role = "user",
                    content = new[]
                    {
                        new { type = "text", text = "Format the extracted resume text into a structured JSON with market-standard formatting and refined content." },
                        new { type = "text", text = extractedText }
                    }
                }
            },
                max_tokens = 5000,
                stream = false
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(HuggingFaceApiUrl, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<HuggingFaceResponse>(responseContent);

            if (parsedResponse?.Choices == null || parsedResponse.Choices.Count == 0)
            {
                throw new Exception("AI model did not return structured data.");
            }

            try
            {
                var structuredResume = JsonSerializer.Deserialize<ResumeJsonModel>(parsedResponse.Choices[0].Message.Content[1].Text);
                return structuredResume ?? throw new Exception("Parsed JSON is null.");
            }
            catch (Exception)
            {
                throw new Exception("Failed to parse AI-generated JSON.");
            }
        }
        //// Agent 1: Extract and format resume into market-standard JSON using Hugging Face
        //public async Task<ResumeJsonModel> GenerateStructuredResume(string resumeText)
        //{
        //    var requestBody = new
        //    {
        //        model = "mistralai/Mistral-Nemo-Instruct-2407-fast",
        //        messages = new[]
        //        {
        //        new
        //        {
        //            role = "user",
        //            content = new[]
        //            {
        //                new { type = "text", text = "create json format the extracted resume and refine the sentences." },
        //                new { type = "text", text = resumeText }
        //            }
        //        }
        //    },
        //        max_tokens = 5000,
        //        stream = false
        //    };

        //    var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

        //    var response = await _httpClient.PostAsync(HuggingFaceApiUrl, requestContent);
        //    response.EnsureSuccessStatusCode();

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var parsedResponse = JsonSerializer.Deserialize<HuggingFaceResponse>(responseContent);

        //    if (parsedResponse?.Choices == null || parsedResponse.Choices.Count == 0)
        //    {
        //        throw new Exception("AI model did not return structured data.");
        //    }

        //    try
        //    {
        //        var structuredResume = JsonSerializer.Deserialize<ResumeJsonModel>(parsedResponse.Choices[0].Message.Content[1].Text);
        //        return structuredResume ?? throw new Exception("Parsed JSON is null.");
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("Failed to parse AI-generated JSON.");
        //    }
        //}

        // Agent 2: Generate modern HTML resume from JSON using Hugging Face
        public async Task<string> GenerateResumeHtml(ResumeJsonModel structuredResume)
        {
            var requestBody = new
            {
                model = "google/gemma-3-27b-it",
                messages = new[]
                {
                new
                {
                    role = "user",
                    content = new[]
                    {
                        new { type = "text", text = "Generate a modern and visually appealing HTML format for this resume JSON." },
                        new { type = "text", text = JsonSerializer.Serialize(structuredResume) }
                    }
                }
            },
                max_tokens = 3000,
                stream = false
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(HuggingFaceApiUrl, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }

    // Model for AI Response
    public class HuggingFaceResponse
    {
        public List<HuggingFaceChoice> Choices { get; set; }
    }

    public class HuggingFaceChoice
    {
        public HuggingFaceMessage Message { get; set; }
    }

    public class HuggingFaceMessage
    {
        public List<HuggingFaceText> Content { get; set; }
    }

    public class HuggingFaceText
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }

   
}
