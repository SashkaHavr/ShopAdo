using DAL;
using DAL.Goods;
using DAL.Sales;
using SimpleApp.Infrastructure;
using SimpleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleApp.ViewModels
{
    class SaleViewModel : NotifiedModelBase
    {
        IRepository<Sale> saleRepository;
        IRepository<Good> goodRepository;
        ObservableCollection<Good> goods;
        Good selectedGood;
        ObservableCollection<Sale> sales;
        Sale selectedSale;
        ObservableCollection<SalePos> addingSalePositions = new ObservableCollection<SalePos>();
        ObservableCollection<SalePos> updatingSalePositions = new ObservableCollection<SalePos>();
        string goodSearchString;
        string saleSearchString;
        Sale prevSale;
        ICommand loadCommand;
        ICommand addGoodCommand;
        ICommand removeGoodCommand;
        ICommand sellCommand;
        ICommand removeSaleCommand;
        ICommand saveCommand;
        ICommand downArrowCommand;
        ICommand upArrowCommand;
        ICommand greenUpArrowCommand;

        public ObservableCollection<Good> Goods { get => goods; set { goods = value; Notify(); } }
        public ObservableCollection<Sale> Sales { get => sales; set { sales = value; Notify(); } }
        public Good SelectedGood { get => selectedGood; set { selectedGood = value; Notify(); } }
        public int CurrentSum { get => (int)AddingSalePositions.Sum(s => s.CountGood * s.Good.Price); }
        public ObservableCollection<SalePos> AddingSalePositions { get => addingSalePositions; set { addingSalePositions = value; Notify(); } }
        public ObservableCollection<SalePos> UpdatingSalePositions { get => updatingSalePositions; set { updatingSalePositions = value; Notify(); } }
        public string GoodSearchString { get => goodSearchString; set { goodSearchString = value; Notify(); } }
        public string SaleSearchString { get => saleSearchString; set { saleSearchString = value; Notify(); } }
        public Sale SelectedSale
        {
            get => selectedSale;
            set
            {
                if(selectedSale != null)
                    prevSale = selectedSale;
                selectedSale = value;
                Notify();
            }
        }
        public int TabId
        {
            set
            {
                if(value == 0)
                    Save(null);
            }
        }
        public bool IsSaleSelected { get => selectedSale != null; }
        public ICommand LoadCommand { get => loadCommand ?? (loadCommand = new RelayCommand(Load)); }
        public ICommand AddGoodCommand { get => addGoodCommand ?? (addGoodCommand = new RelayCommand(AddGood)); }
        public ICommand RemoveGoodCommand { get => removeGoodCommand ?? (removeGoodCommand = new RelayCommand(RemoveGood)); }
        public ICommand SellCommand { get => sellCommand ?? (sellCommand = new RelayCommand(Sell, o => addingSalePositions.Count > 0)); }
        public ICommand RemoveSaleCommand { get => removeSaleCommand ?? (removeSaleCommand = new RelayCommand(RemoveSale)); }
        public ICommand SaveCommand { get => saveCommand ?? (saveCommand = new RelayCommand(Save)); }
        public ICommand DownArrowCommand { get => downArrowCommand ?? (downArrowCommand = new RelayCommand(DownArrow)); }
        public ICommand UpArrowCommand { get => upArrowCommand ?? (upArrowCommand = new RelayCommand(UpArrow)); }
        public ICommand GreenUpArrowCommand { get => greenUpArrowCommand ?? (greenUpArrowCommand = new RelayCommand(GreenUpArrow)); }

        public SaleViewModel(IRepository<Sale> saleRepository, IRepository<Good> goodRepository)
        {
            this.saleRepository = saleRepository;
            this.goodRepository = goodRepository;
            PropertyChanged += PropertyChangedHandler;
        }

        void Load(object o)
        {
            string dataSource = (o as string);
            if(goodRepository.ConnectOrLoad(dataSource) && saleRepository.ConnectOrLoad(dataSource))
            {
                Goods = new ObservableCollection<Good>(goodRepository.GetAll());
                Sales = new ObservableCollection<Sale>(saleRepository.GetAll());
            }
        }

        void AddGood(object o)
        {
            var good = (o as Good);
            if (good.GoodCount > 0)
            {
                if (addingSalePositions.Select(s => s.Good.GoodId).Contains(good.GoodId))
                    addingSalePositions.Where(s => s.Good.GoodId == good.GoodId).SingleOrDefault().CountGood++;
                else
                    addingSalePositions.Add(new SalePos() { Good = good, GoodId = good.GoodId, CountGood = 1 });
                good.GoodCount--;
                Notify("CurrentSum");
            }
        }

        void RemoveGood(object o)
        {
            var salePos = o as SalePos;
            if (salePos.CountGood > 1)
                salePos.CountGood--;
            else
                AddingSalePositions.Remove(salePos);
            salePos.Good.GoodCount++;
            Notify("CurrentSum");
        }

        void Sell(object o)
        {
            Sale sale = new Sale()
            {
                SaleDate = DateTime.Now,
                Sum = CurrentSum,
                SalePositions = new List<SalePos>(AddingSalePositions)
            };
            saleRepository.Add(sale);
            Sales.Add(sale);
            saleRepository.DisconectOrSave();
            foreach (var salePos in addingSalePositions)
                goodRepository.Update(salePos.Good);
            addingSalePositions.Clear();
        }

        void PropertyChangedHandler(object o, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GoodSearchString")
                Goods = new ObservableCollection<Good>(goodRepository.GetAll().Where(g => g.GoodName.ToLower().Contains(goodSearchString.ToLower())));
            else if (e.PropertyName == "SaleSearchString")
                Sales = new ObservableCollection<Sale>(saleRepository.GetAll().Where(s => s.SaleDate.ToString().ToLower().Contains(saleSearchString.ToLower())));
            else if (e.PropertyName == "SelectedSale")
            {
                Notify("IsSaleSelected");
                if (prevSale != null && Sales.Contains(prevSale))
                {
                    prevSale.SalePositions = new List<SalePos>(UpdatingSalePositions);
                    saleRepository.Update(prevSale);
                }
                if (selectedSale != null)
                    UpdatingSalePositions = new ObservableCollection<SalePos>(SelectedSale.SalePositions);
                else
                    UpdatingSalePositions = new ObservableCollection<SalePos>();
            }
        }

        void RemoveSale(object o)
        {
            Sale sale = o as Sale;
            saleRepository.Delete(sale.SaleId);
            if (sale == selectedSale)
                UpdatingSalePositions.Clear();
            Sales.Remove(sale);
        }

        void Save(object o)
        {
            if (selectedSale != null)
            {
                SelectedSale.SalePositions = new List<SalePos>(UpdatingSalePositions);
                saleRepository.Update(SelectedSale);
            }
            saleRepository.DisconectOrSave();
        }

        void DownArrow(object o)
        {
            var salePos = o as SalePos;
            if (salePos.CountGood > 1)
                salePos.CountGood--;
            else
                UpdatingSalePositions.Remove(salePos);
            salePos.Good.GoodCount++;
            selectedSale.Sum -= salePos.Good.Price;
        }

        void UpArrow(object o)
        {
            var salePos = o as SalePos;
            if (salePos.Good.GoodCount > 0)
            {
                salePos.CountGood++;
                salePos.Good.GoodCount--;
                selectedSale.Sum += salePos.Good.Price;
            }
        }

        void GreenUpArrow(object o)
        {
            var good = o as Good;
            if (good.GoodCount > 0)
            {
                if (UpdatingSalePositions.Select(s => s.Good.GoodId).Contains(good.GoodId))
                    UpdatingSalePositions.Where(s => s.Good.GoodId == good.GoodId).SingleOrDefault().CountGood++;
                else
                    UpdatingSalePositions.Add(new SalePos() { Good = good, GoodId = good.GoodId, CountGood = 1 });
                good.GoodCount--;
            }
        }
    }
}
