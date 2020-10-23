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
                    Name = "Редактируемый текстовый элемент по двойному нажатию",
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
                    Name = "Элемент для выбора времени/даты",
                },
                new MenuInfo()
                {
                    Name = "Поле форматированного ввода",
                },
                new MenuInfo()
                {
                    Name = "Элемент для выбора файла",
                },
                new MenuInfo()
                {
                    Name = "Обозреватель свойств объекта",
                },
                new MenuInfo()
                {
                    Name = "Выбор директории",
                },
                new MenuInfo()
                {
                    Name = "Выбор цвета",
                },
                new MenuInfo()
                {
                    Name = "Выбор шрифта и его параметров",
                }
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
                    case "Редактируемый текстовый элемент по двойному нажатию":
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
                    case "Элемент для выбора времени/даты":
                        ControlPanel.Content = new TestDateTimeControl();
                        break;
                    case "Поле форматированного ввода":
                        ControlPanel.Content = new TestTextBox();
                        break;
                    case "Элемент для выбора файла":
                        ControlPanel.Content = new TestUploadControl();
                        break;
                    case "Обозреватель свойств объекта":
                        //ControlPanel.Content = new TestSlider();
                        break;
                    case "Выбор директории":
                        ControlPanel.Content = new TestChooseBox();
                        break;
                    case "Выбор цвета":
                        ControlPanel.Content = new TestColorSelector();
                        break;
                    case "Выбор шрифта и его параметров":
                        //ControlPanel.Content = new TestSlider();
                        break;

                        // разделить редактируемый текствой элемент и поля форматированного ввода
                        // В библиотеке есть множество одиночных элементов которые можно использовать
                        // Сделать нормальный выбор цвета
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
