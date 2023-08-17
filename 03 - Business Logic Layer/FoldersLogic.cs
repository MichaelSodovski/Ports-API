using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using PortsApi.Models;

namespace PortsApi
{
    public class FoldersLogic : BaseLogic
    {
        private readonly ILogger<FoldersLogic> _logger;
        public FoldersLogic(ILogger<FoldersLogic> logger)
        {
            _logger = logger;
        }

        public bool isFolderNameExists(string? folderName)
        {
            _logger.LogInformation("Checking if folder name '{folderName}' exists", folderName);

            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }
            List<Folder> folders = DB.CommonMngFolders.Select(f => new Folder { FolderName = f.FolderName }).ToList();
            return folders.Any(u => u.FolderName == folderName);
        }

        public Folder AddFolder(Folder folder)
        {
            _logger.LogInformation("Adding folder '{folderName}'", folder.FolderName);

            CommonMngFolder folderToAdd = new CommonMngFolder()
            {
                FolderName = folder.FolderName,
                LayerId = folder.LayerID,
                FeatureId = folder.FeatureID
            };
            DB.CommonMngFolders.Add(folderToAdd);
            DB.SaveChanges();
            folder.ID = folderToAdd.Id;
            return folder;
        }

        public List<Folder> GetFolderNameList()
        {
            _logger.LogInformation("Retrieving all folder names");

            return DB.CommonMngFolders.Select(f => new Folder
            {
                ID = f.Id,
                FolderName = f.FolderName,
                LayerID = f.LayerId,
                FeatureID = f.FeatureId
            }).ToList();
        }

        public List<Folder> GetFoldersByFeatureId(int featureId)
        {
            _logger.LogInformation("Retrieving folders for feature ID {featureId}", featureId);

            var folders = DB.CommonMngFolders.Where(f => f.FeatureId == featureId)
                .Select(f => new Folder
                {
                    ID = f.Id,
                    FolderName = f.FolderName,
                    LayerID = f.LayerId,
                    FeatureID = f.FeatureId
                }).ToList();
            return folders;
        }

        public void DeleteFolder(FolderObj[] folders, int featureId)
        {
            _logger.LogInformation("Deleting folders for feature ID {featureId}", featureId);

            if (folders == null || featureId <= 0)
            {
                return;
            }

            // Extract folder names from the folders array
            var folderNamesToDelete = folders.Select(f => f.folderName).ToList();

            // Filter CommonMngFolders by folder names and featureId
            List<CommonMngFolder> foldersToDelete = DB.CommonMngFolders
                .Where(f => folderNamesToDelete.Contains(f.FolderName) && f.FeatureId == featureId)
                .ToList();

            // Delete the folders
            DB.CommonMngFolders.RemoveRange(foldersToDelete);
            DB.SaveChanges();
        }
    }
}
