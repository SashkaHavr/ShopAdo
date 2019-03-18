using DAL;
using SimpleApp.Infrastructure;
using SimpleApp.Models;
using SimpleApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleApp.ViewModels
{
    abstract class TabViewModelBase<T> : NotifiedModelBase where T : NotifiedModelBase, new()
    {
        protected IRepository<T> repository;
        ObservableCollection<T> objects;
        T selectedGood;
        bool connected = false;
        bool isHandling = false;
        string searchString;
        ICommand connectCommand;
        ICommand disconnectCommand;
        ICommand addCommand;
        ICommand deleteCommand;

        public ObservableCollection<T> Objects { get => objects; private set { objects = value; Notify(); } }
        public T SelectedObj { get => selectedGood; set { selectedGood = value; Notify(); } }
        public string SearchString { get => searchString; set { searchString = value; Notify(); } }
        public bool Connected { get => connected; set { connected = value; Notify(); } }
        public ICommand ConnectCommand { get => connectCommand != null ? connectCommand : connectCommand = new RelayCommand(Connect, o => !connected); }
        public ICommand DisconnectCommand { get => disconnectCommand != null ? disconnectCommand : disconnectCommand = new RelayCommand(Disconnect, o => connected); }
        public ICommand AddCommand { get => addCommand != null ? addCommand : addCommand = new RelayCommand(Add, o => connected); }
        public ICommand DeleteCommand { get => deleteCommand != null ? deleteCommand : deleteCommand = new RelayCommand(Delete); }

        public TabViewModelBase(IRepository<T> repository)
        {
            this.repository = repository;
            PropertyChanged += SearchStringChangedHandler;
        }

        void Connect(object o)
        {
            string dataSource = o as string;
            if (repository.ConnectOrLoad(dataSource))
            {
                Connected = true;
                Objects = new ObservableCollection<T>(repository.GetAll());
                foreach (var obj in objects)
                    obj.PropertyChanged += GoodChangedHandler;
            }
        }

        void Disconnect(object o)
        {
            repository.DisconectOrSave();
            Connected = false;
            objects.Clear();
        }

        abstract protected void ReUpdateAllProperties(T obj);
        abstract protected void SetId(T obj, int id);
        abstract protected int GetId(T obj);

        void Add(object o)
        {
            T obj = new T();
            int id = repository.Add(obj);
            SetId(obj, id);
            obj.PropertyChanged += GoodChangedHandler;
            ReUpdateAllProperties(obj);
            objects.Add(obj);
        }

        void Delete(object o)
        {
            T obj = o as T;
            try
            {
                repository.Delete(GetId(obj));
                objects.Remove(obj);
            }
            catch
            { }
        }

        void GoodChangedHandler(object o, PropertyChangedEventArgs e)
        {
            if (!isHandling)
            {
                isHandling = true;
                T obj = o as T;
                repository.Update(obj);
                ReUpdateAllProperties(obj);
                isHandling = false;
            }
        }

        abstract protected bool SearchPredicate(T obj);

        void SearchStringChangedHandler(object o, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchString")
                Objects = new ObservableCollection<T>(repository.GetAll().Where(SearchPredicate));
        }
    }
}
