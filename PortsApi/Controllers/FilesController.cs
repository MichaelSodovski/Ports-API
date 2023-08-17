using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortsApi.Services;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace PortsApi
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FilesLogic _filesLogic;
        private readonly ILogger<FilesController> _logger;

        public FilesController(FilesLogic filesLogic, ILogger<FilesController> logger)
        {
            _filesLogic = filesLogic;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddFile")]
        public IActionResult AddFile([FromForm] File file)
        {
            _logger.LogInformation("Adding a file...");

            try
            {
                if (file.SelectedFolderIds == null || file.Attachment == null)
                {
                    _logger.LogWarning("Bad request: SelectedFolderIds or Attachment is null");
                    return BadRequest();
                }
                // get folder objects to extract the folder data to which the file suppose to be attached. 
                string[] jsonString = file.SelectedFolderIds;
                List<FolderObj> selectedFolders = JsonConvert.DeserializeObject<List<FolderObj>>(jsonString[0]);
                File addedFile = _filesLogic.AddFile(file, selectedFolders); // pass the file and the folders to which it needs to be attached. 
                _logger.LogInformation($"File with ID {addedFile.ID} added successfully");
                return Created("api/files/" + addedFile.ID, addedFile);
            }
            catch (Exception ex)
            {
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


        [Authorize]
        [HttpGet]
        [Route("GetFile")]
        public IActionResult GetFile([FromQuery] GetFileRequest fileFromRequest)
        {
            _logger.LogInformation("GetFile called with the following parameters: {FileFromRequest}", fileFromRequest);

            File selectedFile = new File
            {
                ID = Convert.ToInt32(fileFromRequest.id),
                FileName = fileFromRequest.fileName,
                FolderID = Convert.ToInt32(fileFromRequest.folderID),
                LayerID = Convert.ToInt32(fileFromRequest.layerID),
                FeatureID = Convert.ToInt32(fileFromRequest.featureID),
                FileID = fileFromRequest.fileID,
                FolderName = fileFromRequest.folderName,
            };

            try
            {
                if (selectedFile == null)
                {
                    _logger.LogWarning("Invalid file details provided: {SelectedFile}", selectedFile);
                    return BadRequest("Provide valid file details.");
                }

                File file = _filesLogic.GetFile(selectedFile);

                if (file == null || file.FileContent == null ||
                    file.FileContent.Content == null || file.FileContent.MimeType == null)
                {
                    _logger.LogWarning("File not found: {SelectedFile}", selectedFile);
                    return NotFound("File not found.");
                }

                _logger.LogInformation("File found and being sent: {File}", file);
                if(file.FileName == null)
                {
                    return BadRequest("no valid file name.");
                }

                var encodedFilename = Uri.EscapeDataString(file.FileName);
                var fileToReturn = File(file.FileContent.Content, file.FileContent.MimeType, encodedFilename);

                return fileToReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the file: {SelectedFile}", selectedFile);
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

       
        [HttpPost]
        [Route("GetFileNamesByFolderNames")]                
        public ActionResult<List<File>> GetFileNamesByFolderNames([FromBody] GetFileNamesByFolderNames payload)
        {
            _logger.LogInformation("GetFileNamesByFolderNames called with the following parameters: {Payload}", payload);
            try
            {
                List<File> fileNames = new List<File>();
                if (payload.FolderNames != null && payload.FeatureId > 0)
                {
                    fileNames = _filesLogic.GetFilesByFolderNamesAndFeatureId(payload.FolderNames, payload.FeatureId);
                    _logger.LogInformation("Files found: {FileNames}", fileNames);

                }
                else
                {
                    _logger.LogWarning("Invalid parameters provided: {Payload}", payload);
                }
                return Ok(fileNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the file names by folder names: {Payload}", payload);
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
        [Route("DeleteFiles")]
        public async Task<IActionResult> FileDelete([FromBody] DeleteFilesModel deleteFilesData)
        {
            _logger.LogInformation("FileDelete called with the following parameters: {DeleteFilesData}", deleteFilesData);

            if (deleteFilesData.files == null)
            {
                _logger.LogWarning("No files provided for deletion: {DeleteFilesData}", deleteFilesData);
                return NoContent();
            }

            File[] files = deleteFilesData.files;
            int featureId = deleteFilesData.featureId;

            try
            {
                await _filesLogic.DeleteFiles(files, featureId);
                _logger.LogInformation("Files successfully deleted: {FilesToDelete}", files);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting files: {DeleteFilesData}", deleteFilesData);
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


