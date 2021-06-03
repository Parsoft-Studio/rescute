using Microsoft.AspNetCore.Http;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.Infrastructure.Services
{
    /// <summary>
    /// The default implementation of <see cref="IFileStorageService"/>.
    /// </summary>
    public class FileStorageService : IFileStorageService

    {
        public string StorageRootDirectory { get; private set; }
        public string AttachmentsDirectory => Path.Combine(StorageRootDirectory, "attachments");
        public IReadOnlyCollection<string> ValidFileExtensions { get; private set; }


        public FileStorageService(string storageRootDirectory, IEnumerable<string> validFileExtensions)
        {
            if (validFileExtensions == null || !validFileExtensions.Any()) throw new ArgumentException("Valid file extensions should be provided.",nameof(validFileExtensions));

            StorageRootDirectory = storageRootDirectory;
            ValidFileExtensions = validFileExtensions.Select(ext => ext.ToLower()).ToList() as IReadOnlyCollection<string>;
        }


        public async Task DeleteDirectoryForAttachment(string attachmentParentDirectoryName)
        {
            await Task.Run(() =>
            {
                var path = Path.Combine(AttachmentsDirectory, attachmentParentDirectoryName);

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            });
        }
        public async Task DeleteAttachmentFile(string fileName)
        {
            await Task.Run(() =>
            {
                var path = Path.Combine(AttachmentsDirectory, fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });

        }


        public async Task<Attachment> Store(IFormFile file, string attachmentParentDirectoryName)
        {
            
            if (file == null) throw new ArgumentException("file cannot be null.", nameof(file));

            var extension = Path.GetExtension(file.FileName).Substring(1);
            if (!ValidFileExtensions.Contains(extension.ToLower())) throw new InvalidOperationException("Invalid extension found in list of specified files.");

            var attachmentPath = Path.Combine(AttachmentsDirectory, attachmentParentDirectoryName);

            var result = await Task.Run(async () =>
            {
                if (!Directory.Exists(attachmentPath)) Directory.CreateDirectory(attachmentPath);

                string randomeFileName;
                string fileName;
                do
                {
                    randomeFileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                    fileName = Path.Combine(attachmentPath, randomeFileName);
                } while (File.Exists(fileName));


                using (var fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                var found = file.Headers.TryGetValue("description", out var description);
                return new Attachment(filename: Path.Combine(attachmentParentDirectoryName, randomeFileName),
                                      creationDate: DateTime.Now,
                                      description: description,
                                      extension: Path.GetExtension(randomeFileName)
                                     );

            });
            return result;
        }

    }
}

