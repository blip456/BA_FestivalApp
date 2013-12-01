using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ypsilon2.viewmodel
{
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Pages.Add(new ContactpersonVM());
            Pages.Add(new BandsVM());
            Pages.Add(new LineUpVM());
            Pages.Add(new TicketingVM());
            Pages.Add(new FestivalVM());

            CurrentPage = Pages[0];

        }

        private IPage _currentpage;
        public IPage CurrentPage
        {
            get { return _currentpage; }
            set { _currentpage = value; OnPropertyChanged("CurrentPage"); }
        }

        private List<IPage> _pages;
        public List<IPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<IPage>();
                return _pages;
            }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        private void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        public ICommand ChangePageByIdCommand 
        {
            get { return new RelayCommand<int>(ChangePageById); }
        }

        private void ChangePageById(int i)
        {
            Console.WriteLine("klikerdeklik");
            CurrentPage = Pages[i];
        }        
        
        
    }
}
