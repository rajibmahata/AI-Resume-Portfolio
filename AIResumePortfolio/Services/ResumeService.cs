using AIResumePortfolio.Models;

namespace AIResumePortfolio.Services
{
    public class ResumeService
    {
        //private readonly OpenAIApi _openAi;
        //private readonly IFileService _fileService;

        //public async Task<Resume> ProcessResume(IFormFile file)
        //{
        //    var text = _fileService.ExtractText(file);
        //    var prompt = "Extract JSON with name, summary, skills, experience, education, contact...";

        //    var response = await _openAi.CreateCompletion(new CompletionCreateRequest
        //    {
        //        Prompt = prompt + text,
        //        Model = "text-davinci-003",
        //        MaxTokens = 1000
        //    });

        //    return JsonConvert.DeserializeObject<Resume>(response.Choices[0].Text);
        //}
    }
}
