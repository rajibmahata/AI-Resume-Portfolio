using AIResumePortfolio.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using System.Text;
using Newtonsoft.Json;
using AIResumePortfolio.AIAgents;

namespace AIResumePortfolio.Services
{
    public class FileService : IFileService
    {
        private readonly IGenericRepository<Resume> _resumeRepository;
        private readonly HuggingFaceService _huggingfaceAIService;
        private readonly OllamaAIService _OllamaAIService;
        public FileService(IGenericRepository<Resume> resumeRepository, HuggingFaceService huggingfaceAIService, OllamaAIService ollamaAIService)
        {
            _resumeRepository = resumeRepository;
            _huggingfaceAIService = huggingfaceAIService;
            _OllamaAIService = ollamaAIService;
        }
        public async Task<Resume> InsertAsync(Resume resume)
        {
            if (resume == null)
            {
                throw new ArgumentNullException(nameof(resume));
            }

            //string prompt = "Extract JSON with name, summary, skills, experience, education, contact...";
            //var response = await _openAi.CreateCompletion(new CompletionCreateRequest
            //{
            //    Prompt = prompt + resume.Text,
            //    Model = "text-davinci-003",
            //    MaxTokens = 1000
            //});

            string resumeText = ExtractTextFromDocx(resume.FileContent);

            // 1. AI Parsing
            var standardResume = await _OllamaAIService.ProcessResumeTextAsync(resumeText);

            // 2. AI Parsing
            //var ExtractResumeText = await _huggingfaceAIService.ExtractResumeText(resumeText);
            //// 2. AI Parsing
            //var standardResume = await _huggingfaceAIService.GenerateStructuredResume(ExtractResumeText);

            // 3. HTML Generation
            // var portfolioHtml = await _huggingfaceAIService.GenerateResumeHtml(standardResume);

            // 4. Save to database
            var newresume = new Resume
            {
                FileName = resume.FileName,
            //    ParsedJson = JsonConvert.SerializeObject(standardResume),
           //     PortfolioHtml = portfolioHtml,
                CreatedAt = DateTime.UtcNow
            };

            resume.ParsedTextContent = resumeText;
            await _resumeRepository.InsertAsync(resume);

            return resume;
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

        public string ExtractTextFromDocx(byte[] fileContent)
        {
            try
            {
                StringBuilder extractedText = new StringBuilder();

                using (var memoryStream = ByteArrayToStream(fileContent))
                {
                    using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, false))
                    {
                        var body = doc.MainDocumentPart?.Document?.Body;
                        if (body != null)
                        {
                            foreach (var element in body.Elements())
                            {
                                extractedText.AppendLine(element.InnerText);
                            }
                        }
                    }
                }

                // Add space after each sentence
                string textWithSpaces = System.Text.RegularExpressions.Regex.Replace(
                    extractedText.ToString(),
                    @"(?<=[.!?])\s*(?=[A-Z])",
                    " "
                );

                return textWithSpaces;
            }
            catch (Exception ex)
            {
                return $"Error extracting DOCX text: {ex.Message}";
            }
        }


        public Stream ByteArrayToStream(byte[] byteArray)
        {
            return new MemoryStream(byteArray);
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
