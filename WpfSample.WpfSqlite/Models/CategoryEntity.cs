using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WpfSample.WpfSqlite.Models
{
    /// <summary>
    /// 分类
    /// </summary>
    public class CategoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(7)] // #RRGGBB
        public string ColorCode { get; set; } = "#CCCCCC";

        public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
