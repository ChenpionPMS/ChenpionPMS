using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChenpionPMS.Models
{
    /// <summary>
    /// 選單項目
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 編號
        /// </summary>
        /// <returns></returns>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 選單名稱
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 選單標題
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }

        /// <summary>
        /// 連結
        /// </summary>
        /// <returns></returns>
        public string Href { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        /// <returns></returns>
        public string Icon { get; set; }

        /// <summary>
        /// 順序
        /// </summary>
        /// <returns></returns>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        /// <returns></returns>
        public bool IsActive { get; set; }

        /// <summary>
        /// 子選單
        /// </summary>
        /// <returns></returns>
        public List<MenuItem> SubMenuItems { get; set; }

        /// <summary>
        /// 【關聯表】父選單
        /// </summary>
        /// <returns></returns>
        public int? MenuItemId { get; set; }
    }
}
