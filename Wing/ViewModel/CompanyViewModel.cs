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
                if (_Id == value) return;
                _Id = value;
                //OnPropertyChanged("Id");
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
                if (_Name == value) return;
                _Name = value;
                // OnPropertyChanged("Name");
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
                if (_Address == value) return;
                _Address = value;
                //OnPropertyChanged("Address");
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
                if (_Tell == value) return;
                _Tell = value;
                //OnPropertyChanged("Tell");
                OnPropertyChanged();
            }
        }
    }
}
