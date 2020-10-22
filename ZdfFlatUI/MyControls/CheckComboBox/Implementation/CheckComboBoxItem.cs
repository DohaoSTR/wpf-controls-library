using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZdfFlatUI
{
    public class CheckComboBoxItem : System.Windows.Controls.ListBoxItem
    {
        #region private fields
        private CheckComboBox ParentCheckComboBox
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as CheckComboBox;
            }
        }
        #endregion

        #region DependencyProperty

        #endregion

        #region Constructors

        static CheckComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckComboBoxItem), new FrameworkPropertyMetadata(typeof(CheckComboBoxItem)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (ParentCheckComboBox != null)
            {
                ParentCheckComboBox.NotifyCheckComboBoxItemClicked(this);
            }

            base.OnMouseLeftButtonDown(e);
        }
        #endregion

        #region private function

        #endregion

        #region Event Implement Function

        #endregion
    }
}
