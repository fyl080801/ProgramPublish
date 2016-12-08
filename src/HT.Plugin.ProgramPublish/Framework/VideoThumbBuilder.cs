using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;
using CACSLibrary.Profile;
using HT.Plugin.ProgramPublish.Profiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class VideoThumbBuilder : IThumbBuilder
    {
        string _path;

        public VideoThumbBuilder(string path)
        {
            _path = path;
        }

        public byte[] Build()
        {
            string output = "";
            string error = "";
            var finder = EngineContext.Current.Resolve<IPluginFinder>();
            var plugin = finder.GetPluginDescriptorById(Plugin.SYSTEM_ID, true);
            var profile = EngineContext.Current.Resolve<IProfileManager>().Get<ResourceProfile>();

            string ffmpegPath = plugin.PluginFile.DirectoryName + "\\ffmpeg.exe";
            string thubImagePath = profile.UploadTemp + _path.Substring(_path.LastIndexOf('\\'));
            thubImagePath = thubImagePath.Substring(0, thubImagePath.LastIndexOf(".")) + ".jpg";

            int frameIndex = 10;//为帧处在的秒数  
            string command = string.Format("\"{0}\" -i \"{1}\" -ss {2} -vframes 1 -r 1 -ac 1 -ab 2 -f image2 \"{3}\"", ffmpegPath, _path, frameIndex, thubImagePath);
            Execute(command, out output, out error);
            var result = ImageHelper.GetThumbnail(thubImagePath);
            File.Delete(thubImagePath);
            return result;
        }

        private void Execute(string command, out string output, out string error)
        {
            try
            {
                //创建一个进程
                Process pc = new Process();
                pc.StartInfo.FileName = command;
                pc.StartInfo.UseShellExecute = false;
                pc.StartInfo.RedirectStandardOutput = true;
                pc.StartInfo.RedirectStandardError = true;
                pc.StartInfo.CreateNoWindow = true;
                pc.Start();
                string outputData = string.Empty;
                string errorData = string.Empty;
                pc.BeginOutputReadLine();
                pc.BeginErrorReadLine();

                pc.OutputDataReceived += (ss, ee) =>
                {
                    outputData += ee.Data;
                };
                pc.ErrorDataReceived += (ss, ee) =>
                {
                    errorData += ee.Data;
                };

                pc.WaitForExit();
                pc.Close();
                output = outputData;
                error = errorData;
            }
            catch (Exception)
            {
                output = null;
                error = null;
            }
        }
    }
}
