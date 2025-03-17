using AIResumePortfolio.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace AIResumePortfolio.Services
{
    public interface IFileService
    {
        Task<Resume> UploadFileAsync(InputFileChangeEventArgs e);
        Task<byte[]> ReadFileAsync(string fileName);
    }
}
