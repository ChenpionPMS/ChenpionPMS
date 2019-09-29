using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ChenpionPMS.Models
{
    /// <summary>
    /// 角色可存取選單項目
    /// </summary>
    public class RoleMenuItem
    {
        /// <summary>
        /// 【主鍵、外來鍵】角色編號
        /// </summary>
        /// <value></value>
        [Required]
        public string RoleId { get; set; }

        /// <summary>
        /// 【主鍵、外來鍵】目錄項目編號
        /// </summary>
        /// <value></value>
        [Required]
        public int MenuItemId { get; set; }

        /// <summary>
        /// 可檢視
        /// </summary>
        /// <value></value>
        public bool Visible { get; set; }

        /// <summary>
        /// 權限
        /// </summary>
        /// <value></value>
        public string Permission { get; set; }

        /// <summary>
        /// 鎖定權限，無法修改
        /// </summary>
        /// <value></value>
        public bool LockPermission { get; set; }

        /// <summary>
        /// 【關聯表】角色
        /// </summary>
        /// <value></value>
        public Role Role { get; set; }

        /// <summary>
        /// 【關聯表】目錄項目
        /// </summary>
        /// <value></value>
        public MenuItem MenuItem { get; set; }
    }
}