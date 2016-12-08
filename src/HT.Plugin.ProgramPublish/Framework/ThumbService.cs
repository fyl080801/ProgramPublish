using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Framework
{
    public class ThumbService : IThumbService
    {
        IThumbBuilderFactory _factory;

        public ThumbService()
        {
            _factory = new ThumbBuilderFactory();
        }

        public byte[] BuildThumb(string path)
        {
            return _factory.Build(path).Build();
        }
    }
}
