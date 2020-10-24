using LibraryOfWPFControls.Test.UserControls;
using LibraryOfWPFControls.Test.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI.Test
{
    public partial class Home : Window
    {
        public ObservableCollection<MenuInfo> MenuList { get; set; }

        private static Home instance;

        public static Home Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Home();
                }
                return instance;
            }
        }

        public Home()
        {
            InitializeComponent();

            MenuList = new ObservableCollection<MenuInfo>
            {
                new MenuInfo()
                {
                    Name = "Редактируемый текстовый элемент",
                },
                new MenuInfo()
                {
                    Name = "Переключатель (да/нет)",
                },
                new MenuInfo()
                {
                    Name = "Список изображений",
                },
                new MenuInfo()
                {
                    Name = "Слайдер числовых значений",
                },
                new MenuInfo()
                {
                    Name = "Выбор времени/даты",
                },
                new MenuInfo()
                {
                    Name = "Поля форматированного ввода",
                },
                new MenuInfo()
                {
                    Name = "Элемент для выбора файла",
                },
                new MenuInfo()
                {
                    Name = "Выбор директории",
                },
                new MenuInfo()
                {
                    Name = "Выбор цвета",
                },
            };

            menu.GroupItemsSource = MenuList;
            menu.GroupDescriptions = "GroupName";
        }

        private void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menu.SelectedItem != null)
            {
                MenuInfo info = menu.SelectedItem as MenuInfo;
                switch (info.Name)
                {
                    case "Редактируемый текстовый элемент":
                        ControlPanel.Content = new TestTextBox();
                        break;
                    case "Переключатель (да/нет)":
                        ControlPanel.Content = new TestToggleButton();
                        break;
                    case "Список изображений":
                        ControlPanel.Content = new TestListOfImages();
                        break;
                    case "Слайдер числовых значений":
                        ControlPanel.Content = new TestSlider();
                        break;
                    case "Выбор времени/даты":
                        ControlPanel.Content = new TestDateTimeControl();
                        break;
                    case "Поля форматированного ввода":
                        ControlPanel.Content = new TestInputBox();
                        break;
                    case "Элемент для выбора файла":
                        ControlPanel.Content = new TestUploadControl();
                        break;
                    case "Выбор директории":
                        ControlPanel.Content = new TestChooseBox();
                        break;
                    case "Выбор цвета":
                        ControlPanel.Content = new TestColorSelector();
                        break;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(0);
            base.OnClosed(e);
        }
    }
}
