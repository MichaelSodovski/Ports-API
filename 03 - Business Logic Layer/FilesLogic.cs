using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using PortsApi.Models;

namespace PortsApi
{
    public class FilesLogic : BaseLogic
    {
        private readonly string _attachmentsFolderPath;
        private readonly ILogger<FilesLogic> _logger;

        public FilesLogic(IOptions<AppSettings> appSettings, ILogger<FilesLogic> logger)
        {
            _attachmentsFolderPath = appSettings.Value.AttachmentsFolderPath!;
            _logger = logger;
        }

        public File GetFile(File selectedFile)
        {
            _logger.LogInformation("Getting file with ID '{fileID}'", selectedFile.FileID);

            if (string.IsNullOrEmpty(selectedFile.FileID))
            {
                return selectedFile;
            }

            string filePath = Path.Combine(_attachmentsFolderPath, selectedFile.FileID);

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogWarning("File with ID '{fileID}' not found", selectedFile.FileID);
                return selectedFile;
            }

            byte[] ItemFile = System.IO.File.ReadAllBytes(filePath);
            string mimeType = GetMimeType(filePath);
            _logger.LogInformation("File with ID '{fileID}' has MIME type '{mimeType}' and extension '{extension}'",
                selectedFile.FileID, mimeType, Path.GetExtension(filePath));
            // Initialize the FileContent property if it's null
            if (selectedFile.FileContent == null)
            {
                selectedFile.FileContent = new FileContent();
            }
            // Set the Attachment property to a new CustomFormFile instance
            selectedFile.FileContent.Content = ItemFile;
            selectedFile.FileContent.MimeType = mimeType;

            _logger.LogInformation("File with ID '{fileID}' retrieved successfully", selectedFile.FileID);
            return selectedFile;
        }

        private string GetMimeType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            switch (extension)
            {
                case ".txt":
                    return "text/plain";
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".svg":
                    return "image/svg-xml";
                case ".jpg":
                    return "image/jpeg";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".doc":
                    return "application/msword";
                case ".pdf":
                    return "application/pdf";
                case ".dwg":
                    return "application/acad";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return " application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                default:
                    return "application/octet-stream"; // Fallback to binary data for unknown file types.
            }
        }

        public File AddFile(File file, List<FolderObj> selectedFolders)
        {
            _logger.LogInformation("Adding file to selected folders");

            if (file.Attachment == null || file.Attachment.FileName == null)
            {
                _logger.LogWarning("File attachment or file name is null");
                return file;
            }

            string extension = Path.GetExtension(file.Attachment.FileName);
            string fileName = Path.GetFileName(file.Attachment.FileName);
            string guid = Guid.NewGuid() + extension;
            // place the file in the attachments folder directory by . 
            using (FileStream fileStream = System.IO.File.Create("attachments/" + guid))
            {
                file.Attachment.CopyTo(fileStream);
            }

            _logger.LogInformation("File '{fileName}' saved with ID '{guid}'", fileName, guid);

            List<CommonMngFile> fileEntriesToAdd = new List<CommonMngFile>();
            foreach (var folder in selectedFolders)
            {
                CommonMngFile fileToAdd = new CommonMngFile
                {
                    FileName = fileName, 
                    FolderId = folder.id, 
                    LayerId = file.LayerID,
                    FeatureId = folder.featureId,
                    FileId = guid, 
                    FolderName = folder.folderName
                };
                fileEntriesToAdd.Add(fileToAdd);
            }
            // pass list to db. 
            DB.CommonMngFiles.AddRange(fileEntriesToAdd);
            DB.SaveChanges();

            _logger.LogInformation("File '{fileName}' added to {folderCount} folders", fileName, selectedFolders.Count);

            return file;
        }

        public List<File> GetFilesByFolderNamesAndFeatureId(string[] folderNames, int featureId)
        {
            if (folderNames == null || featureId <= 0)
            {
                return new List<File>();
            }

            List<File> fileNames = DB.CommonMngFiles.Where(f => folderNames
            .Contains(f.FolderName) && f.FeatureId == featureId)
                .Select(f => new File
                {
                    ID = f.Id,
                    FileName = f.FileName,
                    FolderID = f.FolderId,
                    LayerID = f.LayerId,
                    FeatureID = f.FeatureId,
                    FileID = f.FileId,
                    FolderName = f.FolderName
                }).ToList();

            _logger.LogInformation($"Retrieved {fileNames.Count} file names by folder names and feature ID");

            return fileNames;
        }

        // delete the files from the disc contrary to deleting their entries in the common mng files table. 
        public async Task DeleteFilesFromDisc(List<string> fileIds)
        {
            foreach (var fileID in fileIds)
            {
                string filePath = "attachments/" + fileID;

                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        await Task.Run(() => System.IO.File.Delete(filePath));
                    }
                    else
                    {
                        Console.WriteLine("File not found: " + filePath);
                    }
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine("Error deleting file: " + ioEx.Message);
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    Console.WriteLine("Access denied: " + uaEx.Message);
                }
            }
        }

        public async Task DeleteFilesFromFolders(FolderObj[] folders, int featureId)
        {
            if (folders == null || featureId <= 0)
            {
                _logger.LogWarning("Invalid parameters: folders is null or featureId is less than or equal to 0");
                return;
            }

            List<string> fileNames = new List<string>();
            foreach (var folder in folders)
            {
                fileNames.AddRange(folder.fileNames ?? Enumerable.Empty<string>()); // provide an empty enumerable when filename is null to avoid NullReferenceException.
            }

            // fetch file ids
            List<string> fileIds = new List<string>();
            foreach (var fileName in fileNames)
            {
                var fileId = DB.CommonMngFiles
                    .Where(f => f.FileName == fileName && f.FeatureId == featureId)
                    .Select(f => f.FileId)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(fileId))
                {
                    fileIds.Add(fileId);
                }
            }

            // delete the entries in the db. 
            foreach (var folder in folders)
            {
                if (folder.folderName == null || folder.fileNames == null)
                {
                    continue;
                }

                foreach (var fileName in folder.fileNames)
                {
                    List<CommonMngFile> fileToDelete = DB.CommonMngFiles
                        .Where(f => f.FeatureId == featureId && f.FolderName == folder.folderName && f.FileName == fileName).ToList();

                    if (fileToDelete != null)
                    {
                        DB.CommonMngFiles.RemoveRange(fileToDelete);
                    }
                }
            }

                _logger.LogInformation("Deleting non-attached files from disk");
                await DeleteFilesFromDisc(fileIds);
                _logger.LogInformation("Saving changes to the database");
                // save changes. 
                await DB.SaveChangesAsync();
        }

        public async Task DeleteFiles(File[] files, int featureId)
        {
            if (files == null || featureId <= 0)
            {
                _logger.LogWarning("Invalid parameters: files is null or featureId is less than or equal to 0");
                return;
            }

            List<string> fileIds = new List<string>();
            foreach (var file in files)
            {
                fileIds.Add(file.FileID);
            }

            foreach (var file in files)
            {
                if (file.FolderName == null)
                {
                    continue;
                }

                foreach (var fileID in fileIds)
                {
                    List<CommonMngFile> fileToDelete = DB.CommonMngFiles
                        .Where(f => f.FeatureId == featureId && f.FolderName == file.FolderName && f.FileId == fileID).ToList();

                    if (fileToDelete != null)
                    {
                        DB.CommonMngFiles.RemoveRange(fileToDelete);
                    }
                }
                _logger.LogInformation("Saving changes to the database");
                await DB.SaveChangesAsync();
            }

            // after deletion of the selected folder entries, we need to decide either we delete the file from the disc or we need to keep it 
            // because it has other associations with other folders which we didnt choose.. 
            // check if the files has strings attached to other folders:
            // loop over the fileNames array and pull all the entries associated with the file names.
            List<CommonMngFile> entries = new List<CommonMngFile>();
            foreach (var fileID in fileIds)
            {
                CommonMngFile fileEntrie = DB.CommonMngFiles.Where(f => f.FileId == fileID).FirstOrDefault();
                if (fileEntrie != null)
                {
                    entries.Add(fileEntrie);
                }
            }

            if (entries.Count == 0) // if there are no other entries then there is no need to keep the file on the disc. 
            {
                _logger.LogInformation("Deleting non-attached files from disk");
                // delete non attached files psysically. 
                await DeleteFilesFromDisc(fileIds);
            }
            else
            {
                _logger.LogInformation("Saving changes to the database");
                // save changes. 
                await DB.SaveChangesAsync();
            }
        }
    }
}
