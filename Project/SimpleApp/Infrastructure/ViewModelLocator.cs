using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleApp.ViewModels;
using SimpleApp.Infrastructure.Modules;

namespace SimpleApp.Infrastructure
{
    class ViewModelLocator
    {
        static StandardKernel kernel;
        static GoodTabViewModel goodTabViewModel;
        static MainViewModel mainViewModel;
        static ManufacturerTabViewModel manufacturerTabViewModel;
        static CategoryTabViewModel categoryTabViewModel;
        static SaleViewModel saleViewModel;

        public GoodTabViewModel GoodTabViewModel { get => goodTabViewModel != null ? goodTabViewModel : goodTabViewModel = kernel.Get<GoodTabViewModel>(); }
        public MainViewModel MainViewModel { get => mainViewModel != null ? mainViewModel : mainViewModel = kernel.Get<MainViewModel>(); }
        public ManufacturerTabViewModel ManufacturerTabViewModel { get => manufacturerTabViewModel != null ? manufacturerTabViewModel : manufacturerTabViewModel = kernel.Get<ManufacturerTabViewModel>(); }
        public CategoryTabViewModel CategoryTabViewModel { get => categoryTabViewModel != null ? categoryTabViewModel : categoryTabViewModel = kernel.Get<CategoryTabViewModel>(); }
        public SaleViewModel SaleViewModel { get => saleViewModel != null ? saleViewModel : saleViewModel = kernel.Get<SaleViewModel>(); }

        public ViewModelLocator()
        {
            if(kernel == null)
                kernel = new StandardKernel(new MainViewModelModule(), new ManufacturerTabViewModelModule(), new GoodTabViewModelModule(), new CategoryTabViewModelModule(), new SaleViewModelModule());
        }

        
    }
}
