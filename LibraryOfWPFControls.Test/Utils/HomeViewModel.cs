using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace LibraryOfWPFControls.Test.Utils
{
    public class HomeViewModel : ViewModelBase
    {
        private static HomeViewModel instance;

        public static HomeViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HomeViewModel();
                }
                return instance;
            }
        }

        private ObservableCollection<AttachmentDTO> _UploadFileList;

        public ObservableCollection<AttachmentDTO> UploadFileList
        {
            get { return _UploadFileList; }
            set { _UploadFileList = value; RaisePropertyChanged("UploadFileList"); }
        }

        public HomeViewModel()
        {
            UploadFileList = new ObservableCollection<AttachmentDTO>();
        }

        private RelayCommand<object> _FileUploadCommand;

        public RelayCommand<object> FileUploadCommand
        {
            get
            {
                return _FileUploadCommand ?? (new RelayCommand<object>(HandleFileUpload));
            }

            set
            {
                _FileUploadCommand = value;
            }
        }

        private void HandleFileUpload(object param)
        {
            Array files = param as Array;

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files.GetValue(i).ToString();
                FileInfo fileInfo = new FileInfo(filePath);

                UploadFileList.Add(new AttachmentDTO()
                {
                    ID = "1",
                    FJMC = Path.GetFileName(filePath),
                    FJLX = Path.GetExtension(filePath),
                });
            }
        }
    }
}
