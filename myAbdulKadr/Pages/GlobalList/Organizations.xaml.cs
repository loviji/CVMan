using myAbdulKadr.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace myAbdulKadr.Pages.GlobalList
{
    /// <summary>
    /// Interaction logic for Organizations.xaml
    /// </summary>
    public partial class Organizations : UserControl
    {
        private static peopleEntities dbContext = new peopleEntities();

        public Organizations()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgOrg.ItemsSource = GetOrganizationList();

        }
        private ObservableCollection<Organization> GetOrganizationList()
        {
            var list = from e in dbContext.Organization select e;
            return new ObservableCollection<Organization>(list);
        }



        private void dgOrg_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Organization org = e.Row.DataContext as Organization;

                var matchedData = (from o in dbContext.Organization
                                   where o.ID == org.ID
                                   select o).SingleOrDefault();
                if (matchedData == null)
                {
                    Organization rOrganization = new Organization();
                    rOrganization.organizationName = org.organizationName;
                    dbContext.Organization.Add(rOrganization);
                    dbContext.SaveChanges();
                    dgOrg.ItemsSource = GetOrganizationList();
                    txtStatus.Text = rOrganization.organizationName + " has being added!";

                }
                else
                {
                    matchedData.organizationName = org.organizationName;
                    dbContext.SaveChanges();
                    dgOrg.ItemsSource = GetOrganizationList();
                    txtStatus.Text = "Success. Info updated";
                }
            }

        }

        private void DgOrg_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Organization dOrg = dgOrg.SelectedItem as Organization;

            if (dOrg != null)
            {
                var matchedOrganization = (from o in dbContext.Organization
                                           where o.ID == dOrg.ID
                                           select o).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Are you sure?", "Confirm Delete!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        dbContext.Entry(matchedOrganization).State = System.Data.Entity.EntityState.Deleted;
                        dbContext.SaveChanges();


                    }
                }
            }
        }
    }
}
