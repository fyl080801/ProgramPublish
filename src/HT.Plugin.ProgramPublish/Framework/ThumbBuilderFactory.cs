using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class ThumbBuilderFactory : IThumbBuilderFactory
    {
        public IThumbBuilder Build(string path)
        {
            string ext = path.Substring(path.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "flv":
                    return new VideoThumbBuilder(path);
                case "ppt":
                case "pptx":
                    return new DefaultThumbBuilder();
                case "png":
                case "gif":
                case "jpg":
                case "bmp":
                    return new ImageThumbBuilder(path);
                case "avi":
                case "mp4":
                case "mpeg":
                case "mov":
                case "mkv":
                    return new VideoThumbBuilder(path);
                default:
                    return new DefaultThumbBuilder();
            }
        }
    }
}
