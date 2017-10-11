using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class PowerpointThumbBuilder : IThumbBuilder
    {
        string _path;

        public PowerpointThumbBuilder(string path)
        {
            _path = path;
        }

        public byte[] Build()
        {
            try
            {
                var data = new byte[0];
                var app = new Microsoft.Office.Interop.PowerPoint.Application();
                var ppt = app.Presentations.Open(_path, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
                if (ppt.Slides.Count > 0)
                {
                    string pptjpgfile = _path.Substring(0, _path.LastIndexOf('.') + 1) + ".jpg";
                    ppt.Slides[1].Export(pptjpgfile, "jpg");
                    data = ImageHelper.GetThumbnail(pptjpgfile);
                    File.Delete(pptjpgfile);

                }
                ppt.Close();
                app.Quit();

                return data;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }
    }
}
