using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    [TemplatePart(Name = "PART_SlideSwitchPanel", Type = typeof(SlideSwitchPanel))]
    [TemplatePart(Name = "PART_IndexPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PART_LastButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NextButton", Type = typeof(Button))]
    public class Carousel : Control
    {
        #region Private属性
        private SlideSwitchPanel PART_SlideSwitchPanel;
        private StackPanel PART_IndexPanel;
        private Button PART_LastButton;
        private Button PART_NextButton;
        /// <summary>
        /// 轮播的子项的个数
        /// </summary>
        private int ChildCount;
        /// <summary>
        /// 自动轮播定时器
        /// </summary>
        private Timer autoPlayTimer;
        /// <summary>
        /// 索引器分组依据，需唯一
        /// </summary>
        private string GroupName;
        #endregion

        #region 依赖属性定义
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemTemplateProperty;
        public static readonly DependencyProperty AutoPlayProperty;
        public static readonly DependencyProperty AutoPlaySpeedProperty;
        #endregion

        #region 依赖属性set get
        /// <summary>
        /// 轮播数据源
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 轮播的Item的数据模板
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        /// <summary>
        /// 是否自动播放
        /// </summary>
        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }
        /// <summary>
        /// 自动播放速度
        /// </summary>
        public double AutoPlaySpeed
        {
            get { return (double)GetValue(AutoPlaySpeedProperty); }
            set { SetValue(AutoPlaySpeedProperty, value); }
        }
        #endregion

        #region Constructors
        static Carousel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Carousel), new FrameworkPropertyMetadata(typeof(Carousel)));
            Carousel.ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(Carousel));
            Carousel.ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Carousel));
            Carousel.AutoPlayProperty = DependencyProperty.Register("AutoPlay", typeof(bool), typeof(Carousel), new PropertyMetadata(false, OnAutoPlayChangedCallback));
            Carousel.AutoPlaySpeedProperty = DependencyProperty.Register("AutoPlaySpeed", typeof(double), typeof(Carousel), new PropertyMetadata(2d, OnAutoPlaySpeedChangedCallback));
        }

        public Carousel()
        {
            //初始化，防止出现"未将对象引用设置到对象的实例"的错误
            ItemsSource = new List<object>();
            if (autoPlayTimer == null)
            {
                autoPlayTimer = new Timer();
                autoPlayTimer.Interval = AutoPlaySpeed;
                autoPlayTimer.Elapsed += AutoPlayTimer_Elapsed;
            }

            autoPlayTimer.Enabled = AutoPlay;
        }

        ~Carousel()
        {

        }
        #endregion

        #region 依赖属性回调
        private static void OnAutoPlaySpeedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;

            carousel.autoPlayTimer.Enabled = false;
            carousel.autoPlayTimer.Interval = (double)e.NewValue;
            carousel.autoPlayTimer.Enabled = carousel.AutoPlay;
        }

        private static void OnAutoPlayChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;
            carousel.autoPlayTimer.Enabled = (bool)e.NewValue;
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_SlideSwitchPanel = GetTemplateChild("PART_SlideSwitchPanel") as SlideSwitchPanel;
            PART_IndexPanel = GetTemplateChild("PART_IndexPanel") as StackPanel;
            PART_LastButton = GetTemplateChild("PART_LastButton") as Button;
            PART_NextButton = GetTemplateChild("PART_NextButton") as Button;

            GroupName = Guid.NewGuid().ToString("N");

            AddChildToPanel();
            AddIndexControlToPanel();

            if (PART_SlideSwitchPanel != null)
            {
                PART_SlideSwitchPanel.IndexChanged += PART_SlideSwitchPanel_IndexChanged;

            }

            MouseEnter += PART_SlideSwitchPanel_MouseEnter;
            MouseLeave += PART_SlideSwitchPanel_MouseLeave;

            if (PART_IndexPanel != null && PART_IndexPanel.Children.Count > 0)
            {
                ((RadioButton)PART_IndexPanel.Children[0]).IsChecked = true;
            }

            if (PART_LastButton != null)
            {
                PART_LastButton.Click += PART_LastButton_Click;
            }
            if (PART_NextButton != null)
            {
                PART_NextButton.Click += PART_NextButton_Click;
            }

            VisualStateManager.GoToState(this, "Normal", true);
        }
        #endregion

        #region Private方法
        /// <summary>
        /// 添加子控件
        /// </summary>
        private void AddChildToPanel()
        {
            if (ItemsSource == null)
            {
                return;
            }

            foreach (var item in ItemsSource)
            {
                ContentControl control = new ContentControl();
                control.Content = item;
                control.HorizontalAlignment = HorizontalAlignment.Stretch;
                control.HorizontalContentAlignment = HorizontalAlignment.Center;
                control.VerticalContentAlignment = VerticalAlignment.Center;
                control.ContentTemplate = ItemTemplate;
                PART_SlideSwitchPanel.Children.Add(control);
            }
        }

        /// <summary>
        /// 添加索引控件
        /// </summary>
        private void AddIndexControlToPanel()
        {
            if (PART_SlideSwitchPanel == null)
            {
                return;
            }
            if (PART_IndexPanel == null)
            {
                return;
            }

            int count = PART_SlideSwitchPanel.Children.Count;
            for (int i = 0; i < count; i++)
            {
                ZRadionButton radioButton = new ZRadionButton();
                //使用随机数作为RadioButton的分组依据，防止一个界面出现多个Carousel时，RadioButton选中出现问题
                radioButton.GroupName = "Index" + GroupName;
                radioButton.Checked += RadioButton_Checked;
                PART_IndexPanel.Children.Add(radioButton);
            }
            ChildCount = count;
        }

        private void HandleButtonMouse(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //VisualStateManager.GoToState(this, "ButtonMouseOver", true);
        }

        private void PART_NextButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchToNext();
        }

        private void PART_LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (PART_SlideSwitchPanel == null)
            {
                return;
            }

            int index = PART_SlideSwitchPanel.Index;
            index--;

            //当索引小于等于0时，切换到最大的索引的位置
            if (index <= 0)
            {
                index = ChildCount;
            }
            PART_SlideSwitchPanel.Index = index;
        }

        private void PART_SlideSwitchPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void PART_SlideSwitchPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (PART_SlideSwitchPanel == null)
            {
                return;
            }
            if (PART_IndexPanel == null)
            {
                return;
            }

            RadioButton btn = (RadioButton)e.OriginalSource;
            for (int i = 0; i < PART_IndexPanel.Children.Count; i++)
            {
                if (btn == PART_IndexPanel.Children[i] && i + 1 != PART_SlideSwitchPanel.Index)
                {
                    PART_SlideSwitchPanel.Index = i + 1;
                }
            }
        }

        private void PART_SlideSwitchPanel_IndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            SlideSwitchPanel panel = sender as SlideSwitchPanel;
            SetIndexPanelChecked(panel.Index);
        }

        private void SetIndexPanelChecked(int index)
        {
            if (PART_IndexPanel != null && PART_IndexPanel.Children[index - 1] is RadioButton)
            {
                RadioButton radioButton = PART_IndexPanel.Children[index - 1] as RadioButton;
                radioButton.IsChecked = true;
            }
        }

        private void AutoPlayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                SwitchToNext();
            }));
        }

        private void SwitchToNext()
        {
            if (PART_SlideSwitchPanel == null)
            {
                return;
            }

            int index = PART_SlideSwitchPanel.Index;
            index++;

            //当索引超过最大值时，回到第一个
            if (index > ChildCount)
            {
                index = 1;
            }

            PART_SlideSwitchPanel.Index = index;
        }
        #endregion
    }
}
