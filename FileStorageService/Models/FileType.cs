using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageService.Models
{
    public static class FileType
    {
        public static string[] Media = { ".mp4", ".avi", ".h264", ".mpg", ".mpeg", ".m4v", ".mkv", ".mov" };
        public static string[] Image = { ".png", ".jpeg", ".jpg", ".bmp", ".gif" };
        public static string[] Document = { ".pdf", ".docx", ".dotx", ".txt",".doc" };


        public static string GetFileCategory(string fileExtension)
        {

            if (Image.Contains(fileExtension))
            {
                return "Images";
            }

            if (Media.Contains(fileExtension))
            {
                return "Media";
            }

            if (Document.Contains(fileExtension))
            {
                return "Documents";
            }

            return "Others";

        }

    }
}
