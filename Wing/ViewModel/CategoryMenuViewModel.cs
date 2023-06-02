using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.ViewModel
{
    public class CategoryMenuViewModel : ViewModelBase
    {
        private string _LabelUserName;
        public string LabelUserName
        {
            get
            {
                return _LabelUserName;
            }
            set
            {
                _LabelUserName = "【ログインユーザ：" + value + "】";
                OnPropertyChanged();
            }
        }

        private string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
    }
}
