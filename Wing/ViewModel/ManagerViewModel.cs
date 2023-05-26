using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.ViewModel
{
    class ManagerViewModel:ViewModelBase
    {
        public ManagerViewModel()
        {

        }

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
                OnPropertyChanged();
            }
        }
    }
}
