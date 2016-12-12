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
            

            return new byte[0];
        }
    }
}
