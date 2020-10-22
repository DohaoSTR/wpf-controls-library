using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace ZdfFlatUI
{
    /// <summary>
    /// 多选下拉框
    /// </summary>
    public class CheckComboBox : Selector
    {
        #region private fields
        private ContentPresenter PART_ContentSite;
        private TextBox PART_FilterTextBox;
        private ICollectionView view;
        private Popup PART_Popup;

        private bool mPopupIsFirstOpen;
        #endregion

        #region DependencyProperty

        #region Content

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(CheckComboBox), new PropertyMetadata(string.Empty));

        #endregion

        #region Value

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(CheckComboBox), new PropertyMetadata(string.Empty));

        #endregion

        #region SelectedObjList

        public ObservableCollection<object> SelectedObjList
        {
            get { return (ObservableCollection<object>)GetValue(SelectedObjListProperty); }
            private set { SetValue(SelectedObjListProperty, value); }
        }

        public static readonly DependencyProperty SelectedObjListProperty =
            DependencyProperty.Register("SelectedObjList", typeof(ObservableCollection<object>), typeof(CheckComboBox), new PropertyMetadata(null));

        #endregion

        #region SelectedStrList

        public ObservableCollection<string> SelectedStrList
        {
            get { return (ObservableCollection<string>)GetValue(SelectedStrListProperty); }
            private set { SetValue(SelectedStrListProperty, value); }
        }

        public static readonly DependencyProperty SelectedStrListProperty =
            DependencyProperty.Register("SelectedStrList", typeof(ObservableCollection<string>), typeof(CheckComboBox));

        #endregion

        #region IsDropDownOpen

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(CheckComboBox), new PropertyMetadata(false, OnIsDropDownOpenChanged));

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckComboBox checkComboBox = d as CheckComboBox;

        }

        #endregion

        #region IsShowFilterBox
        /// <summary>
        /// 获取或者设置下拉列表过滤文本框的显示与隐藏
        /// </summary>
        public bool IsShowFilterBox
        {
            get { return (bool)GetValue(IsShowFilterBoxProperty); }
            set { SetValue(IsShowFilterBoxProperty, value); }
        }

        public static readonly DependencyProperty IsShowFilterBoxProperty =
            DependencyProperty.Register("IsShowFilterBox", typeof(bool), typeof(CheckComboBox), new PropertyMetadata(false));

        #endregion

        #region MaxShowNumber
        /// <summary>
        /// 获取或者设置最多显示的选中个数
        /// </summary>
        public int MaxShowNumber
        {
            get { return (int)GetValue(MaxShowNumberProperty); }
            set { SetValue(MaxShowNumberProperty, value); }
        }

        public static readonly DependencyProperty MaxShowNumberProperty =
            DependencyProperty.Register("MaxShowNumber", typeof(int), typeof(CheckComboBox), new PropertyMetadata(4));

        #endregion

        #region MaxDropDownHeight

        public double MaxDropDownHeight
        {
            get { return (double)GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(CheckComboBox), new PropertyMetadata(200d));

        #endregion

        #region FilterBoxWatermark

        public string FilterBoxWatermark
        {
            get { return (string)GetValue(FilterBoxWatermarkProperty); }
            set { SetValue(FilterBoxWatermarkProperty, value); }
        }

        public static readonly DependencyProperty FilterBoxWatermarkProperty =
            DependencyProperty.Register("FilterBoxWatermark", typeof(string), typeof(CheckComboBox), new PropertyMetadata("Enter keyword filtering"));

        #endregion

        #endregion

        private bool HasCapture
        {
            get
            {
                return Mouse.Captured == this;
            }
        }

        #region Constructors

        static CheckComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckComboBox), new FrameworkPropertyMetadata(typeof(CheckComboBox)));
        }

        public CheckComboBox()
        {
            SelectedObjList = new ObservableCollection<object>();
            SelectedStrList = new ObservableCollection<string>();
        }
        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (PART_FilterTextBox != null)
            {
                PART_FilterTextBox.TextChanged -= PART_FilterTextBox_TextChanged;
            }
            if (PART_Popup != null)
            {
                PART_Popup.Opened -= PART_Popup_Opened;
            }

            PART_ContentSite = GetTemplateChild("PART_ContentSite") as ContentPresenter;
            PART_FilterTextBox = GetTemplateChild("PART_FilterTextBox") as TextBox;
            PART_Popup = GetTemplateChild("PART_Popup") as Popup;
            if (PART_FilterTextBox != null)
            {
                PART_FilterTextBox.TextChanged += PART_FilterTextBox_TextChanged;
            }

            view = CollectionViewSource.GetDefaultView(ItemsSource);

            if (PART_Popup != null)
            {
                PART_Popup.Opened += PART_Popup_Opened;
            }

            Init();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (!(item is CheckComboBoxItem))
            {
                CheckComboBoxItem checkComboBoxItem = element as CheckComboBoxItem;
                if (checkComboBoxItem != null && !string.IsNullOrEmpty(DisplayMemberPath))
                {
                    Binding binding = new Binding(DisplayMemberPath);
                    checkComboBoxItem.SetBinding(CheckComboBoxItem.ContentProperty, binding);
                }
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CheckComboBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CheckComboBoxItem);
        }

        #endregion

        #region private function
        private void Init()
        {
            mPopupIsFirstOpen = true;

            if (SelectedObjList != null)
            {
                foreach (var obj in SelectedObjList)
                {
                    if (string.IsNullOrWhiteSpace(DisplayMemberPath))
                    {
                        SelectedStrList.Add(obj.ToString());
                    }
                    else
                    {
                        SelectedStrList.Add(Utils.CommonUtil.GetPropertyValue(obj, DisplayMemberPath).ToString());
                    }
                }
            }
            SetCheckComboBoxValueAndContent();
        }

        private void SetCheckComboBoxValueAndContent()
        {
            if (SelectedStrList == null) return;

            if (SelectedStrList.Count > MaxShowNumber)
            {
                Content = SelectedStrList.Count + " Selected";
            }
            else
            {
                Content = SelectedStrList.Aggregate("", (current, p) => current + (p + ", ")).TrimEnd(new char[] { ' ' }).TrimEnd(new char[] { ',' });
            }

            Value = SelectedStrList.Aggregate("", (current, p) => current + (p + ",")).TrimEnd(new char[] { ',' });
        }
        #endregion

        #region internal
        /// <summary>
        /// 行选中
        /// </summary>
        /// <param name="item"></param>
        internal void NotifyCheckComboBoxItemClicked(CheckComboBoxItem item)
        {
            item.SetValue(CheckComboBoxItem.IsSelectedProperty, !item.IsSelected);
            string itemContent = Convert.ToString(item.Content);
            if (item.IsSelected)
            {
                if (!SelectedStrList.Contains(item.Content))
                {
                    SelectedStrList.Add(itemContent);
                }
                if (!SelectedObjList.Contains(item.DataContext))
                {
                    SelectedObjList.Add(item.DataContext);
                }
            }
            else
            {
                if (SelectedStrList.Contains(itemContent))
                {
                    SelectedStrList.Remove(itemContent);
                }
                if (SelectedObjList.Contains(item.DataContext))
                {
                    SelectedObjList.Remove(item.DataContext);
                }
            }

            SetCheckComboBoxValueAndContent();
        }

        #endregion

        #region Event Implement Function
        /// <summary>
        /// 每次Open回显数据不太好，先这么处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            if (!mPopupIsFirstOpen) return;

            mPopupIsFirstOpen = false;

            if (ItemsSource == null || SelectedObjList == null) return;

            foreach (var obj in SelectedObjList)
            {
                foreach (var item in ItemsSource)
                {
                    if (item == obj)
                    {
                        CheckComboBoxItem checkComboBoxItem = ItemContainerGenerator.ContainerFromItem(item) as CheckComboBoxItem;
                        if (checkComboBoxItem != null)
                        {
                            checkComboBoxItem.IsSelected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void PART_FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PART_FilterTextBox == null || view == null) return;

            view.Filter += (o) =>
            {
                string value = Convert.ToString(Utils.CommonUtil.GetPropertyValue(o, DisplayMemberPath)).ToLower();
                return value.IndexOf(PART_FilterTextBox.Text.ToLower()) != -1;
            };
        }
        #endregion
    }
}
