using FirstFloor.ModernUI.Windows;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PersonMotion.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalWorkHistory.xaml
    /// </summary>
    public partial class personalWorkHistory : UserControl, IContent
    {
        private int selectedPersonID { get; set; }


        private static peopleEntities dbContext = DBContextResolver.Instance;
        private static ControlData cd = new ControlData();
        public personalWorkHistory()
        {
            InitializeComponent();

        }

        private void FillEducationList(int selectedID)
        {
            workHistoryList.DataContext = (from e in dbContext.workhistory
                                        
                                         where e.empID == selectedPersonID
                                         select e).ToList();


            //dbContext.education.Where(v => v.empID == selectedID).ToList();
        }


        void IContent.OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            selectedPersonID = Convert.ToInt32(e.Fragment);

            if (GlobalCache.currentPersonID != 0)
            {
                selectedPersonID = GlobalCache.currentPersonID;
                FillEducationList(selectedPersonID);
            }



            //selectedPersonID = Convert.ToInt32(e.Fragment);
            //if (selectedPersonID != 0)
            //{
            //    FillEducationList(selectedPersonID);
            //}



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
            
        }
        private int selectedWorkHistory = 0;
        private void WorkHistoryList_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (workHistoryList.SelectedCells.Count > 0)
            {

                Object selectedItem = ((DataGrid)sender).SelectedItem;
                if (selectedItem != null)
                {
                    Type type = selectedItem.GetType();
                    selectedWorkHistory = (int)type.GetProperty("ID").GetValue(selectedItem, null);

                    if (selectedWorkHistory > 0)
                    {
                        var workHistoryRecord = getWorkHistoryByID(selectedWorkHistory);
                        cstructureName.Text = workHistoryRecord.structureName;
                        positionName.Text = workHistoryRecord.positionName;
                        workBeginDate.Text = workHistoryRecord.beginDate.ToShortDateString();
                        workEndDate.Text = workHistoryRecord.endDate.HasValue?workHistoryRecord.endDate.Value.ToShortDateString():string.Empty;
                        workHistoryEditor.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private workhistory getWorkHistoryByID(int selectedWorkHistoryID)
        {
            return dbContext.workhistory.Where(k => k.ID == selectedWorkHistoryID).FirstOrDefault();
        }

     

        private void AddNewEducation_Click(object sender, RoutedEventArgs e)
        {
            workHistoryEditor.Visibility = Visibility.Visible;
            selectedWorkHistory = 0;
            cstructureName.Text = string.Empty;
            positionName.Text = string.Empty;
            workBeginDate.Text= DateTime.Now.ToShortDateString();
            workEndDate.Text = null;
        }

        private void SaveNewWorkHistory(object sender, RoutedEventArgs e)
        {

            try
            {
                if (selectedWorkHistory != 0)
                {
                    workhistory uWorkHistory = dbContext.workhistory.SingleOrDefault(k => k.ID == selectedWorkHistory);
                    if (uWorkHistory != null)
                    {
                       
                        uWorkHistory.structureName = cstructureName.Text.Substring(0, cstructureName.Text.Length > 500 ? 500 : cstructureName.Text.Length);
                        uWorkHistory.positionName = positionName.Text.Substring(0, positionName.Text.Length > 500 ? 500 : positionName.Text.Length);
                        uWorkHistory.beginDate = DateTime.Parse(workBeginDate.Text);
                        uWorkHistory.endDate = !string.IsNullOrEmpty(workEndDate.Text) ? DateTime.Parse(workEndDate.Text) : (DateTime?)null;
                    }
                }
                else
                {
                    workhistory wkh = new workhistory();
             
                    wkh.empID = selectedPersonID;
                    wkh.structureName = cstructureName.Text.Substring(0, cstructureName.Text.Length > 500 ? 500 : cstructureName.Text.Length);
                    wkh.positionName = positionName.Text.Substring(0, positionName.Text.Length > 500 ? 500 : positionName.Text.Length);
                    wkh.beginDate = DateTime.Parse(workBeginDate.Text);
                    wkh.endDate =  !string.IsNullOrEmpty(workEndDate.Text)?DateTime.Parse(workEndDate.Text): (DateTime?)null;

                    dbContext.workhistory.Add(wkh);
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
                FillEducationList(selectedPersonID);
                MessageBox.Show("Melumatlar saxlanıldı");
            }


        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeletePosition(object sender, RoutedEventArgs e)
        {
            var matchedWorkHistory = (from ed in dbContext.workhistory
                                    where ed.ID == selectedWorkHistory
                                    select ed).SingleOrDefault();
            if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                e.Handled = true;
            }
            else
            {
                dbContext.Entry(matchedWorkHistory).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();

                FillEducationList(selectedPersonID);

            }
        }

    }
}
