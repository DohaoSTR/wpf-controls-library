using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ZdfFlatUI.MyControls.Primitives;

namespace ZdfFlatUI
{
    public class ZPasswordBox : IconTextBoxBase
    {
        private ToggleButton PART_SeePassword;

        private bool mIsHandledTextChanged = true;
        private StringBuilder mPasswordBuilder;

        [Bindable(true)]
        public bool IsCanSeePassword
        {
            get => (bool)GetValue(IsCanSeePasswordProperty);
            set => SetValue(IsCanSeePasswordProperty, value);
        }

        public static readonly DependencyProperty IsCanSeePasswordProperty =
            DependencyProperty.Register("IsCanSeePassword", typeof(bool), typeof(ZPasswordBox), new PropertyMetadata(true, IsCanSeePasswordChangedCallback));

        private static void IsCanSeePasswordChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZPasswordBox passowrdBox && passowrdBox.PART_SeePassword != null)
            {
                passowrdBox.PART_SeePassword.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [Bindable(true)]
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(ZPasswordBox), new PropertyMetadata(string.Empty));

        [Bindable(true)]
        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(ZPasswordBox), new PropertyMetadata('●'));

        public bool ShowPassword
        {
            get => (bool)GetValue(ShowPasswordProperty);
            private set => SetValue(ShowPasswordProperty, value);
        }

        public static readonly DependencyProperty ShowPasswordProperty =
            DependencyProperty.Register("ShowPassword", typeof(bool), typeof(ZPasswordBox), new PropertyMetadata(false, ShowPasswordChanged));

        private static void ShowPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZPasswordBox passwordBox)
            {
                passwordBox.SelectionStart = passwordBox.Text.Length + 1;
            }
        }

        static ZPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZPasswordBox), new FrameworkPropertyMetadata(typeof(ZPasswordBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_SeePassword = GetTemplateChild("PART_SeePassword") as ToggleButton;
            if (PART_SeePassword != null)
            {
                PART_SeePassword.Visibility = IsCanSeePassword ? Visibility.Visible : Visibility.Collapsed;
            }
            SetEvent();

            SetText(ConvertToPasswordChar(Password.Length));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CommandBinding_Executed, CommandBinding_CanExecute));
        }

        public override void OnCornerRadiusChanged(CornerRadius newValue)
        {
            IconCornerRadius = new CornerRadius(newValue.TopLeft, 0, 0, newValue.BottomLeft);
        }

        private void SetEvent()
        {
            TextChanged += ZPasswordBox_TextChanged;
            if (PART_SeePassword != null)
            {
                PART_SeePassword.Checked += (o, e) =>
                {
                    SetText(Password);
                    ShowPassword = true;
                };
                PART_SeePassword.Unchecked += (o, e) =>
                {
                    SetText(ConvertToPasswordChar(Password.Length));
                    ShowPassword = false;
                };
            }
        }

        private void SetText(string str)
        {
            mIsHandledTextChanged = false;
            Text = str;
            mIsHandledTextChanged = true;
        }

        private string ConvertToPasswordChar(int length)
        {
            if (mPasswordBuilder != null)
            {
                mPasswordBuilder.Clear();
            }
            else
            {
                mPasswordBuilder = new StringBuilder();
            }

            for (int i = 0; i < length; i++)
            {
                mPasswordBuilder.Append(PasswordChar);
            }

            return mPasswordBuilder.ToString();
        }

        private void ZPasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!mIsHandledTextChanged)
            {
                return;
            }

            foreach (TextChange c in e.Changes)
            {
                Password = Password.Remove(c.Offset, c.RemovedLength);
                Password = Password.Insert(c.Offset, Text.Substring(c.Offset, c.AddedLength));
            }

            if (!ShowPassword)
            {
                SetText(ConvertToPasswordChar(Text.Length));
            }
            SelectionStart = Text.Length + 1;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
