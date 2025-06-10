using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfSample.WpfSqlite.Data;
using WpfSample.WpfSqlite.Models;

namespace WpfSample.WpfSqlite.ViewModel
{
    internal class PageTaskManagementViewModel:ViewModelBase
    {        
            private readonly AppDbContext _dbContext;
            private ObservableCollection<TaskEntity> _tasks;
            private TaskEntity _selectedTask;
            private ObservableCollection<TagViewModel> _tags;
            private string _addTagText;

            public ObservableCollection<TaskEntity> Tasks
            {
                get => _tasks;
                set
                {
                    _tasks = value;
                    OnPropertyChanged();
                }
            }

            public ObservableCollection<TagViewModel> Tags
            {
                get => _tags;
                set
                {
                    _tags = value;
                    OnPropertyChanged();
                }
            }

            public string AddTagText
            {
                get => _addTagText;
                set
                {
                    _addTagText = value;
                    OnPropertyChanged();
                }
            }

            public ICommand AddCommand { get; }
            public ICommand EditCommand { get; }
            public ICommand DeleteCommand { get; }
            public ICommand SaveCommand { get; }
            public ICommand CancelCommand { get; }
            public ICommand AddTagCommand { get; }

            public PageTaskManagementViewModel()
            {
                _dbContext = new AppDbContext();
            
                _dbContext.Database.EnsureCreated();

                LoadTasks();
                LoadTags();

                AddCommand = new RelayCommand(AddTask);
                EditCommand = new RelayCommand(EditTask, CanEditOrDelete);
                DeleteCommand = new RelayCommand(DeleteTask, CanEditOrDelete);
                SaveCommand = new RelayCommand(SaveChanges);
                // CancelCommand = new RelayCommand(CancelChanges);
                AddTagCommand = new RelayCommand(AddTag);
            }

            private void LoadTags()
            {
                var allTags = _dbContext.Tags.ToList();
                Tags = new ObservableCollection<TagViewModel>(
                    allTags.Select(t => new TagViewModel
                    {
                        TagId = t.TagId,
                        Name = t.Name,
                        IsSelected = false
                    }));
            }

            private void AddTag()
            {
                if (string.IsNullOrWhiteSpace(AddTagText))
                    return;
                    
                // 检查是否已存在相同名称的标签
                if (Tags.Any(t => t.Name.Equals(AddTagText, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("已存在相同名称的标签！");
                    return;
                }
                
                // 创建新标签并保存到数据库
                var newTag = new TagEntity
                {
                    Name = AddTagText
                };
                
                _dbContext.Tags.Add(newTag);
                _dbContext.SaveChanges();
                
                // 添加到UI集合
                Tags.Add(new TagViewModel
                {
                    TagId = newTag.TagId,
                    Name = newTag.Name,
                    IsSelected = false
                });
                
                // 清空输入框
                AddTagText = string.Empty;
            }

            private void LoadTasks()
            {
                Tasks = new ObservableCollection<TaskEntity>(
                    _dbContext.Tasks
                        .Include(t => t.Category)
                        .Include(t => t.SubTaskEntitys)
                        .Include(t => t.TaskTagEntitys)
                        .ThenInclude(tt => tt.TagEntity)
                        .ToList());
            }

            private bool CanEditOrDelete() => IsTaskSelected;

        private void AddTask()
        {
            var newTask = new TaskEntity
            {
                Title = "新任务",
                Description = "",
                DueDate = DateTime.Today,
                Priority = 2,
                Status = 0
            };

            _dbContext.Tasks.Add(newTask);
            Tasks.Add(newTask);
            SelectedTask = newTask;
            _dbContext.SaveChanges();
        }

            private void EditTask()
            {
                if (SelectedTask != null)
                {
                    _dbContext.Entry(SelectedTask).State = EntityState.Modified;
                }
            }

            private void DeleteTask()
            {
                if (SelectedTask != null)
                {
                    // 软删除
                    SelectedTask.IsDeleted = true;
                    _dbContext.Entry(SelectedTask).State = EntityState.Modified;
                    Tasks.Remove(SelectedTask);
                    SelectedTask = null;
                }
            }

            private void SaveChanges()
            {
                try
                {
                    if (SelectedTask != null)
                    {
                        // 更新任务标签关系
                        UpdateTaskTags();
                    }
                    
                    _dbContext.SaveChanges();
                    LoadTasks(); // 刷新列表
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存失败: {ex.Message}");
                }
            }

            private void UpdateTaskTags()
            {
                if (SelectedTask == null) return;
                
                // 获取当前选中的标签
                var selectedTagIds = Tags.Where(t => t.IsSelected).Select(t => t.TagId).ToList();
                
                // 获取任务当前的标签关系
                var currentTaskTags = _dbContext.TaskTags
                    .Where(tt => tt.TaskId == SelectedTask.TaskId)
                    .ToList();
                
                // 需要删除的标签关系
                var tagsToRemove = currentTaskTags
                    .Where(tt => !selectedTagIds.Contains(tt.TagId))
                    .ToList();
                
                // 需要添加的标签关系
                var existingTagIds = currentTaskTags.Select(tt => tt.TagId).ToList();
                var tagsToAdd = selectedTagIds
                    .Where(id => !existingTagIds.Contains(id))
                    .Select(id => new TaskTagEntity
                    {
                        TaskId = SelectedTask.TaskId,
                        TagId = id
                    })
                    .ToList();
                
                // 执行删除和添加操作
                foreach (var tag in tagsToRemove)
                {
                    _dbContext.TaskTags.Remove(tag);
                }
                
                foreach (var tag in tagsToAdd)
                {
                    _dbContext.TaskTags.Add(tag);
                }
            }
            
            // 当选择任务时，更新标签选中状态
            public TaskEntity SelectedTask
            {
                get => _selectedTask;
                set
                {
                    _selectedTask = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsTaskSelected));
                    
                    // 更新标签选中状态
                    if (_selectedTask != null)
                    {
                        UpdateTagSelection();
                    }
                }
            }
            
            private void UpdateTagSelection()
            {
                if (SelectedTask == null || Tags == null) return;
                
                // 获取当前任务的标签ID列表
                var taskTagIds = _dbContext.TaskTags
                    .Where(tt => tt.TaskId == SelectedTask.TaskId)
                    .Select(tt => tt.TagId)
                    .ToList();
                
                // 更新UI中标签的选中状态
                foreach (var tag in Tags)
                {
                    tag.IsSelected = taskTagIds.Contains(tag.TagId);
                }
            }

            public bool IsTaskSelected => SelectedTask != null;
 
            // 添加TagViewModel类
            public class TagViewModel : ViewModelBase
            {
                private int _tagId;
                private string _name;
                private bool _isSelected;
            
                public int TagId
                {
                    get => _tagId;
                    set => SetProperty(ref _tagId, value);
                }
            
                public string Name
                {
                    get => _name;
                    set => SetProperty(ref _name, value);
                }
            
                public bool IsSelected
                {
                    get => _isSelected;
                    set => SetProperty(ref _isSelected, value);
                }
            }
        }
    }
