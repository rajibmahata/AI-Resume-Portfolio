namespace AIResumePortfolio.AIAgents
{
    public class PortfolioDesignAgent
    {
        private readonly OpenAIApi _openAi;
        private const string DesignPrompt = """
        Create modern HTML/CSS portfolio from this JSON resume. Requirements:
        - Responsive design
        - Clean typography
        - Skill tags with hover effects
        - Timeline layout for experience
        - Minimal color scheme
        - Include all sections
        - Mobile-friendly
        Return ONLY the HTML with embedded CSS:
        
        """;

        public async Task<string> GeneratePortfolioHtml(StandardResume resume)
        {
            var resumeJson = JsonConvert.SerializeObject(resume);

            var completion = await _openAi.CreateChatCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage> {
                ChatMessage.FromSystem("You are a professional web designer"),
                ChatMessage.FromUser(DesignPrompt + resumeJson)
            },
                Model = "gpt-4",
                Temperature = 0.3
            });

            return SanitizeHtml(completion.Choices[0].Message.Content);
        }

        private string SanitizeHtml(string html)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add("div");
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedCssProperties.Add("color");
            sanitizer.AllowedCssProperties.Add("background-color");

            return sanitizer.Sanitize(html);
        }
    }
}
