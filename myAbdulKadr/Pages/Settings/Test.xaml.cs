using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hydrogen.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
        }

        int res = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            res=Convert.ToInt32(tb1.Text) + Convert.ToInt32(tb2.Text);

            tb3.Text = res.ToString();
        }
        private DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)

        {

            //Walk the visual tree to get the parent(ItemsControl)

            //of this control

            DependencyObject parent = startObject;

            while (parent != null)

            {

                if (type.IsInstanceOfType(parent))

                    break;

                else

                    parent = VisualTreeHelper.GetParent(parent);

            }

            return parent;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;

            //pg.NavigationService.Navigate(new Uri("/Pages/Settings/Twister.xaml?sendedParam=7", UriKind.Relative));

            //pg.NavigationService.Navigate(new Uri("/Pages/Settings/Twister.xaml?parameter=test", UriKind.Relative));
            var k=tb3.Text;
            string url = "/Pages/Settings/Twister.xaml#" + k.ToString();
            NavigationCommands.GoToPage.Execute(url, this);
            //NavigationCommands.GoToPage.Execute("/Pages/Settings/Twister.xaml#sendedParam=7", this);
            //BBCodeBlock bs = new BBCodeBlock();
            //try
            //{
            //    bs.LinkNavigator.Navigate(new Uri("/Pages/Settings/Twister.xaml", UriKind.Relative), this, res.ToString());
            //}
            //catch (Exception error)
            //{
            //    ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
            //}
        }


        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    // NavigationEventArgs returns destination page
        //    Page destinationPage = e.Content as Page;
        //    if (destinationPage != null)
        //    {

        //        // Change property of destination page
        //        destinationPage.PublicProperty = "String or object..";
        //    }
        //}
    }
}
