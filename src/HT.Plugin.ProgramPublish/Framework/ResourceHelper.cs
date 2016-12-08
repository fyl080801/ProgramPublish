using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class ResourceHelper
    {
        public static ResourceCategories ExtToCategory(string ext)
        {
            switch (ext.ToLower())
            {
                case "flv":
                    return ResourceCategories.Flash;
                case "ppt":
                case "pptx":
                    return ResourceCategories.PPTX;
                case "png":
                case "gif":
                case "jpg":
                case "bmp":
                    return ResourceCategories.图片;
                case "avi":
                case "mp4":
                case "mpeg":
                case "mov":
                case "mkv":
                    return ResourceCategories.视频;
                default:
                    return ResourceCategories.图片;
            }
        }

        public static string ExtToMime(string ext)
        {
            switch (ext.ToLower())
            {
                case "flv":
                    return "application/x-shockwave-flash";
                case "ppt":
                case "pptx":
                    return "application/vnd.ms-powerpoint";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "jpg":
                    return "image/jpg";
                case "bmp":
                    return "image/bmp";
                case "avi":
                    return "video/x-msvideo";
                case "mp4":
                    return "video/mp4";
                case "mov":
                    return "video/quicktime";
                case "mpeg":
                    return "video/mpeg";
                case "mkv":
                    return "video/x-matroska";
                default:
                    return "";
            }
        }
    }
}
