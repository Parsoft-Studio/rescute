using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure.Services;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

public class FileStorageServiceTests
{
    private static readonly string TestFileStorageRoot =
        Path.Combine(Environment.CurrentDirectory, "FileStorageService Tests");

    [Fact]
    private async Task StorageServiceStoresFileCorrectly()
    {
        // Arrange

        var storageService =
            new FileStorageService(TestFileStorageRoot, new List<string> { "jpg", "avi", "png", "mp4" });
        var parentDirectoryId = Guid.NewGuid().ToString();
        var attachmentDescription = "My test pet image.";
        FileInfo fileInfo;
        Attachment attachment;

        var files = CreateAttachments(1, 4, 5);

        // Act
        attachment =
            await storageService.Store(files.First(), "filename.jpg", attachmentDescription, parentDirectoryId);


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
            Array.TrueForAll(buffer.ToArray(), b => b == 5).Should().Be(true);
        }
    }

    [Fact]
    public void StorageServiceDoesntStoreInvalidFileType()
    {
        // Arrange
        var storageService =
            new FileStorageService(TestFileStorageRoot, new List<string> { "jpg", "avi", "png", "mp4" });
        var parentDirectoryId = Guid.NewGuid().ToString();

        var action = async () =>
        {
            using (var stream = new MemoryStream([5, 5, 5, 5]))
            {
                // Act
                await storageService.Store(stream, "filename.pdf", "description", parentDirectoryId);
            }
        };
        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task StorageServiceDeletesAttachmentFile()
    {
        // Arrange

        var storageService =
            new FileStorageService(TestFileStorageRoot, new List<string> { "jpg", "avi", "png", "mp4" });
        var parentDirectoryId = Guid.NewGuid().ToString();

        Attachment attachment;
        using (var stream = new MemoryStream([5, 5, 5, 5]))
        {
            // Act
            attachment = await storageService.Store(stream, "filename.jpg", "description", parentDirectoryId);
            await storageService.DeleteAttachmentFile(attachment.FileName);
        }

        // Assert
        var fileInfo = new FileInfo(attachment.FileName);
        fileInfo.Exists.Should().Be(false);
    }

    [Fact]
    public async Task StorageServiceDeletesAttachmentDirectory()
    {
        // Arrange

        var storageService =
            new FileStorageService(TestFileStorageRoot, new List<string> { "jpg", "avi", "png", "mp4" });
        var parentDirectoryId = Guid.NewGuid().ToString();

        using (var stream = new MemoryStream([5, 5, 5, 5]))
        {
            // Act
            await storageService.Store(stream, "filename.jpg", "description", parentDirectoryId);
            await storageService.DeleteDirectoryForAttachment(parentDirectoryId);
        }

        // Assert
        var directoryInfo = new DirectoryInfo(Path.Combine(storageService.AttachmentsDirectory, parentDirectoryId));
        directoryInfo.Exists.Should().Be(false);
    }

    private static IEnumerable<Stream> CreateAttachments(int howManyFiles, int howManyBytesInFiles, byte byteData)
    {
        var data = new byte[howManyBytesInFiles];

        for (var i = 0; i < data.Length; i++) data.SetValue(byteData, i);
        for (var i = 0; i < howManyFiles; i++)
        {
            var stream = new MemoryStream(data);

            yield return stream;
        }
    }
}