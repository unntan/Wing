using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.Model
{
    public class CompanyListData : ViewModelBase
    {
        // 企業ID
        private int _Id = 0;
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

        // 企業名
        public string _Name = "";
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

        public CompanyListData(int id,string name)
        {
            Id = id;
            Name = name;
        }
    }
}
