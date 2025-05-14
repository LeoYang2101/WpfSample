using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample.WpfSqlite.ViewModel
{
    internal class ViewModel
    {

    }

    // 今日任务视图模型
    public class TodayTaskViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public int CompletedSubtasks { get; set; }
        public int TotalSubtasks { get; set; }
        public List<string> Tags { get; set; }
    }

    // 高优先级任务视图模型
    public class HighPriorityTaskViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string CategoryName { get; set; }
    }
}
