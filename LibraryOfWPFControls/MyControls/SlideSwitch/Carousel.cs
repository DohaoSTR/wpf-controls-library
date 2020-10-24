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
        private SlideSwitchPanel PART_SlideSwitchPanel;
        private StackPanel PART_IndexPanel;
        private Button PART_LastButton;
        private Button PART_NextButton;
        private int ChildCount;
        private readonly Timer autoPlayTimer;
        private string GroupName;

        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemTemplateProperty;
        public static readonly DependencyProperty AutoPlayProperty;
        public static readonly DependencyProperty AutoPlaySpeedProperty;

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty);
            set => SetValue(AutoPlayProperty, value);
        }

        public double AutoPlaySpeed
        {
            get => (double)GetValue(AutoPlaySpeedProperty);
            set => SetValue(AutoPlaySpeedProperty, value);
        }

        static Carousel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Carousel), new FrameworkPropertyMetadata(typeof(Carousel)));
            ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(Carousel));
            ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Carousel));
            AutoPlayProperty = DependencyProperty.Register("AutoPlay", typeof(bool), typeof(Carousel), new PropertyMetadata(false, OnAutoPlayChangedCallback));
            AutoPlaySpeedProperty = DependencyProperty.Register("AutoPlaySpeed", typeof(double), typeof(Carousel), new PropertyMetadata(2d, OnAutoPlaySpeedChangedCallback));
        }

        public Carousel()
        {
            ItemsSource = new List<object>();
            if (autoPlayTimer == null)
            {
                autoPlayTimer = new Timer
                {
                    Interval = AutoPlaySpeed
                };
                autoPlayTimer.Elapsed += AutoPlayTimer_Elapsed;
            }

            autoPlayTimer.Enabled = AutoPlay;
        }

        ~Carousel()
        {

        }

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

        private void AddChildToPanel()
        {
            if (ItemsSource == null)
            {
                return;
            }

            foreach (object item in ItemsSource)
            {
                ContentControl control = new ContentControl
                {
                    Content = item,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    ContentTemplate = ItemTemplate
                };
                PART_SlideSwitchPanel.Children.Add(control);
            }
        }

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
                ZRadionButton radioButton = new ZRadionButton
                {
                    GroupName = "Index" + GroupName
                };
                radioButton.Checked += RadioButton_Checked;
                PART_IndexPanel.Children.Add(radioButton);
            }
            ChildCount = count;
        }

        private void HandleButtonMouse(object sender, System.Windows.Input.MouseEventArgs e)
        {

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

            if (index > ChildCount)
            {
                index = 1;
            }

            PART_SlideSwitchPanel.Index = index;
        }
    }
}
