using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortsApi
{
    public class File
    {
        public int ID { get; set; }
        public string? FileName { get; set; }
        public int? FolderID { get; set; }
        public int? LayerID { get; set; }
        public int? FeatureID { get; set; }
        public string? FileID { get; set; }
        [NotMapped]
        public IFormFile? Attachment { get; set; }
        public string? FolderName { get; set; }
        public string[]? SelectedFolderIds { get; set; }
        [NotMapped]
        public FileContent? FileContent { get; set; }
    }
}
