using System;

namespace ChenpionPMS.Models
{
    public interface IModel
    {
        /// <summary>
        /// 建立時間
        /// </summary>
        /// <value></value>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        /// <value></value>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 刪除時間
        /// </summary>
        /// <value></value>
        DateTime? DeletedAt { get; set; }
    }
}