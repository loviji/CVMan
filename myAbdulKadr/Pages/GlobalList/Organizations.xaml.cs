using myAbdulKadr.Common;
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
        private peopleEntities dbContext = DBContextResolver.Instance;

        public Organizations()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgOrg.ItemsSource = GetOrganizationList();

        }
        private ObservableCollection<organization> GetOrganizationList()
        {
            var list = from e in dbContext.organization select e;
            return new ObservableCollection<organization>(list);
        }



        private void dgOrg_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                organization org = e.Row.DataContext as organization;

                var matchedData = (from o in dbContext.organization
                                   where o.ID == org.ID
                                   select o).SingleOrDefault();
                if (matchedData == null)
                {
                    organization rOrganization = new organization();
                    rOrganization.organizationName = org.organizationName;
                    dbContext.organization.Add(rOrganization);
                    dbContext.SaveChanges();
                    dgOrg.ItemsSource = GetOrganizationList();
                    txtStatus.Text = rOrganization.organizationName + " has being added!";

                }
                else
                {
                    matchedData.organizationName = org.organizationName;
                    dbContext.SaveChanges();
                    dgOrg.ItemsSource = GetOrganizationList();
                    txtStatus.Text = "Məlumat yeniləndi";
                }
            }

        }

        private void DgOrg_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
           organization dOrg = dgOrg.SelectedItem as organization;

            if (dOrg != null)
            {
                var matchedOrganization = (from o in dbContext.organization
                                           where o.ID == dOrg.ID
                                           select o).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Əminsiniz?", "Sətir Silinir!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
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
