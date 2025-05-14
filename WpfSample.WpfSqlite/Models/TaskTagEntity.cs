using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WpfSample.WpfSqlite.Models
{
    public class TaskTagEntity
    {
        [Key]
        public int Id { get; set; }

        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskEntity TaskEntity { get; set; }

        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public virtual TagEntity TagEntity { get; set; }
    }
}
