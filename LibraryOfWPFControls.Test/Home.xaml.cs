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

            Utils.PaletteHelper.SetLightDarkTheme(false);

            MenuList = new ObservableCollection<MenuInfo>();

            MenuList.Add(new MenuInfo()
            {
                Name = "Редактируемый текстовый элемент по двойному нажатию",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Переключатель (да/нет)",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Список изображений",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Слайдер числовых значений",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Элемент для выбора времени/даты",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Поле форматированного ввода",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Элемент для выбора файла",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Обозреватель свойств объекта",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Выбор директории",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Выбор цвета",
            });
            MenuList.Add(new MenuInfo()
            {
                Name = "Выбор шрифта и его параметров",
            });

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
