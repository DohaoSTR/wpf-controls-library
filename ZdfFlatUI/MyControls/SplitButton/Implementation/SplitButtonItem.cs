using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class SplitButtonItem : ContentControl
    {
        #region Private属性

        #endregion

        private SplitButton ParentListBox
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as SplitButton;
            }
        }

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #endregion

        #region Constructors
        static SplitButtonItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButtonItem), new FrameworkPropertyMetadata(typeof(SplitButtonItem)));
        }

        public SplitButtonItem()
        {
            MouseEnter += DropdownButtonItem_MouseEnter;
            MouseLeave += DropdownButtonItem_MouseLeave;
            MouseLeftButtonUp += DropdownButtonItem_MouseLeftButtonUp;
        }

        private void DropdownButtonItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SplitButtonItem item = sender as SplitButtonItem;
            ParentListBox.OnItemClick(item.Content, item.Content);
            ParentListBox.IsDropDownOpen = false;
            e.Handled = true;
        }

        private void DropdownButtonItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void DropdownButtonItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", false);
        }
        #endregion

        #region Override方法

        #endregion

        #region Private方法

        #endregion
    }
}
