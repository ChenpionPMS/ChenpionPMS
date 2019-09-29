using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChenpionPMS.Models
{
    public class Role : IdentityRole<String>
    {
        public Role() : base()
        {
        }

        public Role(string roleName) : base(roleName)
        {
            Name = roleName;
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        /// <summary>
        /// 是系統預設角色
        /// </summary>
        /// <value></value>
        public bool IsBuiltIn { get; set; }

        /// <summary>
        /// 對應目錄項目列表
        /// </summary>
        /// <value></value>
        public List<RoleMenuItem> RoleMenuItems { get; set; }
    }
}