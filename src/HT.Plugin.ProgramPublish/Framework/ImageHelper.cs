using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public sealed class ImageHelper
    {
        public static byte[] GetThumbnail(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            using (Image originalImage = Image.FromFile(path))
            {
                using (MemoryStream mstream = new MemoryStream())
                {
                    var img = GetThumbnail(originalImage, 300, Convert.ToInt32(originalImage.Height * (300.0 / originalImage.Width)));
                    img.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] byData = new byte[mstream.Length];
                    mstream.Position = 0;
                    mstream.Read(byData, 0, byData.Length);
                    return byData;
                }
            }
        }

        public static Image GetThumbnail(Image image, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.CompositingQuality = CompositingQuality.HighSpeed;
            gr.InterpolationMode = InterpolationMode.Low;
            Rectangle rectDestination = new Rectangle(0, 0, width, height);
            gr.DrawImage(image, rectDestination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            return bmp;
        }
    }
}
