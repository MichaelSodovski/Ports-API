using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using PortsApi.Services;

namespace PortsApi
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]    
    public class FoldersController : ControllerBase
    {
        private readonly FoldersLogic _foldersLogic;
        private readonly FilesLogic _filesLogic;
        private readonly ILogger<FoldersController> _logger;

        public FoldersController(FoldersLogic foldersLogic, FilesLogic filesLogic, ILogger<FoldersController> logger)
        {
            _foldersLogic = foldersLogic;
            _filesLogic = filesLogic;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddFolderDetails")]
        public IActionResult AddFolderDetails(Folder folder)
        {
            try
            {
                if (_foldersLogic.isFolderNameExists(folder.FolderName))
                {
                    _logger.LogInformation("Folder already exists: {FolderName}", folder.FolderName);
                    return Ok(); // return message which states that the folder already exists. 
                }
                else
                {
                    Folder addedFolder = _foldersLogic.AddFolder(folder);
                    _logger.LogInformation("Folder created with ID {FolderID}: {FolderName}", addedFolder.ID, addedFolder.FolderName);
                    return Created("api/folders/" + addedFolder.ID, addedFolder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the folder: {FolderName}", folder.FolderName);
                if (ex.InnerException != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetFolderNames/{featureId}")]
        public IActionResult GetFolderNames(int featureId)
        {
            try
            {
                List<Folder> folderNames = _foldersLogic.GetFoldersByFeatureId(featureId);
                _logger.LogInformation("Retrieved folder names for feature ID {FeatureId}", featureId);
                return Ok(folderNames);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving folder names for feature ID {FeatureId}", featureId);
                if (ex.InnerException != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpDelete]
        [Route("DeleteFolders")]
        public async Task<IActionResult> FolderDelete([FromBody] DeleteModel deleteFoldersData) // i may need the db to which the feature is associated to too... 
        {
            if (deleteFoldersData.folders == null)
            {
                return NoContent();
            }

            FolderObj[] folders = deleteFoldersData.folders;
            int featureId = deleteFoldersData.featureId;

            try
            {
                if (folders != null && featureId > 0) // check that the array contains only the folder that needs to be deleted. 
                {
                    // before folder deleting, delete all the files accociated with it. 
                    await _filesLogic.DeleteFilesFromFolders(folders, featureId);
                    _foldersLogic.DeleteFolder(folders, featureId);
                    _logger.LogInformation("Deleted folders and associated files for feature ID {FeatureId}", featureId);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting folders and associated files for feature ID {FeatureId}", featureId);
                if (ex.InnerException != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }
    }
}
