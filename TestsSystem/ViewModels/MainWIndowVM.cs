using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestsSystem.Classes;

namespace TestsSystem.ViewModels
{
    public class MainWIndowVM: INotifyPropertyChanged
    {
        private bool _canGoBack;
        public bool canGoBack {
            get {
                return _canGoBack;
            }
            set {
                _canGoBack = value;
                OnPropertyChanged("canGoBack");
            }
        }

        private RelayCommand _backButClick;
        public RelayCommand backButClick
        {
            get
            {
                return _backButClick ?? (_backButClick = new RelayCommand(obj =>
                {
                    
                },
                (obj) => canGoBack));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
