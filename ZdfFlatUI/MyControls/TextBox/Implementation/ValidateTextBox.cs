using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    /// <summary>
    /// 自带数据校验的文本输入框
    /// 目前已有数据校验如下：
    /// 1、为空判断：IsRequired、RequiredMessage
    /// 2、数字校验：IsNumber、NumberMessage
    /// 3、号码校验：IsPhoneNumber、PhoneNumberMessage
    /// </summary>
    /// <remarks>add by zhidf 2016.7.31</remarks>
    public class ValidateTextBox : TextBox
    {
        private bool mIsValidatePass;

        static ValidateTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidateTextBox), new FrameworkPropertyMetadata(typeof(ValidateTextBox)));
        }

        public ValidateTextBox() : base()
        {
            LostFocus += ValidateTextBox_LostFocus;
            Loaded += ValidateTextBox_Loaded;
            IsEnabledChanged += ValidateTextBox_IsEnabledChanged;
        }

        private void ValidateTextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsEnabled)
            {

            }
        }

        private void ValidateTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            switch (ValidateType)
            {
                case EnumValidateType.Loaded:
                    BeginValidate();
                    break;
                case EnumValidateType.LostFocus:
                    break;
                default:
                    break;
            }
        }

        private void ValidateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            BeginValidate();
        }

        private void BeginValidate()
        {
            if (IsRequired && CheckIsEmpty())
            {
                ShowError(RequiredMessage);
                return;
            }

            if (IsNumber && !CheckIsNumber())
            {
                ShowError(NumberMessage);
                return;
            }

            if (IsPhoneNumber && !CheckIsPhoneNum())
            {
                ShowError(PhoneNumberMessage);
                return;
            }

            HideError();
        }

        #region 控件名称
        private Popup PART_ErrorPopup;
        private TextBlock PART_ErrorContent;
        #endregion

        #region 依赖属性

        #region 是否必填
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired"
            , typeof(bool), typeof(ValidateTextBox));
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        public static readonly DependencyProperty RequiredMessageProperty = DependencyProperty.Register("RequiredMessage"
            , typeof(string), typeof(ValidateTextBox));
        /// <summary>
        /// 是否必填
        /// </summary>
        public string RequiredMessage
        {
            get { return (string)GetValue(RequiredMessageProperty); }
            set { SetValue(RequiredMessageProperty, value); }
        }
        #endregion

        #region 是否为数字
        public static readonly DependencyProperty IsNumberProperty = DependencyProperty.Register("IsNumber"
            , typeof(bool), typeof(ValidateTextBox));
        /// <summary>
        /// 是否为数字
        /// </summary>
        public bool IsNumber
        {
            get { return (bool)GetValue(IsNumberProperty); }
            set { SetValue(IsNumberProperty, value); }
        }

        public static readonly DependencyProperty NumberMessageProperty = DependencyProperty.Register("NumberMessage"
            , typeof(string), typeof(ValidateTextBox));
        /// <summary>
        /// 是否为数字
        /// </summary>
        public string NumberMessage
        {
            get { return (string)GetValue(NumberMessageProperty); }
            set { SetValue(NumberMessageProperty, value); }
        }
        #endregion

        #region 是否为号码（电话号码、手机号码）
        public static readonly DependencyProperty IsPhoneNumberProperty = DependencyProperty.Register("IsPhoneNumber"
            , typeof(bool), typeof(ValidateTextBox));
        /// <summary>
        /// 是否为号码（电话号码、手机号码）
        /// </summary>
        public bool IsPhoneNumber
        {
            get { return (bool)GetValue(IsPhoneNumberProperty); }
            set { SetValue(IsPhoneNumberProperty, value); }
        }

        public static readonly DependencyProperty PhoneNumberMessageProperty = DependencyProperty.Register("PhoneNumberMessage"
            , typeof(string), typeof(ValidateTextBox));
        /// <summary>
        /// 是否为号码（电话号码、手机号码）的提示信息
        /// </summary>
        public string PhoneNumberMessage
        {
            get { return (string)GetValue(PhoneNumberMessageProperty); }
            set { SetValue(PhoneNumberMessageProperty, value); }
        }
        #endregion

        #region 是否校验
        public static readonly DependencyProperty IsValidateProperty = DependencyProperty.Register("IsValidate"
            , typeof(bool), typeof(ValidateTextBox)
            , new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsValidateChanged)));

        /// <summary>
        /// 是否校验
        /// </summary>
        public bool IsValidate
        {
            get { return (bool)GetValue(IsValidateProperty); }
            set { SetValue(IsValidateProperty, value); }
        }

        private static void OnIsValidateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ValidateTextBox validateTextBox = (ValidateTextBox)sender;
            if (e.Property == IsValidateProperty)
            {
                if (Convert.ToBoolean(e.NewValue))
                {
                    validateTextBox.BeginValidate();
                }
            }
        }
        #endregion

        public static readonly DependencyProperty ValidateTypeProperty = DependencyProperty.Register("ValidateType"
            , typeof(EnumValidateType), typeof(ValidateTextBox));

        /// <summary>
        /// 
        /// </summary>
        public EnumValidateType ValidateType
        {
            get { return (EnumValidateType)GetValue(ValidateTypeProperty); }
            set { SetValue(ValidateTypeProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_ErrorPopup = GetTemplateChild("PART_ErrorPopup") as Popup;
            PART_ErrorContent = GetTemplateChild("PART_ErrorContent") as TextBlock;
        }

        #region 校验方法
        /// <summary>
        /// 显示错误提示信息
        /// </summary>
        /// <param name="errorContent"></param>
        private void ShowError(string errorContent)
        {
            PART_ErrorContent.Text = errorContent;
            VisualStateManager.GoToState(this, "InvalidFocused", true);
            //this.PART_ErrorPopup.IsOpen = true;

        }
        /// <summary>
        /// 隐藏错误提示信息
        /// </summary>
        private void HideError()
        {
            //this.PART_ErrorPopup.IsOpen = false;
            VisualStateManager.GoToState(this, "ValidUnfocused", true);
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <returns></returns>
        private bool CheckIsEmpty()
        {
            return string.IsNullOrEmpty(Text);
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <returns></returns>
        private bool CheckIsNumber()
        {
            return Regex.IsMatch(Text, @"^[0-9]*$");
        }

        private bool CheckIsPhoneNum()
        {
            return Regex.IsMatch(Text, @"^[1][358][0-9]{9}$");
        }
        #endregion
    }

    public enum EnumValidateType
    {
        LostFocus,
        Loaded,
    }
}
