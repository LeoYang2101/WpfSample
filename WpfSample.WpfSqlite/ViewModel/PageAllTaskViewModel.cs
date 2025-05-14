using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSample.WpfSqlite.Data;
using WpfSample.WpfSqlite.Models;

namespace WpfSample.WpfSqlite.ViewModel
{
    public class PageAllTaskViewModel: ViewModelBase
    {
        private readonly AppDbContext _context;
        private string _newTitle;
        private string _newDescription;
        private TodoItem _selectedItem;

        public PageAllTaskViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // 确保数据库已创建

            LoadData();

            AddCommand = new RelayCommand(AddTodoItem);
            DeleteCommand = new RelayCommand(DeleteSelectedItem, CanDeleteItem);
            ToggleCompleteCommand = new RelayCommand(ToggleComplete);
        }

        public ObservableCollection<TodoItem> TodoItems { get; } = new ObservableCollection<TodoItem>();

        public string NewTitle
        {
            get => _newTitle;
            set => SetProperty(ref _newTitle, value);
        }

        public string NewDescription
        {
            get => _newDescription;
            set => SetProperty(ref _newDescription, value);
        }

        public TodoItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ToggleCompleteCommand { get; }

        private void LoadData()
        {
            TodoItems.Clear();
            foreach (var item in _context.TodoItems.OrderBy(t => t.CreatedDate))
            {
                TodoItems.Add(item);
            }
        }

        private void AddTodoItem()
        {
            if (string.IsNullOrWhiteSpace(NewTitle)) return;

            var newItem = new TodoItem
            {
                Title = NewTitle,
                Description = NewDescription,
                IsCompleted = false
            };

            _context.TodoItems.Add(newItem);
            _context.SaveChanges();

            TodoItems.Add(newItem);

            NewTitle = string.Empty;
            NewDescription = string.Empty;
        }

        private void DeleteSelectedItem()
        {
            if (SelectedItem == null) return;

            _context.TodoItems.Remove(SelectedItem);
            _context.SaveChanges();

            TodoItems.Remove(SelectedItem);
        }

        private bool CanDeleteItem() => SelectedItem != null;

        private void ToggleComplete()
        {
            if (SelectedItem == null) return;

            SelectedItem.IsCompleted = !SelectedItem.IsCompleted;
            _context.SaveChanges();
        }
    }
}
