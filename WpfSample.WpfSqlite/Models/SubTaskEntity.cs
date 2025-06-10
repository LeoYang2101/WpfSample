using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WpfSample.WpfSqlite.Models
{
    /// <summary>
    /// 子任务
    /// </summary>
    public class SubTaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubTaskId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskEntity TaskEntity { get; set; }
    }
}
