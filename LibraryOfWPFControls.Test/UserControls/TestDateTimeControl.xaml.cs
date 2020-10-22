using System;
using System.Windows;
using System.Windows.Controls;

namespace LibraryOfWPFControls.Test.UserControls
{
    public partial class TestDateTimeControl : UserControl
    {
        public TestDateTimeControl()
        {
            InitializeComponent(); dateControl1.SelectedDate = DateTime.Now;
            dateControl5.SelectedDateStart = DateTime.Today.AddDays(-10);
            dateControl5.SelectedDateEnd = DateTime.Today;
            dateControl7.SelectedDate = DateTime.Now;
        }

        private void btnSetDate_Click(object sender, RoutedEventArgs e)
        {
            timePicker.Value = DateTime.Now;
        }

        private void btnGetDate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(timePicker.Value.Value.ToString());
            MessageBox.Show(DateTime.DaysInMonth(2017, 4).ToString());
            MessageBox.Show(DateTime.Now.DayOfWeek.ToString());
        }

        private void btnGetDateTimePicker_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(dateTimePicker.Value.ToString());
        }

        private void btnSetDateTimePicker_Click(object sender, RoutedEventArgs e)
        {
            dateTimePicker.Value = DateTime.Now.AddDays(5);
        }
    }
}
