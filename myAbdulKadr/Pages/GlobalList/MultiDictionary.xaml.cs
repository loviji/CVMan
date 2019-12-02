using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using System.Windows.Shapes;

namespace PersonMotion.Pages.GlobalList
{
    /// <summary>
    /// Interaction logic for MultiDictionary.xaml
    /// </summary>
    public partial class MultiDictionary : UserControl, IContent, INotifyPropertyChanged
    {

        private peopleEntities dbContext = DBContextResolver.Instance;

        public MultiDictionary()
        {
            InitializeComponent();
        }

        private ObservableCollection<organization> GetOrganizationList()
        {
            var list = from e in dbContext.organization select e;
            return new ObservableCollection<organization>(list.ToList());
        }

        private List<organization> GetOrganizationList2()
        {
            var list = from e in dbContext.organization select e;
            return list.ToList();
        }


        private ObservableCollection<department> GetDepartmentList(int orgID)
        {
            var list = from dp in dbContext.department
                       where dp.organizationID == orgID
                       select dp;
            return new ObservableCollection<department>(list);
        }


        private void dgDept_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                department dept = e.Row.DataContext as department;

                var matchedData = (from dp in dbContext.department
                                   where dp.ID == dept.ID
                                   select dp).SingleOrDefault();
                if (matchedData == null)
                {
                    department rDepartment = new department();
                    rDepartment.departmentName = dept.departmentName;
                    rDepartment.organizationID = selectedMDictID;
                    dbContext.department.Add(rDepartment);
                    dbContext.SaveChanges();
                    dgDept.ItemsSource = GetDepartmentList(selectedMDictID);
                    txtStatus.Text = rDepartment.departmentName + " has being added!";

                }
                else
                {
                    matchedData.departmentName = dept.departmentName;
                    dbContext.SaveChanges();
                    dgDept.ItemsSource = GetDepartmentList(selectedMDictID);
                    txtStatus.Text = "Success. Info updated";
                }
            }

        }

        private void DgDept_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            department dDept = dgDept.SelectedItem as department;

            if (dDept != null)
            {
                var matchedDepartment = (from o in dbContext.department
                                         where o.ID == dDept.ID
                                         select o).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        dbContext.Entry(matchedDepartment).State = System.Data.Entity.EntityState.Deleted;
                        dbContext.SaveChanges();


                    }
                }
            }
        }

        private int selectedMDictID = 0;

        private ObservableCollection<organization> orgList;

        public ObservableCollection<organization> OrgList
        {
            get
            {
                return orgList;
            }
            set
            {
                if (value != orgList)
                {
                    orgList = value;
                    NotifyPropertyChanged("OrgList"); // method implemented below
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CmbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMDictID = Convert.ToInt32(cmbMDict.SelectedValue);
            dgDept.ItemsSource = GetDepartmentList(selectedMDictID);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {

        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {

            cmbMDict.Items.Clear();
            //OrgList = GetOrganizationList();

            cmbMDict.ItemsSource = GetOrganizationList2();
            cmbMDict.DisplayMemberPath = "organizationName";
            cmbMDict.SelectedValuePath = "ID";
            dgDept.ItemsSource = GetDepartmentList(selectedMDictID);
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            cmbMDict.ItemsSource = null;
            dgDept.ItemsSource = null;
        }

       
    }
}

