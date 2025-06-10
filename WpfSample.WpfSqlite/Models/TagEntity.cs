using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WpfSample.WpfSqlite.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class TagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<TaskTagEntity> TaskTagEntitys { get; set; } = new List<TaskTagEntity>();
    }
}
