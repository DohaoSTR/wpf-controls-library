using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZdfFlatUI
{
    [TemplatePart(Name = "PART_TagListBox", Type = typeof(ListBox))]
    public class TagTextBox : TextBox
    {
        private const string DefaultInputLanguage = "en";
        private ListBox PART_TagListBox;

        static TagTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TagTextBox), new FrameworkPropertyMetadata(typeof(TagTextBox)));
        }

        public TagTextBox()
        {
            PreviewKeyDown += TagTextBox_PreviewKeyDown;
            KeyUp += TagTextBox_KeyUp;
            GotFocus += TagTextBox_GotFocus;
        }

        private void TagTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Oem1)
            {
                OnAddItem(null, Text.Remove(Text.Length - 1, 1));
                Text = string.Empty;
            }
        }

        private void TagTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeInputLanguage();
        }

        #region 依赖属性

        #region ItemsSource数据源
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource"
            , typeof(System.Collections.IEnumerable), typeof(TagTextBox)
            , new FrameworkPropertyMetadata(new PropertyChangedCallback(OnItemsSourceChanged)));

        /// <summary>
        /// 数据源
        /// </summary>
        public System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region DisplayMemberPath
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath"
            , typeof(string), typeof(TagTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDisplayMemberPathChanged)));

        /// <summary>
        /// 显示的
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion

        #region RemoveIndex
        public static readonly DependencyProperty RemoveIndexProperty = DependencyProperty.Register("RemoveIndex"
            , typeof(int), typeof(TagTextBox));
        /// <summary>
        /// 需要删除项的索引
        /// </summary>
        public int RemoveIndex
        {
            get { return (int)GetValue(RemoveIndexProperty); }
            set { SetValue(RemoveIndexProperty, value); }
        }
        #endregion

        #endregion

        #region 依赖属性数值改变回调
        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TagTextBox tagTextBox = (TagTextBox)sender;
            if (e.Property == ItemsSourceProperty)
            {
                tagTextBox.ItemsSource = (System.Collections.IEnumerable)e.NewValue;
            }
        }

        private static void OnDisplayMemberPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TagTextBox tagTextBox = (TagTextBox)sender;
            if (e.Property == ItemsSourceProperty)
            {
                tagTextBox.DisplayMemberPath = (string)e.NewValue;
            }
        }
        #endregion

        #region 自定义命令

        #region 增加一项
        public static readonly RoutedEvent AddItemEvent = EventManager.RegisterRoutedEvent("AddItem",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(TagTextBox));

        public event RoutedPropertyChangedEventHandler<object> AddItem
        {
            add
            {
                AddHandler(AddItemEvent, value);
            }
            remove
            {
                RemoveHandler(AddItemEvent, value);
            }
        }

        protected virtual void OnAddItem(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, AddItemEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region 移除一项
        public static readonly RoutedEvent RemoveItemEvent = EventManager.RegisterRoutedEvent("RemoveItem",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(TagTextBox));

        public event RoutedPropertyChangedEventHandler<object> RemoveItem
        {
            add
            {
                AddHandler(RemoveItemEvent, value);
            }
            remove
            {
                RemoveHandler(RemoveItemEvent, value);
            }
        }

        protected virtual void OnRemoveItem(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, RemoveItemEvent);
            RaiseEvent(arg);
        }
        #endregion

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_TagListBox = GetTemplateChild("PART_TagListBox") as ListBox;

            //ListBox失去焦点时，重新设置SelectedIndex为-1即未选中项
            PART_TagListBox.LostFocus += PART_TagListBox_LostFocus;
        }

        #region 事件执行
        private void TagTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnAddItem(null, null);
                Text = string.Empty;
            }
            else if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                //当选中了ListBox中的一项，那么点击删除按键则删除选中的那一项
                //如果没有选中ListBox中的任何一项，并且TextBox中也没有输入文字，则删除最后一项
                //否则则执行正常的Delete操作，即删除TextBox中的一个字符
                if (PART_TagListBox.SelectedIndex != -1)
                {
                    //给RemoveIndex赋值是为了在MVVM模式下，给ViewModel传递当前选中的Item的索引，用于删除使用
                    RemoveIndex = PART_TagListBox.SelectedIndex;
                    OnRemoveItem(null, RemoveIndex);
                }
                else
                {
                    if (string.IsNullOrEmpty(Text))
                    {
                        RemoveIndex = PART_TagListBox.Items.Count - 1;
                        OnRemoveItem(null, RemoveIndex);
                    }
                }
            }
        }

        private void PART_TagListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PART_TagListBox.SelectedIndex = -1;
        }
        #endregion

        void ChangeInputLanguage()
        {
            //改变当前输入法为英文的。
            if (InputLanguageManager.Current.CurrentInputLanguage.Name.StartsWith(DefaultInputLanguage))
            {
                return;
            }
            foreach (var lang in InputLanguageManager.Current.AvailableInputLanguages)
            {
                var langCultureInfo = lang as CultureInfo;
                if (langCultureInfo.Name.StartsWith(DefaultInputLanguage))
                {
                    InputLanguageManager.Current.CurrentInputLanguage = langCultureInfo;
                    break;
                }
            }
        }
    }
}
