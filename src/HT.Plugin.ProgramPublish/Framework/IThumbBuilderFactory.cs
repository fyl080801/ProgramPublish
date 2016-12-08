using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public interface IThumbBuilderFactory
    {
        IThumbBuilder Build(string path);
    }
}
