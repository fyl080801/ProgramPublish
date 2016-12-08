using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Profile;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.Framework;
using HT.Plugin.ProgramPublish.Profiles;
using HT.Plugin.ProgramPublish.WebSite.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class ProgramController : CACSController
    {
        IRepository<Program> _programRepository;
        IRepository<ProgramResource> _resourceRepository;
        IRepository<ProgramExamine> _examineRepository;
        IProfileManager _profileManager;

        public ProgramController(
            IRepository<Program> programRepository,
            IRepository<ProgramResource> resourceRepository,
            IRepository<ProgramExamine> examineRepository,
            IProfileManager profileManager)
        {
            _programRepository = programRepository;
            _resourceRepository = resourceRepository;
            _examineRepository = examineRepository;
            _profileManager = profileManager;
        }

        [AccountTicket(AuthorizeName = "列表", Group = "节目管理")]
        public ActionResult Programs(ListModel model)
        {
            var query = _programRepository.Table.Where(e => e.State != ProgramStates.审批中);
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Name.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.UpdateTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<Program>(query, model.Page - 1, model.Limit)
                ?? new PagedList<Program>(new List<Program>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (Program)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket(AuthorizeName = "审核", Group = "节目管理")]
        public ActionResult Examines(ListModel model)
        {
            var query = _programRepository.Table.Where(e => e.State == ProgramStates.审批中);
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Name.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.UpdateTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<Program>(query, model.Page - 1, model.Limit)
                ?? new PagedList<Program>(new List<Program>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (Program)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Terminal/SetPrograms")]
        public ActionResult Publishable(ListModel model)
        {
            var query = _programRepository.Table.Where(e => e.State == ProgramStates.通过 && e.Enabled == true);
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Name.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.UpdateTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<Program>(query, model.Page - 1, model.Limit)
                ?? new PagedList<Program>(new List<Program>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (Program)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket]
        public ActionResult ExamineRecord(ListModel model, int id)
        {
            var query = _examineRepository.Table.Where(e => e.ProgramId == id);
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Remark.Contains(model.Search) || e.State.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.ExamineTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<ProgramExamine>(query, model.Page - 1, model.Limit)
                ?? new PagedList<ProgramExamine>(new List<ProgramExamine>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (ProgramExamine)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket(AuthorizeName = "查看", Group = "节目管理")]
        public ActionResult Details(int id)
        {
            var domain = _programRepository.GetById(id);
            return Json(domain.Clone());
        }

        [AccountTicket(AuthorizeName = "编辑", Group = "节目管理")]
        public ActionResult Save(Program model)
        {
            var domain = _programRepository.GetById(model.Id);
            if (domain == null)
            {
                model.State = ProgramStates.新建立;
                model.IsUpdated = true;
                model.UpdateTime = DateTime.Now;
                _programRepository.Insert(model);

                return Json(model.Id);
            }
            else
            {
                domain.Enabled = model.Enabled;
                domain.EndTime = model.EndTime;
                domain.Name = model.Name;
                domain.Remark = model.Remark;
                domain.StartTime = model.StartTime;
                domain.IsUpdated = true;
                domain.UpdateTime = DateTime.Now;
                domain.Template = model.Template;
                _programRepository.Update(domain);

                return Json(domain.Id);
            }
        }

        [AccountTicket(AuthorizeName = "删除", Group = "节目管理")]
        public ActionResult Delete(int id)
        {
            var domain = _programRepository.GetById(id);
            _programRepository.Delete(domain);
            return Json(true);
        }

        [AccountTicket(AuthorizeName = "素材", Group = "节目管理")]
        public ActionResult Resources(ResourceListModel model, int id)
        {
            var query = _resourceRepository.Table.Where(e => e.ProgramId == id);
            if (model.Category.HasValue)
            {
                query = query.Where(e => e.CategoryId == (ResourceCategories)model.Category.Value);
            }
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Content.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.Id);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<ProgramResource>(query, model.Page - 1, model.Limit)
                ?? new PagedList<ProgramResource>(new List<ProgramResource>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (ProgramResource)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddResource(ProgramResource model)
        {
            _resourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddFile(ProgramResource model)
        {
            model.CategoryId = ResourceHelper.ExtToCategory(model.Content.Substring(model.Content.LastIndexOf(".") + 1));
            _resourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddText(ProgramResource model)
        {
            model.CategoryId = ResourceCategories.文字;
            _resourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddLink(ProgramResource model)
        {
            model.CategoryId = ResourceCategories.网址;
            _resourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult DeleteResource(int id)
        {
            var domain = _resourceRepository.GetById(id);
            _resourceRepository.Delete(domain);
            return Json(true);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Save")]
        public ActionResult ToExamine(int id)
        {
            var domain = _programRepository.GetById(id);
            domain.State = ProgramStates.审批中;
            _programRepository.Update(domain);
            return Json(id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Examines")]
        public ActionResult DoExamine(ExamineModel model)
        {
            var domain = _programRepository.GetById(model.Id);
            domain.State = (ProgramStates)model.State;
            _examineRepository.Insert(new ProgramExamine()
            {
                ExamineTime = DateTime.Now,
                ProgramId = model.Id,
                Remark = model.Comment,
                UserId = Convert.ToInt32(HttpContext.User.Identity.GetUserId()),
                State = domain.State.ToString()
            });
            _programRepository.Update(domain);
            return Json(model.Id);
        }

        [AccountTicket]
        public ActionResult Templates()
        {
            var templates = _profileManager.Get<TemplateProfile>();
            return JsonList(templates.Templates.ToArray());
        }

        [AccountTicket]
        public ActionResult TemplateArea(Template model)
        {
            var template = _profileManager.Get<TemplateProfile>().Templates.FirstOrDefault(e => e.Name == model.Name);
            if (template == null)
                return JsonList(new Area[0]);
            else
                return JsonList(template.Areas.ToArray());
        }
    }
}