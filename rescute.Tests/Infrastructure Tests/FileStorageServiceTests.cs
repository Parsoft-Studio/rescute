using rescute.Infrastructure.Services;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;
using rescute.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.Tests.InfrastructureTests
{

    public class FileStorageServiceTests
    {
        public static readonly string TestFileStorageRoot = Path.Combine(Environment.CurrentDirectory, "FileStorageService Tests");
        public static IEnumerable<IFormFile> CreateIFormFileAttachments(int howManyFiles, int howManyBytesInFiles, byte byteData, string fileDescription)
        {
            var result = new List<IFormFile>();
            var data = Array.CreateInstance(typeof(byte), howManyBytesInFiles);

            for (int i = 0; i < data.Length; i++)
            {
                data.SetValue(byteData, i);
            }
            for (int i = 0; i < howManyFiles; i++)
            {

                var stream = new MemoryStream((byte[])data);
                var file = new FormFile(stream, 0, howManyBytesInFiles, "name", $"fileName{i}.jpg");

                var headerItem = new KeyValuePair<string, StringValues>("description", new StringValues(fileDescription));
                file.Headers = new HeaderDictionary { headerItem };
                yield return file;
            }
            //return result;
        }
        [Fact]
        public async void StorageServiceStoresFileCorrectly()
        {
            // Arrange

            var storageService = new FileStorageService(TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
            var parentDirectoryId = Guid.NewGuid().ToString();
            var attachmentDescription = "My test pet image.";
            FileInfo fileInfo;
            Attachment attachment;

            var files = CreateIFormFileAttachments(1, 4, 5, attachmentDescription);

            // Act
            attachment = await storageService.Store(files.First(), parentDirectoryId, AttachmentType.Image());


            // Assert
            attachment.Should().NotBeNull();
            attachment.Description.Should().Be(attachmentDescription);
            attachment.Type.Should().Be(AttachmentType.Image());
            attachment.FileName.Should().NotBeNullOrEmpty();

            fileInfo = new FileInfo(Path.Combine(storageService.AttachmentsDirectory, attachment.FileName));
            fileInfo.Exists.Should().Be(true);
            fileInfo.Length.Should().Be(4);
            using (var stream = fileInfo.OpenRead())
            {
                var buffer = new Memory<byte>();
                await stream.ReadAsync(buffer);
                buffer.ToArray().All(b => b == 5).Should().Be(true);
            }
        }

        [Fact]
        public void StorageServiceDoesntStoreInvalidFileType()
        {
            // Arrange
            var storageService = new FileStorageService(TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
            var parentDirectoryId = Guid.NewGuid().ToString();
            Attachment attachment;

            Func<Task> action = async () =>
            {
                using (var stream = new MemoryStream(new byte[4] { 5, 5, 5, 5 }))
                {
                    var file = new FormFile(stream, 0, 4, "name", "fileName.exe");
                    file.Headers = new HeaderDictionary();

                    // Act
                    attachment = await storageService.Store(file, parentDirectoryId, AttachmentType.Image());
                }

            };
            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async void StorageServiceDeletesAttachmentFile()
        {
            // Arrange

            var storageService = new FileStorageService(TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
            var parentDirectoryId = Guid.NewGuid().ToString();

            FileInfo fileInfo;
            Attachment attachment;
            using (var stream = new MemoryStream(new byte[4] { 5, 5, 5, 5 }))
            {
                var file = new FormFile(stream, 0, 4, "name", "fileName.jpg");
                file.Headers = new HeaderDictionary();

                // Act
                attachment = await storageService.Store(file, parentDirectoryId, AttachmentType.Image());
                await storageService.DeleteAttachmentFile(attachment.FileName);
            }

            // Assert
            fileInfo = new FileInfo(attachment.FileName);
            fileInfo.Exists.Should().Be(false);
        }

        [Fact]
        public async void StorageServiceDeletesAttachmentDirectory()
        {
            // Arrange

            var storageService = new FileStorageService(TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
            var parentDirectoryId = Guid.NewGuid().ToString();

            DirectoryInfo directoryInfo;
            Attachment attachment;
            using (var stream = new MemoryStream(new byte[4] { 5, 5, 5, 5 }))
            {
                var file = new FormFile(stream, 0, 4, "name", "fileName.jpg");
                file.Headers = new HeaderDictionary();

                // Act
                attachment = await storageService.Store(file, parentDirectoryId, AttachmentType.Image());
                await storageService.DeleteDirectoryForAttachment(parentDirectoryId.ToString());
            }

            // Assert
            directoryInfo = new DirectoryInfo(Path.Combine(storageService.AttachmentsDirectory, parentDirectoryId.ToString()));
            directoryInfo.Exists.Should().Be(false);

        }
    }
}