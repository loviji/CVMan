using myAbdulKadr.ViewModel;
using PersonMotion.Common;
using PersonMotion.Model;
using PersonMotion.WF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace myAbdulKadr.Pages
{
    /// <summary>
    /// Interaction logic for CV.xaml
    /// </summary>
    /// 
    public class SearchFieds
    {
        public int? OrgID { get; set; }
        public int? DeptID { get; set; }
        public int? SecID { get; set; }
    }

    public partial class CV : UserControl
    {
        private peopleEntities dbContext = DBContextResolver.Instance;

        private static ControlData cd = new ControlData();
        SearchFieds sf = new SearchFieds();

        public CV()
        {
            InitializeComponent();
            GlobalCache.showdialog = true;

            cmbOrganization.ItemsSource = cd.GetOrganizationList();
            cmbOrganization.DisplayMemberPath = "organizationName";
            cmbOrganization.SelectedValuePath = "ID";

         
            var d = new EmployeeViewModel();
            perList.DataContext = d;

            var employeeList = d.Employees.ToList();
            int totalValue = employeeList.Count();
            int totalNotExists = employeeList.Where(z => z.isfired == true).Count();
            int totalExists = totalValue - totalNotExists;
            infoCount.Content = "Ümumi - " + totalValue.ToString() + " (Faktiki - " + totalExists.ToString() + ", Xitam - " + totalNotExists.ToString() + ")";
        }

        private ObservableCollection<EMP_LIST> GetData(string name, string surname, string midname, int? orgID, int? deptID, int? sectID)
        {
            List<EMP_LIST> employeeList = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(midname) && !orgID.HasValue)
            {
                employeeList = new EmployeeViewModel().Employees.ToList();
            }
            else
            {
                employeeList = new EmployeeViewModel().Employees.Where(m => m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname) && (sf.OrgID == null || m.orgID == sf.OrgID) && (sf.DeptID == null || m.deptID == sf.DeptID) && (sf.SecID == null || m.sectID == sf.SecID)).ToList();
            }

            return new ObservableCollection<EMP_LIST>(employeeList);
        }



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            perList.ItemsSource = GetData(pName.Text, pSurname.Text, pMidName.Text, sf.OrgID, sf.DeptID, sf.OrgID);
        }

        private void PerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (perList.SelectedCells.Count > 0)
            {
                var cellInfo = perList.SelectedCells[0];
                var selectedEmp = (EMP_LIST)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedEmp.ID > 0)
                {
                    HRCard hc = new HRCard();
                    hc.personID = selectedEmp.ID.ToString();
                    hc.ShowDialog();
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
            sf.DeptID = Convert.ToInt32(cmbDepartment.SelectedValue);

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
