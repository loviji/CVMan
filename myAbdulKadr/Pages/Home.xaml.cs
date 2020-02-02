using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using myAbdulKadr.ViewModel;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace hydrogen.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    /// 
    public class SearchFieds
    {
        public int? OrgID { get; set; }
        public int? DeptID { get; set; }
        public int? SecID { get; set; }
    }

    public partial class Home : UserControl, IContent
    {
        private peopleEntities dbContext = DBContextResolver.Instance;

        private static ControlData cd = new ControlData();
        SearchFieds sf = new SearchFieds();
        public Home()
        {
            InitializeComponent();
            GlobalCache.showdialog = true;
            
            cmbOrganization.ItemsSource = cd.GetOrganizationList();
            cmbOrganization.DisplayMemberPath = "organizationName";
            cmbOrganization.SelectedValuePath = "ID";

        }

        private ObservableCollection<EMP_LIST> GetData(string name, string surname, string midname, int? orgID, int? deptID, int? sectID)
        {
            List<EMP_LIST> employeeList = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(midname)&&!orgID.HasValue)
            {
                employeeList = new EmployeeViewModel().Employees.ToList();
            }
            else
            {
                employeeList = new EmployeeViewModel().Employees.Where(m => m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)&&(sf.OrgID==null||m.orgID==sf.OrgID)&&(sf.DeptID==null||m.deptID==sf.DeptID)&&(sf.SecID== null||m.sectID==sf.SecID)).ToList();
            }
            int totalValue = employeeList.Count();
            int totalNotExists = employeeList.Where(d=>d.isfired==true).Count();
            int totalExists = totalValue- totalNotExists;
            return new ObservableCollection<EMP_LIST>(employeeList);
           
        }


        private void AddNewPersonal_Click(object sender, RoutedEventArgs e)
        {
            string url = "../PersonalDataPages/PersonDataEditor.xaml#0";
            NavigationCommands.GoToPage.Execute(url, this);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
             perList.ItemsSource = GetData(pName.Text, pSurname.Text, pMidName.Text,  sf.OrgID, sf.DeptID, sf.OrgID);
        }

        private const string mainWindowHeader = "Person motion";
        private string personFullName = string.Empty;
        private void PerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (perList.SelectedCells.Count > 0)
            {
                var cellInfo = perList.SelectedCells[0];
                var selectedEmp = (EMP_LIST)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedEmp.ID > 0)
                {
                    string url = "../PersonalDataPages/PersonDataEditor.xaml#" + selectedEmp.ID.ToString();
                    Window mainWindow = Application.Current.MainWindow;
                    personFullName = dbContext.employee.Where(s => s.ID == selectedEmp.ID).Select(s => s.name + " " + s.surname + " " + s.secondname).SingleOrDefault();
                    mainWindow.Title = mainWindowHeader + " " + personFullName;
                    NavigationCommands.GoToPage.Execute(url, this);
                }
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {

        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            perList.DataContext = new EmployeeViewModel();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }

        private void CmbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            sf.OrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
            cmbDepartment.ItemsSource = sf.OrgID.HasValue ? cd.GetDepartmentList(sf.OrgID.Value) : null;
            cmbDepartment.DisplayMemberPath = "departmentName";
            cmbDepartment.SelectedValuePath = "ID";
        }

        private void CmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sf.DeptID= Convert.ToInt32(cmbDepartment.SelectedValue);

            cmbSection.ItemsSource = sf.DeptID.HasValue ? cd.GetSectionList(sf.DeptID.Value) : null;
            cmbSection.DisplayMemberPath = "sectionName";
            cmbSection.SelectedValuePath = "ID";
            
        }

        private void CmbSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sf.SecID = Convert.ToInt32(cmbSection.SelectedValue);
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            pName.Text = string.Empty;
            pSurname.Text = string.Empty;
            pMidName.Text = string.Empty;
            cmbOrganization.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
            cmbSection.SelectedIndex = -1;
            sf.OrgID = null;
            sf.DeptID = null;
            sf.SecID = null;
        }
    }
}
