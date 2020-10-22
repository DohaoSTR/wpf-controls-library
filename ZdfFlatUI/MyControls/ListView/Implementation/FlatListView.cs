using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class FlatListView : ListView
    {
        #region Constructors

        static FlatListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatListView), new FrameworkPropertyMetadata(typeof(FlatListView)));
        }

        #endregion
    }
}
