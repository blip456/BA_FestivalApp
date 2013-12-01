using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ypsilon2.viewmodel
{
    class FestivalVM : ObservableObject, IPage
    {
        #region fields en props
        public string Name
        {
            get { return "Festivalgegevens"; }
        }

        public FestivalVM()
        {

        } 
        #endregion
    }
}
