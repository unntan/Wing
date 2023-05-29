using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.ViewModel
{
    public class TaxViewModel : ViewModelBase
    {
        private int _BeforeTax;
        public int BeforeTax
        {
            get
            {
                return _BeforeTax;
            }
            set
            {
                _BeforeTax = value;
                OnPropertyChanged();
            }
        }

        private int _AfterTax;
        public int AfterTax
        {
            get
            {
                return _AfterTax;
            }
            set
            {
                _AfterTax = value;
                OnPropertyChanged();
            }
        }
    }
}
