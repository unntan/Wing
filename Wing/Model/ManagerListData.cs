using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.Model
{
    public class ManagerListData : ViewModelBase
    {
        // 担当者ID
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

        // 担当者名
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

        // 会社ID
        private int _KaisyaId = 0;
        public int KaisyaId
        {
            get
            {
                return _KaisyaId;
            }
            set
            {
                _KaisyaId = value;
                OnPropertyChanged();
            }
        }

        // 会社名
        private string _KaisyaName = "";
        public string KaisyaName
        {
            get
            {
                return _KaisyaName;
            }
            set
            {
                _KaisyaName = value;
                OnPropertyChanged();
            }
        }

        public ManagerListData(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
