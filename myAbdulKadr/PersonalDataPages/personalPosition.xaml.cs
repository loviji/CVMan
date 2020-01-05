using FirstFloor.ModernUI.Windows;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace hydrogen.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalPosition.xaml
    /// </summary>
    public partial class personalPosition : UserControl, IContent
    {
        private int selectedPersonID { get; set; }
        private static peopleEntities dbContext = DBContextResolver.Instance;
        private static ControlData cd = new ControlData();
        public personalPosition()
        {
            InitializeComponent();

        }

        private void FillPositionList(int selectedID)
        {
            positionList.DataContext = dbContext.VW_Position.Where(v => v.empID == selectedID).ToList();
        }


        void IContent.OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

            selectedPersonID = Convert.ToInt32(e.Fragment);
            
            if (GlobalCache.currentPersonID!=0)
            {
                selectedPersonID = GlobalCache.currentPersonID;
                FillPositionList(selectedPersonID);
            }
            //txtIDSaver.Text = e.Fragment;

        }

        void IContent.OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            // throw new NotImplementedException();
        }

        void IContent.OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            //  throw new NotImplementedException();
        }

        void IContent.OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            // throw new NotImplementedException();
        }
        private int selectedPosition = 0;
        private void PositionList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (positionList.SelectedCells.Count > 0)
            {
                var cellInfo = positionList.SelectedCells[0];
                var selectedGridPosition = (VW_Position)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedGridPosition.ID > 0)
                {
                    selectedPosition = selectedGridPosition.ID;

                    var positionRecord = getPositionByID(selectedGridPosition.ID);

                    cmbOrganization.ItemsSource = cd.GetOrganizationList();
                    cmbOrganization.DisplayMemberPath = "organizationName";
                    cmbOrganization.SelectedValuePath = "ID";
                    cmbOrganization.SelectedValue = positionRecord.orgID;

                    //var selectedOrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
                    cmbDepartment.ItemsSource = positionRecord.orgID.HasValue ? cd.GetDepartmentList(positionRecord.orgID.Value) : null;
                    cmbDepartment.DisplayMemberPath = "departmentName";
                    cmbDepartment.SelectedValuePath = "ID";
                    cmbDepartment.SelectedValue = positionRecord.deptID;

                    //var selectedDeptID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    cmbSection.ItemsSource = positionRecord.deptID.HasValue ? cd.GetSectionList(positionRecord.deptID.Value) : null;
                    cmbSection.DisplayMemberPath = "sectionName";
                    cmbSection.SelectedValuePath = "ID";
                    cmbSection.SelectedValue = positionRecord.sectID;

                    txtPosition.Text = positionRecord.positionName;

                    isActualPositon.IsChecked = positionRecord.isMain;
                    dtBeginDate.Text = positionRecord.beginDate.ToShortDateString();

                    positionEditor.Visibility = Visibility.Visible;




                }


            }


        }

        private position getPositionByID(int selectedPositionID)
        {
            return dbContext.position.Where(k => k.ID == selectedPositionID).FirstOrDefault();
        }

        private void CmbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
            cmbDepartment.ItemsSource = cd.GetDepartmentList(selectedOrgID);
            cmbDepartment.DisplayMemberPath = "departmentName";
            cmbDepartment.SelectedValuePath = "ID";

        }

        private void CmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDeptID = Convert.ToInt32(cmbDepartment.SelectedValue);
            cmbSection.ItemsSource = cd.GetSectionList(selectedDeptID);
            cmbSection.DisplayMemberPath = "sectionName";
            cmbSection.SelectedValuePath = "ID";

        }

        private void CmbSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddNewPosition_Click(object sender, RoutedEventArgs e)
        {
            positionEditor.Visibility = Visibility.Visible;
            selectedPosition = 0;
            cmbDepartment.SelectedIndex = cmbOrganization.SelectedIndex = cmbSection.SelectedIndex = -1 ;
            cmbOrganization.ItemsSource = null;
            cmbDepartment.ItemsSource = null;
            cmbSection.ItemsSource = null;
            txtPosition.Text = string.Empty;
            dtBeginDate.Text = DateTime.Now.ToShortDateString();


            cmbOrganization.ItemsSource = cd.GetOrganizationList();
            cmbOrganization.DisplayMemberPath = "organizationName";
            cmbOrganization.SelectedValuePath = "ID";
        

        }

        private void SaveNewPosition(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (selectedPosition != 0)
                {
                    position uPosition = dbContext.position.SingleOrDefault(k => k.ID == selectedPosition);
                    if (uPosition != null)
                    {
                        uPosition.orgID = Convert.ToInt32(cmbOrganization.SelectedValue);
                        uPosition.deptID = cmbDepartment.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbDepartment.SelectedValue);
                        uPosition.sectID = cmbSection.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbSection.SelectedValue);
                        uPosition.isMain = isActualPositon.IsChecked.Value;
                        uPosition.positionName = txtPosition.Text;
                        uPosition.beginDate = DateTime.Parse(dtBeginDate.Text);
                    }
                }
                else
                {
                    position pst = new position();

                    pst.orgID = Convert.ToInt32(cmbOrganization.SelectedValue);
                    pst.deptID = cmbDepartment.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbDepartment.SelectedValue);
                    pst.sectID = cmbSection.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbSection.SelectedValue);
                    pst.isMain = isActualPositon.IsChecked.Value;
                    pst.positionName = txtPosition.Text;
                    pst.beginDate = DateTime.Parse(dtBeginDate.Text);
                    pst.empID = selectedPersonID;

                    dbContext.position.Add(pst);
                }
                dbContext.SaveChanges();

                //  txtStatus.Text = rOrganization.organizationName + " has being added!";
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Problem yarandı" + ex.ToString());
            }
            finally
            {
                FillPositionList(selectedPersonID);
                MessageBox.Show("Melumatlar saxlanıldı");
            }


        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeletePosition(object sender, RoutedEventArgs e)
        {
            var matchedPosition = (from po in dbContext.position
                                     where po.ID == selectedPosition
                                     select po).SingleOrDefault();
            if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                e.Handled = true;
            }
            else
            {
                dbContext.Entry(matchedPosition).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();

                FillPositionList(selectedPersonID);

            }
        }
    }
}
