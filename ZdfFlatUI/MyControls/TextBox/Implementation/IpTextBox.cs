using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZdfFlatUI
{
    public enum EnumIpBoxType
    {
        /// <summary>
        /// IP地址或者网关
        /// </summary>
        IpAddress,
        /// <summary>
        /// 子网掩码
        /// </summary>
        SubnetMask,
    }

    public class IpTextBox : Control
    {
        /// <summary>
        /// IP正则
        /// </summary>
        private const string ipRegex = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
        private const string ZeroTo255Tip = "{0} 不是有效项。请指定一个介于 0 和 255 间的值";

        #region private fields
        private TextBox PART_BOX1;
        private TextBox PART_BOX2;
        private TextBox PART_BOX3;
        private TextBox PART_BOX4;
        #endregion

        #region DependencyProperty

        #region Type

        /// <summary>
        /// 获取或者设置文本框类型
        /// </summary>
        public EnumIpBoxType Type
        {
            get { return (EnumIpBoxType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumIpBoxType), typeof(IpTextBox), new PropertyMetadata(EnumIpBoxType.IpAddress));

        #endregion

        #region IsHasError

        /// <summary>
        /// 获取或者设置是否输入了不合法的数字
        /// </summary>
        public bool IsHasError
        {
            get { return (bool)GetValue(IsHasErrorProperty); }
            private set { SetValue(IsHasErrorProperty, value); }
        }

        public static readonly DependencyProperty IsHasErrorProperty =
            DependencyProperty.Register("IsHasError", typeof(bool), typeof(IpTextBox), new PropertyMetadata(false));

        #endregion

        #region ErrorContent

        /// <summary>
        /// 获取或者设置非法输入时的提示内容
        /// </summary>
        public string ErrorContent
        {
            get { return (string)GetValue(ErrorContentProperty); }
            private set { SetValue(ErrorContentProperty, value); }
        }

        public static readonly DependencyProperty ErrorContentProperty =
            DependencyProperty.Register("ErrorContent", typeof(string), typeof(IpTextBox), new PropertyMetadata(string.Empty));

        #endregion

        #region IsKeyboardFocused

        public new bool IsKeyboardFocused
        {
            get { return (bool)GetValue(IsKeyboardFocusedProperty); }
            private set { SetValue(IsKeyboardFocusedProperty, value); }
        }

        public new static readonly DependencyProperty IsKeyboardFocusedProperty =
            DependencyProperty.Register("IsKeyboardFocused", typeof(bool), typeof(IpTextBox), new PropertyMetadata(false));

        #endregion

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IpTextBox), new PropertyMetadata(string.Empty));

        #endregion

        #endregion

        #region Constructors

        static IpTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IpTextBox), new FrameworkPropertyMetadata(typeof(IpTextBox)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_BOX1 = GetTemplateChild("PART_BOX1") as TextBox;
            PART_BOX2 = GetTemplateChild("PART_BOX2") as TextBox;
            PART_BOX3 = GetTemplateChild("PART_BOX3") as TextBox;
            PART_BOX4 = GetTemplateChild("PART_BOX4") as TextBox;

            if (PART_BOX1 != null)
            {
                PART_BOX1.PreviewTextInput += PART_BOX1_PreviewTextInput;
                PART_BOX1.TextChanged += PART_BOX1_TextChanged;
                PART_BOX1.GotFocus += (o, e) => { IsKeyboardFocused = true; };
                PART_BOX1.LostFocus += (o, e) => { IsKeyboardFocused = false; };
                PART_BOX1.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            if (PART_BOX2 != null)
            {
                PART_BOX2.PreviewTextInput += PART_BOX2_PreviewTextInput;
                PART_BOX2.TextChanged += PART_BOX2_TextChanged;
                PART_BOX2.GotFocus += (o, e) => { IsKeyboardFocused = true; };
                PART_BOX2.LostFocus += (o, e) => { IsKeyboardFocused = false; };
                PART_BOX2.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            if (PART_BOX3 != null)
            {
                PART_BOX3.PreviewTextInput += PART_BOX3_PreviewTextInput;
                PART_BOX3.TextChanged += PART_BOX3_TextChanged;
                PART_BOX3.GotFocus += (o, e) => { IsKeyboardFocused = true; };
                PART_BOX3.LostFocus += (o, e) => { IsKeyboardFocused = false; };
                PART_BOX3.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            if (PART_BOX4 != null)
            {
                PART_BOX4.PreviewTextInput += PART_BOX4_PreviewTextInput;
                PART_BOX4.GotFocus += (o, e) => { IsKeyboardFocused = true; };
                PART_BOX4.LostFocus += (o, e) => { IsKeyboardFocused = false; };
                PART_BOX4.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }
        }

        void PART_BOX1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.V)
            {
                IDataObject data = Clipboard.GetDataObject();
                if (data.GetDataPresent(DataFormats.Text))
                {
                    string text = (string)data.GetData(DataFormats.UnicodeText);
                    Regex regex = new Regex(ipRegex);
                    if (regex.IsMatch(text))
                    {
                        string[] strs = text.Split(new char[] { '.' });
                        //因为已经判断过是正确的IP地址，因此不用判断索引是否越界
                        PART_BOX1.Text = strs[0];
                        PART_BOX2.Text = strs[1];
                        PART_BOX3.Text = strs[2];
                        PART_BOX4.Text = strs[3];

                        PART_BOX1.Focus();
                        PART_BOX1.SelectionStart = 0;
                    }
                    else
                    {
                        IsHasError = true;
                        ErrorContent = "您正在尝试将格式错误的 IP 地址粘贴到该字段";
                        PART_BOX1.Text = string.Empty;
                        PART_BOX2.Text = string.Empty;
                        PART_BOX3.Text = string.Empty;
                        PART_BOX4.Text = string.Empty;
                    }
                }
                e.Handled = true;
            }
        }

        #endregion

        #region private function
        /// <summary>
        /// 检查输入的字符是否为数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsNumber(string input)
        {
            Regex regex = new Regex("^[0-9]*$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 检查输入的数字是否在某个范围内
        /// </summary>
        /// <param name="number"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool IsNumberRange(int number, int start, int end)
        {
            return number >= start && number <= end;
        }

        /// <summary>
        /// 检查输入的文本是否符合IP地址格式
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        /// <returns></returns>
        private bool CheckNumberIsLegal(string text1, string text2)
        {
            if (!IsNumber(text2)) return true;

            int text = Convert.ToInt32(text1);

            return !IsNumberRange(text, 0, 255);
        }
        #endregion

        #region Event Implement Function
        void PART_BOX1_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = PART_BOX1.Text + e.Text;
            if (!string.IsNullOrWhiteSpace(input) && ".".Equals(e.Text))
            {
                e.Handled = true;
                PART_BOX2.Focus();
            }
            else
            {
                //输入的不是数字，则直接返回
                if (!IsNumber(e.Text))
                {
                    e.Handled = true;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(PART_BOX1.SelectedText))
                {
                    input = PART_BOX1.Text.Remove(PART_BOX1.SelectionStart, PART_BOX1.SelectionLength) + e.Text;
                }

                int text = Convert.ToInt32(input);
                switch (Type)
                {
                    case EnumIpBoxType.IpAddress:
                        if (!IsNumberRange(text, 0, 223))
                        {
                            e.Handled = true;
                            IsHasError = true;
                            ErrorContent = string.Format("{0} 不是有效项。请指定一个介于 1 和 223 间的值", input);
                            PART_BOX1.Text = "223";
                        }
                        else
                        {
                            IsHasError = false;
                        }
                        break;
                    case EnumIpBoxType.SubnetMask:
                        if (!IsNumberRange(text, 0, 255))
                        {
                            e.Handled = true;
                            IsHasError = true;
                            ErrorContent = string.Format(ZeroTo255Tip, input);
                            PART_BOX1.Text = "255";
                        }
                        else
                        {
                            IsHasError = false;
                        }
                        break;
                }
            }
        }

        void PART_BOX2_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = PART_BOX2.Text + e.Text;

            if (!string.IsNullOrWhiteSpace(PART_BOX2.SelectedText))
            {
                input = PART_BOX2.Text.Remove(PART_BOX2.SelectionStart, PART_BOX2.SelectionLength) + e.Text;
            }

            //如果输入了.且该文本框不为空，则自动跳到下一个输入框中
            if (!string.IsNullOrWhiteSpace(input) && ".".Equals(e.Text))
            {
                e.Handled = true;
                PART_BOX3.Focus();
            }
            else
            {
                e.Handled = CheckNumberIsLegal(input, e.Text);
                //如果输入不合法，则默认为255
                if (e.Handled)
                {
                    PART_BOX2.Text = "255";
                    PART_BOX2.SelectionStart = PART_BOX2.Text.Length + 1;
                    IsHasError = true;
                    ErrorContent = string.Format(ZeroTo255Tip, input);
                }
                else
                {
                    IsHasError = false;
                }
            }
        }

        void PART_BOX3_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = PART_BOX3.Text + e.Text;

            if (!string.IsNullOrWhiteSpace(PART_BOX3.SelectedText))
            {
                input = PART_BOX3.Text.Remove(PART_BOX3.SelectionStart, PART_BOX3.SelectionLength) + e.Text;
            }

            if (!string.IsNullOrWhiteSpace(input) && ".".Equals(e.Text))
            {
                e.Handled = true;
                PART_BOX4.Focus();
            }
            else
            {
                e.Handled = CheckNumberIsLegal(input, e.Text);
                if (e.Handled)
                {
                    PART_BOX3.Text = "255";
                    PART_BOX3.SelectionStart = PART_BOX3.Text.Length + 1;
                    IsHasError = true;
                    ErrorContent = string.Format(ZeroTo255Tip, input);
                }
                else
                {
                    IsHasError = false;
                }
            }
        }

        void PART_BOX4_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = PART_BOX4.Text + e.Text;

            if (!string.IsNullOrWhiteSpace(PART_BOX4.SelectedText))
            {
                input = PART_BOX4.Text.Remove(PART_BOX4.SelectionStart, PART_BOX4.SelectionLength) + e.Text;
            }

            e.Handled = CheckNumberIsLegal(input, e.Text);
            if (e.Handled)
            {
                PART_BOX4.Text = "255";
                PART_BOX4.SelectionStart = PART_BOX4.Text.Length + 1;
                IsHasError = true;
                ErrorContent = string.Format(ZeroTo255Tip, input);
            }
            else
            {
                IsHasError = false;
            }
        }

        void PART_BOX1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PART_BOX1.Text.Length == 3)
            {
                int number = 1;
                if (Int32.TryParse(PART_BOX1.Text, out number))
                {
                    switch (Type)
                    {
                        case EnumIpBoxType.IpAddress:
                            if (number < 1)
                            {
                                PART_BOX1.Text = "1";
                            }
                            break;
                        case EnumIpBoxType.SubnetMask:
                            break;
                    }
                }
                PART_BOX2.Focus();
            }
        }

        void PART_BOX2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PART_BOX2.Text.Length == 3)
            {
                PART_BOX3.Focus();
            }
        }

        void PART_BOX3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PART_BOX3.Text.Length == 3)
            {
                PART_BOX4.Focus();
            }
        }
        #endregion
    }
}
