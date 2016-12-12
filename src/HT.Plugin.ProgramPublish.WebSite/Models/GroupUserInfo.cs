using CACS.Framework.Domain;
using CACS.Framework.Mvc;
using HT.Plugin.ProgramPublish.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class GroupUserInfo : BaseEntityModel<int>
    {
        [Required]
        public string Username { get; set; }

        public string PersonalName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? GroupId { get; set; }

        public string GroupName { get; set; }

        public GroupUser ToDomain()
        {
            return new GroupUser()
            {
                Id = this.Id,
                GroupId = this.GroupId.Value,
                User = new User()
                {
                    Id = this.Id,
                    UserName = this.Username,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Email = this.Email
                }
            };
        }
    }
}