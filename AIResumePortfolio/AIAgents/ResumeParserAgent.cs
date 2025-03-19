using Newtonsoft.Json;

namespace AIResumePortfolio.AIAgents
{
    public class ResumeParserAgent
    {
        private readonly OpenAIApi _openAi;
        private const string ParserPrompt = """
        Extract resume information into this JSON format:
        {
            "name": "Full Name",
            "summary": "Professional summary",
            "contact": {
                "email": "email@example.com",
                "phone": "+1234567890",
                "linkedin": "profile-url"
            },
            "skills": ["Skill 1", "Skill 2"],
            "experience": [
                {
                    "title": "Job Title",
                    "company": "Company Name",
                    "duration": "2020-Present",
                    "description": "Job responsibilities"
                }
            ],
            "education": [
                {
                    "degree": "Degree Name",
                    "institution": "University Name",
                    "year": "2020"
                }
            ]
        }
        Return ONLY valid JSON. Input resume text:
        """;

        public async Task<StandardResume> ParseResumeText(string resumeText)
        {
            var completion = await _openAi.CreateCompletion(new CompletionCreateRequest
            {
                Prompt = ParserPrompt + resumeText,
                Model = "gpt-3.5-turbo-instruct",
                MaxTokens = 1000,
                Temperature = 0.2
            });

            return ValidateAndParseJson(completion.Choices[0].Text);
        }

        private StandardResume ValidateAndParseJson(string json)
        {
            // Add JSON schema validation
            var schema = JsonSchema.Parse(@"{
            'type': 'object',
            'properties': {
                'name': {'type': 'string'},
                'summary': {'type': 'string'},
                'contact': {'type': 'object'},
                'skills': {'type': 'array'},
                'experience': {'type': 'array'},
                'education': {'type': 'array'}
            },
            'required': ['name', 'skills']
        }");

            var resume = JsonConvert.DeserializeObject<StandardResume>(json);
            var isValid = schema.Validate(json).Valid;

            return isValid ? resume : throw new InvalidResumeFormatException();
        }
    }
}
