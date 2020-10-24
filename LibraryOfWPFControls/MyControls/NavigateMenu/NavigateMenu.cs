using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZdfFlatUI
{
    public class NavigateMenu : ListBox
    {
        private readonly CollectionViewSource viewSource = new CollectionViewSource();

        public static readonly DependencyProperty GroupDescriptionsProperty;
        public static readonly DependencyProperty GroupItemsSourceProperty;
        public static readonly DependencyProperty MyGroupStyleProperty;
        public static readonly DependencyProperty ShowGroupProperty;

        public string GroupDescriptions
        {
            get => (string)GetValue(GroupDescriptionsProperty);
            set => SetValue(GroupDescriptionsProperty, value);
        }

        public IEnumerable GroupItemsSource
        {
            get => (IEnumerable)GetValue(GroupItemsSourceProperty);
            set => SetValue(GroupItemsSourceProperty, value);
        }

        public GroupStyle MyGroupStyle
        {
            get => (GroupStyle)GetValue(MyGroupStyleProperty);
            set => SetValue(MyGroupStyleProperty, value);
        }

        public bool ShowGroup
        {
            get => (bool)GetValue(ShowGroupProperty);
            set => SetValue(ShowGroupProperty, value);
        }

        static NavigateMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigateMenu), new FrameworkPropertyMetadata(typeof(NavigateMenu)));

            GroupDescriptionsProperty = DependencyProperty.Register("GroupDescriptions", typeof(string), typeof(NavigateMenu));
            GroupItemsSourceProperty = DependencyProperty.Register("GroupItemsSource", typeof(IEnumerable), typeof(NavigateMenu));
            MyGroupStyleProperty = DependencyProperty.Register("MyGroupStyle", typeof(GroupStyle), typeof(NavigateMenu));
            ShowGroupProperty = DependencyProperty.Register("ShowGroup", typeof(bool), typeof(NavigateMenu));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NavigateMenuItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (!string.IsNullOrEmpty(GroupDescriptions))
            {
                string[] list = GroupDescriptions.Split(',');
                foreach (string desc in list)
                {
                    viewSource.GroupDescriptions.Add(new PropertyGroupDescription(desc));
                }
            }
            viewSource.Source = GroupItemsSource;

            Binding binding = new Binding
            {
                Source = viewSource
            };

            BindingOperations.SetBinding(this, ItemsSourceProperty, binding);
        }
    }
}