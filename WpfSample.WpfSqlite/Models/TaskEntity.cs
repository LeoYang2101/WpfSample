using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WpfSample.WpfSqlite.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskEntity
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int TaskId { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; }

            public string Description { get; set; }

            public DateTime? DueDate { get; set; }

            [Range(1, 3)]
            public int Priority { get; set; } = 2; // 默认中优先级

            [Range(0, 3)]
            public int Status { get; set; } = 0; // 默认未开始

            public int? CategoryId { get; set; }

            [ForeignKey("CategoryId")]
            public virtual CategoryEntity Category { get; set; }

            public bool IsDeleted { get; set; } = false;

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public DateTime UpdatedAt { get; set; } = DateTime.Now;

            public virtual ICollection<SubTaskEntity> SubTaskEntitys { get; set; } = new List<SubTaskEntity>();

            public virtual ICollection<TaskTagEntity> TaskTagEntitys { get; set; } = new List<TaskTagEntity>();

            [NotMapped]
            public IEnumerable<TagEntity> TagEntitys => TaskTagEntitys?.Select(tt => tt.TagEntity);
        
    }
}
