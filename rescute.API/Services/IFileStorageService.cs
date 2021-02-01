using Microsoft.AspNetCore.Http;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// The root directory of tha application in which attachments directory is located.
        /// </summary>
        string StorageRootDirectory { get; }
        /// <summary>
        /// The directory within the storage root directory where all attachments are stored. Includes the root directory path.
        /// </summary>
        string AttachmentsDirectory { get; }
        /// <summary>
        /// A list of valid file extensions. Attempting to store files not ending with these extensions would result in an <see cref="InvalidOperationException"/>. Not containing the preceding dot (.)
        /// </summary>
        public IReadOnlyCollection<string> ValidFileExtensions { get; }
        /// <summary>
        /// Stores the specified <see cref="IFormFile"/>s on disk and returns the corresponding <see cref="Attachment"/>.
        /// </summary>
        /// <param name="file">The <see cref="IFormFile"/> to be stored.</param>
        /// <param name="attachmentParentDirectoryName">The directory name (not path) of the parent to which specified file belongs. The file will be stored in this directory,
        /// which will be created in attachments directory if it doesn't already exist.</param>
        /// <param name="type">The type of this attachment.</param>
        /// <returns></returns>        
        Task<Attachment> Store(IFormFile file, string attachmentParentDirectoryName, AttachmentType type);
        /// <summary>
        /// Deletes the directory pertaining to a specific attachment (located inside the attachments directory) and all the files in it.
        /// </summary>
        /// <param name="attachmentParentDirectoryName">The name of the directory (located in the attachments directory) to be deleted.</param>
        /// <returns></returns>
        Task DeleteDirectoryForAttachment(string attachmentParentDirectoryName);
        /// <summary>
        /// Deletes a specific attachment file located in the attachments directory.
        /// </summary>
        /// <param name="fileName">The fully qualified path and file name of the file to be deleted.</param>
        /// <returns></returns>
        Task DeleteAttachmentFile(string fileName);
    }
}
