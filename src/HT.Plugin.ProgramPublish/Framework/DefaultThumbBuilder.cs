using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class DefaultThumbBuilder : IThumbBuilder
    {
        public byte[] Build()
        {
            return new byte[1] { 0x00 };
        }
    }
}
