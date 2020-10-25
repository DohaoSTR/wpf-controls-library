using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace LibraryOfWPFControls.Test.UserControls
{
    public partial class TestListOfImages : UserControl
    {
        public TestListOfImages()
        {
            InitializeComponent();

            ObservableCollection<string> list = new ObservableCollection<string>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(i.ToString());
            }
            Carousel.ItemsSource = list;

            FileStream f = new FileStream("img1.png", FileMode.Open, FileAccess.Read);
            string path = f.Name.Remove(f.Name.Length - 5);
            ObservableCollection<CarouselModel> data = new ObservableCollection<CarouselModel>();
            for (int i = 1; i <= 5; i++)
            {
                data.Add(new CarouselModel()
                {
                    Title = i.ToString(),
                    ImageUrl = path + i + ".png",
                });
            }
            Carousel2.ItemsSource = data;
        }
    }

    public class CarouselModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }
    }
}
