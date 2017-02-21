using CACS.Framework.Domain;
using CACS.Framework.Identity;
using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACSLibrary;
using CACSLibrary.Data;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.WebSite.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class GroupUserController : CACSController
    {
        ApplicationUserManager _userManager;
        IRepository<GroupUser> _groupuserRepository;

        public GroupUserController(
            ApplicationUserManager userManager,
            IRepository<GroupUser> groupuserRepository)
        {
            _userManager = userManager;
            _groupuserRepository = groupuserRepository;
        }

        [AccountTicket(AuthorizeId = "/User/Save")]
        public ActionResult Save(GroupUserInfo model)
        {
            var sysuser = _userManager.FindById(model.Id);
            sysuser = sysuser == null ? new CACS.Framework.Domain.User() : sysuser;
            sysuser.Email = model.Email;
            sysuser.FirstName = model.FirstName;
            sysuser.LastName = model.LastName;
            sysuser.UserName = model.Username;
            if (sysuser.Id > 0)
            {
                _userManager.Update(sysuser);
            }
            else
            {
                _userManager.Create(sysuser, sysuser.UserName);
            }

            if (model.GroupId.HasValue)
            {
                var user = _groupuserRepository.GetById(model.Id);
                if (user != null)
                {
                    user.GroupId = model.GroupId.Value;
                    _groupuserRepository.Update(user);
                }
                else
                {
                    user = new GroupUser();
                    user.User = sysuser;
                    user.GroupId = model.GroupId.Value;
                    _groupuserRepository.Insert(user);
                }
            }
            else
            {
                var user = _groupuserRepository.GetById(model.Id);
                if (user != null)
                {
                    _groupuserRepository.Delete(user);
                }
            }
            return Json(sysuser.Id);
        }

        [AccountTicket]
        public ActionResult Load(int id)
        {
            var user = _groupuserRepository.GetById(id);
            var groupuser = new GroupUserInfo();
            if (user == null)
            {
                var sysuser = _userManager.FindById(id);
                groupuser.Email = sysuser.Email;
                groupuser.FirstName = sysuser.FirstName;
                groupuser.Id = sysuser.Id;
                groupuser.LastName = sysuser.LastName;
                groupuser.Username = sysuser.UserName;
            }
            else
            {
                groupuser.Email = user.User.Email;
                groupuser.FirstName = user.User.FirstName;
                groupuser.GroupId = user.GroupId;
                groupuser.GroupName = user.Group.Name;
                groupuser.Id = user.Id;
                groupuser.LastName = user.User.LastName;
                groupuser.Username = user.User.UserName;
            }

            return Json(groupuser);
        }
    }
}