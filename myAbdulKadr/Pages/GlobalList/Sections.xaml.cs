using myAbdulKadr.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace myAbdulKadr.Pages.GlobalList
{
    /// <summary>
    /// Interaction logic for Sections.xaml
    /// </summary>
    public partial class Sections : UserControl
    {
        private static peopleEntities dbContext = new peopleEntities();

        public Sections()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }

        private int selectedDeptID = 0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            cmbOrganization.ItemsSource = GetOrganizationList();
            cmbOrganization.DisplayMemberPath = "organizationName";
            cmbOrganization.SelectedValuePath = "ID";




        }

        private ObservableCollection<Organization> GetOrganizationList()
        {
            var list = from e in dbContext.Organization select e;

            return new ObservableCollection<Organization>(list);
        }


        private ObservableCollection<department> GetDepartmentList(int orgID)
        {
            var list = from dp in dbContext.department
                       where dp.organizationID == orgID
                       select dp;
            return new ObservableCollection<department>(list);
        }


        private ObservableCollection<section> GetSectionList(int deptID)
        {
            var list = from sc in dbContext.section
                       where sc.departmentID == deptID
                       select sc;
            return new ObservableCollection<section>(list);
        }



        private void dgSect_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                section sect = e.Row.DataContext as section;

                var matchedData = (from sc in dbContext.section
                                   where sc.ID == sect.ID
                                   select sc).SingleOrDefault();
                if (matchedData == null)
                {
                    section rSection = new section();
                    rSection.sectionName = sect.sectionName;
                    rSection.departmentID = selectedDeptID;
                    dbContext.section.Add(rSection);
                    dbContext.SaveChanges();
                    dgSect.ItemsSource = GetSectionList(selectedDeptID);
                    txtStatus.Text = rSection.sectionName + " has being added!";

                }
                else
                {
                    matchedData.sectionName = sect.sectionName;
                    dbContext.SaveChanges();
                    dgSect.ItemsSource = GetSectionList(selectedDeptID);
                    txtStatus.Text = "Success. Info updated";
                }
            }

        }

        private void DgSect_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            section dSect = dgSect.SelectedItem as section;

            if (dSect != null)
            {
                var matchedSection = (from sc in dbContext.section
                                              where sc.ID == dSect.ID
                                         select sc).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Are you sure?", "Confirm Delete!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        dbContext.Entry(matchedSection).State = System.Data.Entity.EntityState.Deleted;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        private int selectedOrgID = 0;

        private void CmbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
            cmbDepartment.ItemsSource = GetDepartmentList(selectedOrgID);
            cmbDepartment.DisplayMemberPath = "departmentName";
            cmbDepartment.SelectedValuePath = "ID";
        }

        private void CmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDeptID = Convert.ToInt32(cmbDepartment.SelectedValue);
            dgSect.ItemsSource = GetSectionList(selectedDeptID);
        }
    }
}
