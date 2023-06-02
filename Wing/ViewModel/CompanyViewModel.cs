using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Other;

namespace Wing.ViewModel
{
    class CompanyViewModel:ViewModelBase
    {
        private int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _Address;
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                OnPropertyChanged();
            }
        }

        private string _Tell;
        public string Tell
        {
            get
            {
                return _Tell;
            }
            set
            {
                _Tell = value;
                OnPropertyChanged();
            }
        }
    }
}
