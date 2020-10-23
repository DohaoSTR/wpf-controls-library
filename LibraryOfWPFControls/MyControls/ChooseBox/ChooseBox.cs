﻿using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class ChooseBox : TextBox
    {
        #region private fields
        private Button PART_ChooseButton;
        #endregion

        #region DependencyProperty

        #region ChooseButtonStyle
        /// <summary>
        /// 获取或者设置选择按钮的样式
        /// </summary>
        public Style ChooseButtonStyle
        {
            get => (Style)GetValue(ChooseButtonStyleProperty);
            set => SetValue(ChooseButtonStyleProperty, value);
        }

        public static readonly DependencyProperty ChooseButtonStyleProperty =
            DependencyProperty.Register("ChooseButtonStyle", typeof(Style), typeof(ChooseBox));

        #endregion

        #region ChooseBoxType

        public EnumChooseBoxType ChooseBoxType
        {
            get => (EnumChooseBoxType)GetValue(ChooseBoxTypeProperty);
            set => SetValue(ChooseBoxTypeProperty, value);
        }

        public static readonly DependencyProperty ChooseBoxTypeProperty =
            DependencyProperty.Register("ChooseBoxType", typeof(EnumChooseBoxType), typeof(ChooseBox), new PropertyMetadata(EnumChooseBoxType.SingleFile));

        #endregion

        #region ChooseButtonWidth

        public double ChooseButtonWidth
        {
            get => (double)GetValue(ChooseButtonWidthProperty);
            set => SetValue(ChooseButtonWidthProperty, value);
        }

        public static readonly DependencyProperty ChooseButtonWidthProperty =
            DependencyProperty.Register("ChooseButtonWidth", typeof(double), typeof(ChooseBox), new PropertyMetadata(20d));

        #endregion

        #endregion

        #region Constructors

        static ChooseBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChooseBox), new FrameworkPropertyMetadata(typeof(ChooseBox)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_ChooseButton = GetTemplateChild("PART_ChooseButton") as Button;
            if (PART_ChooseButton != null)
            {
                PART_ChooseButton.Click += PART_ChooseButton_Click;
            }
        }

        #endregion

        #region private function

        #endregion

        #region Event Implement Function
        private void PART_ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ChooseBoxType)
            {
                case EnumChooseBoxType.SingleFile:
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
                    {
                        Multiselect = false
                    };
                    //"文本文件|*.*|C#文件|*.cs|所有文件|*.*"
                    //openFileDialog.Filter = this.Filter;
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Text = openFileDialog.FileName;
                    }
                    break;
                case EnumChooseBoxType.MultiFile:
                    break;
                case EnumChooseBoxType.Folder:
                    System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Text = folderDialog.SelectedPath;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}