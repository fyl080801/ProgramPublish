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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class ProgramController : CACSController
    {
        IRepository<Program> _programRepository;
        IRepository<ProgramResource> _programResourceRepository;
        IRepository<ProgramResourceThumb> _programResourceThumbRepository;
        IRepository<Resource> _resourceRepository;
        IRepository<ResourceThumb> _resourceThumbRepository;
        IRepository<ProgramExamine> _examineRepository;
        IRepository<GroupUser> _groupuserRepository;
        IProfileManager _profileManager;
        IRepository<ResourceThumb> _thumbRepository;
        IThumbService _thumbService;

        public ProgramController(
            IRepository<Program> programRepository,
            IRepository<ProgramResource> programResourceRepository,
            IRepository<ProgramResourceThumb> programResourceThumbRepository,
            IRepository<Resource> resourceRepository,
            IRepository<ResourceThumb> resourceThumbRepository,
            IRepository<ProgramExamine> examineRepository,
            IRepository<GroupUser> groupuserRepository,
            IRepository<ResourceThumb> thumbRepository,
            IThumbService thumbService,
            IProfileManager profileManager)
        {
            _programRepository = programRepository;
            _programResourceRepository = programResourceRepository;
            _programResourceThumbRepository = programResourceThumbRepository;
            _resourceRepository = resourceRepository;
            _resourceThumbRepository = resourceThumbRepository;
            _examineRepository = examineRepository;
            _groupuserRepository = groupuserRepository;
            _profileManager = profileManager;
            _thumbRepository = thumbRepository;
            _thumbService = thumbService;
        }

        [AccountTicket(AuthorizeName = "列表", Group = "节目管理")]
        public ActionResult Programs(ListModel model)
        {
            var query = _programRepository.Table.Where(e => e.State != ProgramStates.审批中);
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.Group.RelationPath.Contains(groupuser.Group.RelationPath)) : query;
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
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.Group.RelationPath.Contains(groupuser.Group.RelationPath)) : query;
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
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.Group.RelationPath.Contains(groupuser.Group.RelationPath)) : query;
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
        public ActionResult Rejects(ListModel model)
        {
            var query = _programRepository.Table.Where(e => e.State == ProgramStates.退回);
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.Group.RelationPath.Contains(groupuser.Group.RelationPath)) : query;
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
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            var domain = _programRepository.GetById(model.Id);
            if (string.IsNullOrEmpty(model.Weeks)) model.Weeks = "1,2,3,4,5,6,7";
            if (domain == null)
            {
                model.State = ProgramStates.新建立;
                model.IsUpdated = true;
                model.UpdateTime = DateTime.Now;
                model.GroupId = groupuser != null ? groupuser.GroupId : new int?();
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
                domain.Weeks = model.Weeks;
                _programRepository.Update(domain);

                var profile = _profileManager.Get<ResourceProfile>();
                if (!Directory.Exists(profile.TerminalFlag)) Directory.CreateDirectory(profile.TerminalFlag);
                var terminals = domain.Terminals.Select(e => e.TerminalCode).ToArray();
                foreach (var terminal in terminals)
                {
                    using (var sw = new StreamWriter(new FileStream(
                        string.Format("{0}/{1}.txt", profile.TerminalFlag, terminal),
                        FileMode.Create, FileAccess.ReadWrite)))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sw.Close();
                    }
                }

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
            var query = _programResourceRepository.Table.Where(e => e.ProgramId == id);
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
            var thumb = _resourceThumbRepository.GetById(model.Id);
            var resource = _resourceRepository.GetById(model.Id);
            model.Mime = resource.Mime;
            _programResourceRepository.Insert(model);
            _programResourceThumbRepository.Insert(new ProgramResourceThumb()
            {
                Id = model.Id,
                Thumb = thumb.Thumb
            });
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddFile(ProgramResource model, bool? addresource)
        {
            var profile = _profileManager.Get<ResourceProfile>();
            var resourceFile = profile.Path + "\\" + model.Content;
            var extname = model.Content.Substring(model.Content.LastIndexOf(".") + 1);
            var thumb = _thumbService.BuildThumb(resourceFile);

            model.CategoryId = ResourceHelper.ExtToCategory(extname);
            model.Mime = ResourceHelper.ExtToMime(extname);
            _programResourceRepository.Insert(model);
            _programResourceThumbRepository.Insert(new ProgramResourceThumb()
            {
                Id = model.Id,
                Thumb = thumb
            });

            if (addresource.HasValue && addresource == true)
            {
                var domain = new Resource()
                {
                    Content = model.Content,
                    Name = model.Name,
                    UploadTime = DateTime.Now,
                    Mime = model.Mime,
                    UserId = Convert.ToInt32(HttpContext.User.Identity.GetUserId()),
                    CategoryId = model.CategoryId
                };
                _resourceRepository.Insert(domain);
                _thumbRepository.Insert(new ResourceThumb()
                {
                    Id = domain.Id,
                    Thumb = thumb
                });
            }
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddText(ProgramResource model)
        {
            model.CategoryId = ResourceCategories.文字;
            _programResourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddLink(ProgramResource model)
        {
            model.CategoryId = ResourceCategories.网址;
            _programResourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddStream(ProgramResource model)
        {
            model.CategoryId = ResourceCategories.流媒体;
            _programResourceRepository.Insert(model);
            return Json(model.Id);
        }

        // 编辑节目时添加新素材
        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult AddNewResource(ProgramResource model)
        {
            _programResourceRepository.Insert(model);
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Program/Resources")]
        public ActionResult DeleteResource(int id)
        {
            var domain = _programResourceRepository.GetById(id);
            _programResourceRepository.Delete(domain);
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

        [AccountTicket]
        [HttpGet]
        public ActionResult ResourceThumb(int id)
        {
            var domain = _programResourceThumbRepository.GetById(id);
            return domain == null
                ? File(new byte[0], "image/*")
                : File(domain.Thumb, "image/*");
        }

        [AccountTicket]
        public ActionResult DownloadResource(int id)
        {
            var profile = _profileManager.Get<ResourceProfile>();
            var domain = _programResourceRepository.GetById(id);
            var path = profile.Path + "\\" + domain.Content;
            return string.IsNullOrEmpty(domain.Mime)
                ? File(path, "application/octet-stream", domain.Name)
                : File(path, domain.Mime, domain.Name);
        }
    }
}