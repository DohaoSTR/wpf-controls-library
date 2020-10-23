using GalaSoft.MvvmLight;

namespace LibraryOfWPFControls.Test.Utils
{
    public class AttachmentDTO : ViewModelBase
    {
        private string _ID;

        public string ID
        {
            get => _ID;
            set { _ID = value; RaisePropertyChanged("ID"); }
        }

        private string _ZDDWID;

        public string ZDDWID
        {
            get => _ZDDWID;
            set { _ZDDWID = value; RaisePropertyChanged("ZDDWID"); }
        }

        private string _ZDDWMC;

        public string ZDDWMC
        {
            get => _ZDDWMC;
            set { _ZDDWMC = value; RaisePropertyChanged("ZDDWMC"); }
        }

        private string _FJMC;

        public string FJMC
        {
            get => _FJMC;
            set { _FJMC = value; RaisePropertyChanged("FJMC"); }
        }

        private string _FJLX;

        public string FJLX
        {
            get => _FJLX;
            set { _FJLX = value; RaisePropertyChanged("FJLX"); }
        }
    }
}
