using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace LibraryOfWPFControls.Test.UserControls
{
    /// <summary>
    /// Interaction logic for TestListOfImages.xaml
    /// </summary>
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

            ObservableCollection<CarouselModel> data = new ObservableCollection<CarouselModel>();
            for (int i = 1; i <= 5; i++)
            {
                data.Add(new CarouselModel()
                {
                    Title = i.ToString(),
                    ImageUrl = string.Format(@"C:\Users\muzal\Downloads\WPF.UI-master\WPF.UI-master\LibraryOfWPFControls.Test\Images\img{0}.png", i),
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
