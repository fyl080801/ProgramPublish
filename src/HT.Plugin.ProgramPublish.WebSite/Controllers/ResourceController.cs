using CACS.Framework.Domain;
using CACS.Framework.Identity;
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
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class ResourceController : CACSController
    {
        IRepository<Resource> _resourceRepository;
        IRepository<ResourceThumb> _thumbRepository;
        IProfileManager _profileManager;
        IThumbService _thumbService;
        ApplicationUserManager _userManager;
        ApplicationSignInManager _signinManager;

        public ResourceController(
            IRepository<Resource> resourceRepository,
            IRepository<ResourceThumb> thumbRepository,
            IProfileManager profileManager,
            IThumbService thumbService,
            HttpContextBase httpContext)
        {
            _resourceRepository = resourceRepository;
            _thumbRepository = thumbRepository;
            _profileManager = profileManager;
            _thumbService = thumbService;
            _userManager = httpContext.GetOwinContext().Get<ApplicationUserManager>();
            _signinManager = httpContext.GetOwinContext().Get<ApplicationSignInManager>();
        }

        /// <summary>
        /// 分片数据上传
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AccountTicket]
        public ActionResult Upload(UploadModel model)
        {
            if (HttpContext.Request.Files.Count <= 0) throw new CACSException("未选择上传文件");

            var profile = _profileManager.Get<ResourceProfile>();
            var tempPath = profile.UploadTemp + "\\" + model.Ruid;//分片文件存储命名
            if (!Directory.Exists(profile.UploadTemp)) Directory.CreateDirectory(profile.UploadTemp);
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            var file = HttpContext.Request.Files[0];//获取分片文件信息
            file.SaveAs(tempPath + "\\" + file.FileName + "." + model.Part);//存储分片数据成分片文件

            return Content("");
        }

        /// <summary>
        /// 合并文件分片
        /// 上传完毕后合并成实际文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AccountTicket]
        public ActionResult Merge(ResourceUploadModel model)
        {
            var profile = _profileManager.Get<ResourceProfile>();
            if (!Directory.Exists(profile.Path)) Directory.CreateDirectory(profile.Path);

            //获取存储的分片文件集合
            var files = Directory.GetFiles(profile.UploadTemp + "\\" + model.Ruid)
                .Select(e => new KeyValuePair<int, string>(Convert.ToInt32(e.Substring(e.LastIndexOf(".") + 1)), e));
            var resourcePath = profile.Path;
            var fileName = Guid.NewGuid() + "." + model.Ext;
            var resourceFile = profile.Path + "\\" + fileName;
            using (var fs = new FileStream(resourceFile, FileMode.Create))
            {
                foreach (var part in files.OrderBy(e => e.Key))
                {
                    var bytes = System.IO.File.ReadAllBytes(part.Value);
                    fs.Write(bytes, 0, bytes.Length);
                    bytes = null;
                }
            }
            Directory.Delete(profile.UploadTemp + "\\" + model.Ruid, true);//删除分片文件
            return Json(fileName);
        }

        /// <summary>
        /// 保存文件记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AccountTicket(AuthorizeId = "/ProgramPublish/Resource/List")]
        public ActionResult SaveUpload(ResourceUploadModel model)
        {
            var profile = _profileManager.Get<ResourceProfile>();
            if (!Directory.Exists(profile.Path)) Directory.CreateDirectory(profile.Path);

            var tempPath = profile.UploadTemp + "\\" + model.Ruid;
            var files = Directory.GetFiles(tempPath)
                .Select(e => new KeyValuePair<int, string>(Convert.ToInt32(e.Substring(e.LastIndexOf(".") + 1)), e));
            var resourcePath = profile.Path;
            var fileName = Guid.NewGuid() + "." + model.Ext;
            var resourceFile = profile.Path + "\\" + fileName;
            using (var fs = new FileStream(resourceFile, FileMode.Create))
            {
                foreach (var part in files.OrderBy(e => e.Key))
                {
                    var bytes = System.IO.File.ReadAllBytes(part.Value);
                    fs.Write(bytes, 0, bytes.Length);
                    bytes = null;
                }
            }
            Directory.Delete(tempPath, true);
            var domain = new Resource()
            {
                Content = fileName,
                Name = model.Name,
                UploadTime = DateTime.Now,
                Mime = ResourceHelper.ExtToMime(model.Ext),
                UserId = Convert.ToInt32(HttpContext.User.Identity.GetUserId()),
                CategoryId = ResourceHelper.ExtToCategory(model.Ext)
            };
            _resourceRepository.Insert(domain);
            _thumbRepository.Insert(new ResourceThumb()
            {
                Id = domain.Id,
                Thumb = _thumbService.BuildThumb(resourceFile)
            });
            return Json(true);
        }

        [AccountTicket(AuthorizeName = "管理", Group = "素材管理")]
        public ActionResult List(ListModel model)
        {
            var query = _resourceRepository.Table;
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Name.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.UploadTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<Resource>(query, model.Page - 1, model.Limit)
                ?? new PagedList<Resource>(new List<Resource>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (Resource)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket]
        public ActionResult Delete(int id)
        {
            var resource = _resourceRepository.GetById(id);
            if (resource == null) throw new CACSException("未找到素材");
            var profile = _profileManager.Get<ResourceProfile>();
            var filePath = profile.Path + "\\" + resource.Content;
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            _resourceRepository.Delete(resource);
            return Json(true);
        }

        /// <summary>
        /// 缩略图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AccountTicket]
        public ActionResult Thumb(int id)
        {
            var domain = _thumbRepository.GetById(id);
            return File(domain.Thumb, "image/*");
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AccountTicket]
        public ActionResult Download(int id)
        {
            var profile = _profileManager.Get<ResourceProfile>();
            var domain = _resourceRepository.GetById(id);
            return File(profile.Path + "\\" + domain.Content, domain.Mime);
        }
    }
}