using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        private string _firstRow;
        public string firstRow
        {
            get
            {
                return _firstRow;
            }
            set
            {
                _firstRow = value;
                OnPropertyChanged();
            }
        }

        private string _secondRow;
        public string secondRow
        {
            get
            {
                return _secondRow;
            }
            set
            {
                _secondRow = value;
                OnPropertyChanged();
            }
        }

        private string _thirdRow;
        public string thirdRow
        {
            get
            {
                return _thirdRow;
            }
            set
            {
                _thirdRow = value;
                OnPropertyChanged();
            }
        }

        private string _fourthRow;
        public string fourthRow
        {
            get
            {
                return _fourthRow;
            }
            set
            {
                _fourthRow = value;
                OnPropertyChanged();
            }
        }

    }
}
