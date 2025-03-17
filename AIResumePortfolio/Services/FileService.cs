using AIResumePortfolio.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.NetworkInformation;
using System.Net;

namespace AIResumePortfolio.Services
{
    public class FileService : IFileService
    {
        private readonly IGenericRepository<Resume> _resumeRepository;

        public FileService(IGenericRepository<Resume> resumeRepository)
        {
            _resumeRepository = resumeRepository;
        }

        public async Task<Resume> UploadFileAsync(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var validFormats = new[] { ".pdf", ".doc", ".docx" };

            if (!validFormats.Contains(Path.GetExtension(file.Name).ToLower()))
            {
                throw new InvalidOperationException("Invalid file format. Please upload a PDF or DOC file.");
            }

            if (file.Size > 5 * 1024 * 1024) // 5 MB limit
            {
                throw new InvalidOperationException("File size exceeds the 5 MB limit.");
            }

            using var memoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(memoryStream);
            var mac = GetMacAddress();
            var ipAddress = GetIpAddress();

            var resume = new Resume
            {
                Id = Guid.NewGuid(),
                FileName = file.Name,
                FileContent = memoryStream.ToArray(),
                CreatedAt = DateTime.Now,
                ParsedJson = "{}",
                PortfolioHtml = "<p>Generated Portfolio</p>",
                MAC = mac,
                IPAddress = ipAddress
            };

            await _resumeRepository.InsertAsync(resume);
            return resume;
        }

        public async Task<byte[]> ReadFileAsync(string fileName)
        {
            var resume = await _resumeRepository.FindAsync(r => r.FileName == fileName);
            if (resume == null)
            {
                throw new FileNotFoundException("File not found.");
            }

            return resume.FileContent;
        }

        private string GetMacAddress()
        {
            var macAddr =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();
            return macAddr;
        }

        private string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ip?.ToString();
        }

        //public string ExtractText(IFormFile file)
        //{
        //    if (file.ContentType == "application/pdf")
        //        return new IronPdf.PdfDocument(file.OpenReadStream()).ExtractAllText();

        //    if (file.ContentType == "application/msword" ||
        //        file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        //        return DocX.Load(file.OpenReadStream()).Text;

        //    throw new InvalidDataException("Unsupported file format");
        //}
    }
}
