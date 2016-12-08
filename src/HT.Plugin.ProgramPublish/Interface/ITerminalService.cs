using CACSLibrary;
using HT.Plugin.ProgramPublish.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Interface
{
    public interface ITerminalService
    {
        IPagedList<Terminal> GetTerminalByGroup(
            int groupId,
            string search,
            int index,
            int count,
            params KeyValuePair<string, string>[] sorts);
        
        IPagedList<Terminal> GetTerminalByGroups(
            int[] groups,
            string search,
            int index,
            int count,
            params KeyValuePair<string, string>[] sorts);

        IList<Terminal> GetTerminalByGroups(int[] groups);
    }
}
