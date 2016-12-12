using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACSLibrary.Data;
using HT.Plugin.ProgramPublish.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class GroupController : CACSController
    {
        IRepository<Group> _groupRepository;
        IRepository<Terminal> _terminalRepository;
        IRepository<Program> _programRepository;
        IRepository<GroupUser> _groupuserRepository;

        public GroupController(
            IRepository<Group> groupRepository,
            IRepository<Terminal> terminalRepository,
            IRepository<GroupUser> groupuserRepository,
            IRepository<Program> programRepository)
        {
            _groupRepository = groupRepository;
            _terminalRepository = terminalRepository;
            _programRepository = programRepository;
            _groupuserRepository = groupuserRepository;
        }

        [AccountTicket]
        public ActionResult List(ListModel model, int? parentId)
        {
            var query = _groupRepository.Table;
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.RelationPath.Contains(groupuser.Group.RelationPath) && e.RelationPath != groupuser.Group.RelationPath) : query;
            query = parentId.HasValue ? query.Where(e => e.ParentId == parentId.Value) : (groupuser != null ? query.Where(e => e.ParentId == groupuser.GroupId) : query.Where(e => !e.ParentId.HasValue));
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Name.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.Id);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            return Json(query
                .ToList()
                .Select(e => (Group)e.Clone())
                .ToArray());
        }

        [AccountTicket(Group = "设备", AuthorizeName = "分组管理")]
        public ActionResult Save(Group model)
        {
            var domain = _groupRepository.GetById(model.Id);
            var parent = _groupRepository.GetById(model.ParentId);
            if (domain != null)
            {
                domain.Name = model.Name;
                domain.ParentId = model.ParentId;
                domain.RelationPath = parent != null ? parent.RelationPath + domain.Id.ToString() + "/" : "/" + domain.Id.ToString() + "/";
                _groupRepository.Update(domain);
            }
            else
            {
                _groupRepository.Insert(model);
                model.RelationPath = parent != null ? parent.RelationPath + model.Id.ToString() + "/" : "/" + model.Id.ToString() + "/";
                _groupRepository.Update(model);
            }
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Group/Save")]
        public ActionResult Delete(int id)
        {
            var domain = _groupRepository.GetById(id);
            var deletes = _groupRepository.Table
                .Where(e => e.RelationPath.Contains(domain.RelationPath))
                .OrderByDescending(e => e.RelationPath)
                .ToList();
            deletes.Add(domain);
            if (domain != null)
            {
                _groupRepository.Delete(deletes.ToArray());
            }
            return Json(true);
        }

        [AccountTicket]
        public ActionResult All()
        {
            var query = _groupRepository.Table;
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.RelationPath.Contains(groupuser.Group.RelationPath)) : query;
            var result = query
                .ToList()
                .Select(e =>
                {
                    var clone = (Group)e.Clone();
                    if (groupuser != null)
                    {
                        clone.ParentId = e.Id == groupuser.GroupId ? clone.ParentId = null : clone.ParentId;
                    }
                    return clone;
                });
            return Json(result.ToArray());
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Terminal/SetPrograms")]
        public ActionResult SetPrograms(Program[] models, int id)
        {
            if (models == null) models = new Program[0];
            var domain = _groupRepository.GetById(id);
            var selects = models.Select(m => m.Id).ToArray();
            var groupTerminals = _terminalRepository.Table.Where(e => e.Group.RelationPath.Contains(domain.RelationPath)).ToList();
            var programs = _programRepository.Table.Where(e => selects.Contains(e.Id)).ToList();

            groupTerminals.ForEach(e =>
            {
                e.Programs.Clear();
                programs.ForEach(p => e.Programs.Add(p));
            });
            _terminalRepository.Update(groupTerminals.ToArray());
            return Json(true);
        }
    }
}