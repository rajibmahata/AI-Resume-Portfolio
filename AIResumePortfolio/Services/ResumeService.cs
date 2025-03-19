using AIResumePortfolio.Models;

namespace AIResumePortfolio.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IGenericRepository<Resume> _resumeRepository;

        public ResumeService(IGenericRepository<Resume> resumeRepository)
        {
            _resumeRepository = resumeRepository;
        }

        public async Task<List<Resume>> GetResumesByIpAddressAsync(string ipAddress)
        {
            return (await _resumeRepository.FindAllAsync(r => r.IPAddress == ipAddress)).ToList();
        }
        public async Task<List<Resume>> GetResumesByIpandMacAddressAsync(string ipAddress, string macAddress)
        {
            return (await _resumeRepository.FindAllAsync(r => r.IPAddress == ipAddress || r.MAC==macAddress)).ToList();
        }

        public async Task DeleteResumeAsync(Guid resumeId)
        {
            var resume = await _resumeRepository.GetByIdAsync(resumeId);
            if (resume != null)
            {
                await _resumeRepository.RemoveAsync(resume);
            }
        }

        public async Task<Resume> InsertAsync(Resume resume)
        {
            await _resumeRepository.InsertAsync(resume);
            return resume;
        }

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
