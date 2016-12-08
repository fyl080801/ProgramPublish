using CACSLibrary;
using CACSLibrary.Data;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Services
{
    public class TerminalService : ITerminalService
    {
        IRepository<Group> _groupRepository;
        IRepository<Terminal> _terminalRepository;

        public TerminalService(
            IRepository<Group> groupRepository,
            IRepository<Terminal> terminalRepository)
        {
            _groupRepository = groupRepository;
            _terminalRepository = terminalRepository;
        }

        public IPagedList<Terminal> GetTerminalByGroup(
            int groupId,
            string search,
            int index,
            int count,
            params KeyValuePair<string, string>[] sorts)
        {
            var query = _terminalRepository.Table;
            var group = _groupRepository.GetById(groupId);
            var groups = _groupRepository.Table
                .Where(e => e.RelationPath.Contains(group.RelationPath))
                .Select(e => e.Id)
                .ToArray();
            query = query.Where(e => groups.Contains(e.GroupId));
            query = !string.IsNullOrEmpty(search) ? query.Where(e => e.Name.Contains(search)) : query;
            if (sorts == null || sorts.Length <= 0)
                query = query.OrderBy(e => e.Id);
            else
                sorts.ForEach(sort =>
                {
                    query = QueryBuilder.DataSorting(
                        query,
                        sort.Key,
                        sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false);
                });
            return new PagedList<Terminal>(query.Distinct(), index, count) ?? new PagedList<Terminal>(new List<Terminal>(), index, count);
        }

        public IPagedList<Terminal> GetTerminalByGroups(int[] groups, string search, int index, int count, params KeyValuePair<string, string>[] sorts)
        {
            var groupQuery = _groupRepository.Table
                .Where(e => groups.Contains(e.Id))
                .Select(e => e.RelationPath)
                .ToArray();
            var query = _terminalRepository.Table
                .Where(e => groupQuery.Count(g => e.Group.RelationPath.Contains(g)) > 0);
            query = !string.IsNullOrEmpty(search) ? query.Where(e => e.Name.Contains(search)) : query;
            if (sorts == null || sorts.Length <= 0)
                query = query.OrderBy(e => e.Id);
            else
                sorts.ForEach(sort =>
                {
                    query = QueryBuilder.DataSorting(
                        query,
                        sort.Key,
                        sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false);
                });
            return new PagedList<Terminal>(query.Distinct(), index, count) ?? new PagedList<Terminal>(new List<Terminal>(), index, count);
        }

        public IList<Terminal> GetTerminalByGroups(int[] groups)
        {
            var groupQuery = _groupRepository.Table
                .Where(e => groups.Contains(e.Id))
                .Select(e => e.RelationPath)
                .ToArray();
            var terminalQuery = _terminalRepository.Table
                .Where(e => groupQuery.Count(g => e.Group.RelationPath.Contains(g)) > 0)
                .Distinct();
            return terminalQuery.ToList();
        }
    }

    public class TerminalEqualCompare : EqualityComparer<Terminal>
    {
        public override bool Equals(Terminal x, Terminal y)
        {
            return x.Id == y.Id;
        }

        public override int GetHashCode(Terminal obj)
        {
            return obj.GetHashCode();
        }
    }
}
