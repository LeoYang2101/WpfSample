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

            public ObservableCollection<TaskEntity> Tasks
            {
                get => _tasks;
                set
                {
                    _tasks = value;
                    OnPropertyChanged();
                }
            }

            public TaskEntity SelectedTask
            {
                get => _selectedTask;
                set
                {
                    _selectedTask = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsTaskSelected));
                }
            }

            public bool IsTaskSelected => SelectedTask != null;

            public ICommand AddCommand { get; }
            public ICommand EditCommand { get; }
            public ICommand DeleteCommand { get; }
            public ICommand SaveCommand { get; }
            public ICommand CancelCommand { get; }

            public PageTaskManagementViewModel()
            {
                _dbContext = new AppDbContext();
            
                _dbContext.Database.EnsureCreated();

                LoadTasks();

                AddCommand = new RelayCommand(AddTask);
                EditCommand = new RelayCommand(EditTask, CanEditOrDelete);
                DeleteCommand = new RelayCommand(DeleteTask, CanEditOrDelete);
                SaveCommand = new RelayCommand(SaveChanges);
                CancelCommand = new RelayCommand(CancelChanges);
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
                    _dbContext.SaveChanges();
                    LoadTasks(); // 刷新列表
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存失败: {ex.Message}");
                }
            }

            private void CancelChanges()
            {
                var changedEntries = _dbContext.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Unchanged)
                    .ToList();

                foreach (var entry in changedEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        case EntityState.Modified:
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                    }
                }

                LoadTasks(); // 重新加载数据
            }
        }
    }
