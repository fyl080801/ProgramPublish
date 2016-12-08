using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class ImageThumbBuilder : IThumbBuilder
    {
        string _path;

        public ImageThumbBuilder(string path)
        {
            _path = path;
        }

        public byte[] Build()
        {
            return ImageHelper.GetThumbnail(_path);
        }
    }
}
