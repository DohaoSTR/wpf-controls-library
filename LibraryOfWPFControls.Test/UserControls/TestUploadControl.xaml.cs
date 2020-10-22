﻿using GalaSoft.MvvmLight;
using LibraryOfWPFControls.Test.Utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LibraryOfWPFControls.Test.UserControls
{
    /// <summary>
    /// Interaction logic for TestUploadControl.xaml
    /// </summary>
    public partial class TestUploadControl : UserControl
    {
        public TestUploadControl()
        {
            InitializeComponent();

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                var vm = new Lazy<HomeViewModel>(() => HomeViewModel.Instance);
                DataContext = vm.Value;
            }
        }

        private void Upload_FileUpload(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            HomeViewModel vm = DataContext as HomeViewModel;
            vm.FileUploadCommand.Execute(e.NewValue);
        }
    }
}
