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
        public int? orgID { get; set; }
        public int? deptID { get; set; }
        public int? secID { get; set; }
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
            // cmbOrganization.SelectedValue = positionRecord.orgID;

            //var selectedOrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
            // cmbDepartment.ItemsSource = positionRecord.orgID.HasValue ? cd.GetDepartmentList(positionRecord.orgID.Value) : null;
            // cmbDepartment.DisplayMemberPath = "departmentName";
            // cmbDepartment.SelectedValuePath = "ID";
            //// cmbDepartment.SelectedValue = positionRecord.deptID;

            // //var selectedDeptID = Convert.ToInt32(cmbDepartment.SelectedValue);
            // cmbSection.ItemsSource = positionRecord.deptID.HasValue ? cd.GetSectionList(positionRecord.deptID.Value) : null;
            // cmbSection.DisplayMemberPath = "sectionName";
            // cmbSection.SelectedValuePath = "ID";
            //// cmbSection.SelectedValue = positionRecord.sectID;




            //GetData(string.Empty, string.Empty, string.Empty);
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
                employeeList = new EmployeeViewModel().Employees.Where(m => m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)&&m.orgID==sf.orgID&&m.deptID==sf.deptID&&m.sectID==sf.secID).ToList();
                //where m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)
                //select m).ToList();

            }
            //new { m.ID, m.name, m.surname, m.secondname, m.birthdate, m.birthplace });
            return new ObservableCollection<EMP_LIST>(employeeList);
            //return ObservableCollection<d;
        }


        private void AddNewPersonal_Click(object sender, RoutedEventArgs e)
        {
            string url = "../PersonalDataPages/PersonDataEditor.xaml#0";
            NavigationCommands.GoToPage.Execute(url, this);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            
            perList.ItemsSource = GetData(pName.Text, pSurname.Text, pMidName.Text,  sf.orgID, sf.deptID, sf.orgID);
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
            // perList.ItemsSource = null;
            // perList.ItemsSource = GetData(string.Empty, string.Empty, string.Empty);
            //perList.Items.Refresh();
            // var d = new EmployeeViewModel();
            perList.DataContext = new EmployeeViewModel();

            //perList.ItemsSource = null;

            //         perList.ItemsSource = new EmployeeViewModel().Employees;//Preferably do this somewhere else, not in the add method.
            //perList.Items.Refresh();

        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }

        private void CmbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            sf.orgID = Convert.ToInt32(cmbOrganization.SelectedValue);


            cmbDepartment.ItemsSource = sf.orgID.HasValue ? cd.GetDepartmentList(sf.orgID.Value) : null;
            cmbDepartment.DisplayMemberPath = "departmentName";
            cmbDepartment.SelectedValuePath = "ID";
           

        }

        private void CmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sf.deptID= Convert.ToInt32(cmbDepartment.SelectedValue);

            cmbSection.ItemsSource = sf.deptID.HasValue ? cd.GetSectionList(sf.deptID.Value) : null;
            cmbSection.DisplayMemberPath = "sectionName";
            cmbSection.SelectedValuePath = "ID";
            
        }

        private void CmbSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sf.secID = Convert.ToInt32(cmbSection.SelectedValue);
        }
    }
}
