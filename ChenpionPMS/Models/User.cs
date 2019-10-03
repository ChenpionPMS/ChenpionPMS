using System;
using Microsoft.AspNetCore.Identity;

namespace ChenpionPMS.Models
{
    public class User : IdentityUser<String>, IModel
    {
        /// <summary>
        /// 頭像
        /// </summary>
        /// <value></value>
        public string Avatar { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        /// <value></value>
        public string LastName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// <value></value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        /// <value></value>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 刪除時間
        /// </summary>
        /// <value></value>
        public DateTime? DeletedAt { get; set; }

        public string FullName
        {
            get
            {
                return this.LastName + this.FirstName;
            }
        }
    }
}