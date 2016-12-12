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
            var user = _groupuserRepository.GetById(model.Id);
            if (user != null)
            {
                if (model.GroupId.HasValue)
                {
                    user.GroupId = model.GroupId.Value;
                    user.User.Email = model.Email;
                    user.User.FirstName = model.FirstName;
                    user.User.LastName = model.LastName;
                    _groupuserRepository.Update(user);
                }
                else
                {
                    _groupuserRepository.Delete(user);
                }
            }
            else
            {
                var sysuser = _userManager.FindById(model.Id);
                user = new GroupUser();
                user.GroupId = model.GroupId.Value;
                if (sysuser != null)
                {
                    user.Id = sysuser.Id;
                }
                else
                {
                    user.User = new User()
                    {
                        UserName = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };
                }
                _groupuserRepository.Insert(user);
            }
            return Json(user.Id);
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