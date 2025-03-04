using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample.WpfListViewDataLoading
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Student> _stuList;
        public ObservableCollection<Student> StuList
        {
            get { return _stuList; }
            set { _stuList = value; OnPropertyChanged(nameof(StuList)); }
        }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public MainWindowViewModel()
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
            Reset();

            _addCommand = new ButtonCommand(AddFunc, () => { return true; });
            _resetCommand = new ButtonCommand(ResetFunc, () => { return true; });
        }
        private void Reset()
        {
            StuList = new ObservableCollection<Student>();
            StuList.Add(new Student() { Id = 1, Name = "111" });
            StuList.Add(new Student() { Id = 2, Name = "222" });
            StuList.Add(new Student() { Id = 3, Name = "333" });
            StuList.Add(new Student() { Id = 4, Name = "444" });
            StuList.Add(new Student() { Id = 5, Name = "555" });
            StuList.Add(new Student() { Id = 6, Name = "666" });
            StuList.Add(new Student() { Id = 7, Name = "777" });
            StuList.Add(new Student() { Id = 8, Name = "888" });
            StuList.Add(new Student() { Id = 9, Name = "999" });
            StuList.Add(new Student() { Id = 10, Name = "10" });
        }


        private ButtonCommand _addCommand;
        public ButtonCommand AddCommand
        {
            get
            {
                return _addCommand;
            }
            set
            {
                _addCommand = value;
                OnPropertyChanged(nameof(AddCommand));
            }
        }

        private ButtonCommand _resetCommand;
        public ButtonCommand ResetCommand
        {
            get
            {
                return _resetCommand;
            }
            set
            {
                _resetCommand = value;
                OnPropertyChanged(nameof(ResetCommand));
            }
        }

        public void ResetFunc()
        {
            Reset();
        }

        public void AddFunc()
        {
            StuList.Add(new Student() { Id = 1, Name = "tom" });

        }


    }
    public class Student : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public long _id;
        public long Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }

        public string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        public string _date;
        public string Date { get { return _date; } set { _date = value; OnPropertyChanged(nameof(Date)); } }

        public string _color= "red";
        public string Color { get { return _color; } set { _color = value; OnPropertyChanged(nameof(Color)); } }

        public async void Update()
        {
            if (Color.Equals("green")) return;
            Color = "yellow";
            Date = DateTime.Now.ToString("HH:mm:mm fff");
            Debug.WriteLine($"触发更新 Id:{Id},Name:{Name},Date:{Date}");
            await Task.Delay(2000);
            Color = "green";
        }
    }
}
