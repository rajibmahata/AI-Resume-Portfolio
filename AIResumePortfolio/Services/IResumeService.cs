using AIResumePortfolio.Models;

namespace AIResumePortfolio.Services
{
    public interface IResumeService
    {
        Task<List<Resume>> GetResumesByIpandMacAddressAsync(string ipAddress, string macAddress);
        Task<List<Resume>> GetResumesByIpAddressAsync(string ipAddress);
        Task DeleteResumeAsync(Guid resumeId);
        Task<Resume> InsertAsync(Resume resume);
    }
}
