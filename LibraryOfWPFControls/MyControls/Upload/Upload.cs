using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    public class Upload : ButtonBase
    {
        public static readonly DependencyProperty MultiSelectProperty;
        public static readonly DependencyProperty FilterProperty;

        public bool MultiSelect
        {
            get => (bool)GetValue(MultiSelectProperty);
            set => SetValue(MultiSelectProperty, value);
        }

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly RoutedEvent UploadEvent;

        public event RoutedPropertyChangedEventHandler<object> FileUpload
        {
            add
            {
                AddHandler(UploadEvent, value);
            }
            remove
            {
                RemoveHandler(UploadEvent, value);
            }
        }

        protected virtual void OnFileUpload(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, UploadEvent);
            RaiseEvent(arg);
        }

        static Upload()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Upload), new FrameworkPropertyMetadata(typeof(Upload)));

            MultiSelectProperty = DependencyProperty.Register("MultiSelect", typeof(bool), typeof(Upload));
            FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(Upload));

            UploadEvent = EventManager.RegisterRoutedEvent("FileUpload"
                , RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>)
                , typeof(Upload));
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Array files = (Array)e.Data.GetData(DataFormats.FileDrop);
                OnFileUpload(null, files);
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
        }

        protected override void OnClick()
        {
            base.OnClick();

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Multiselect = MultiSelect,
                Filter = Filter
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                OnFileUpload(null, files);
            }
        }
    }
}
