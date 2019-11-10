using myAbdulKadr.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace hydrogen.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private static peopleEntities dbContext = new peopleEntities();
        
       // ModernFrame frame;
        public Home()
        {
            InitializeComponent();

            perList.ItemsSource = (from m in dbContext.employee
                                   select new { m.ID, m.name, m.surname, m.secondname, m.birthdate, m.birthplace }).ToList();
        }

        private void AddNewPersonal_Click(object sender, RoutedEventArgs e)
        {
            //PersonalData lp = new PersonalData(4);
            //this.Content = lp;


            string url = "../PersonalDataPages/PersonDataEditor.xaml#0";
            NavigationCommands.GoToPage.Execute(url, this);
       
        }

        private void PerList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            //MessageBox.Show("clikecd");
        }

        private void PerList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var dg = sender as ListView;
            if (dg == null) return;
            //var index = dg.SelectedIndex;

            //ListViewItem riw = dg.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;

            //var item = dg.ItemContainerGenerator.ItemFromContainer(riw);
            var item = dg.SelectedItem;
            System.Type st = item.GetType();
            int selectedID = (int)(st.GetProperties()).Where(k => k.Name == "ID").FirstOrDefault().GetValue(item, null);

            //object propValue = props.GetValue(item, null);
            if (selectedID > 0)
            {
                //PersonDataEditor lp = new PersonDataEditor(selectedID.ToString());
                //this.Content = lp;


                string url = "../PersonalDataPages/PersonDataEditor.xaml#"+selectedID.ToString();
                NavigationCommands.GoToPage.Execute(url, this);
                
                //s.NavigationService.Navigate(new Page("passing a string to the constructor"));
            }
            // MessageBox.Show(selectedID.ToString());
            //foreach (PropertyInfo prop in props.Where(k=>k.Name=="id"))
            //{
            //    object propValue = prop.GetValue(item, null);
            //    MessageBox.Show(propValue.ToString());
            //}

            // var d=st.GetProperties().Where(k=>k.Name=="id").FirstOrDefault().GetValue();
            //  MessageBox.Show(d.ToString());
        }

        //private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        //{
        //    frame = NavigationHelper.FindFrame(null, this);
             
        //}

        //public int personID
        //{
        //    get { return (int)this.GetValue(StateProperty); }
        //    set { this.SetValue(StateProperty, value); }
        //}
        //public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
        //  "personID", typeof(int), typeof(MainWindow), new PropertyMetadata(0));


        //protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //}
    }
}
