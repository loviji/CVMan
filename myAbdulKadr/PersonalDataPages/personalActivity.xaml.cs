using FirstFloor.ModernUI.Windows;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PersonMotion.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalActivity.xaml
    /// </summary>
    public partial class personalActivity : UserControl, IContent
    {

        private static peopleEntities dbContext = DBContextResolver.Instance;
        private static ControlData cd = new ControlData();
        public personalActivity()
        {
            InitializeComponent();
        }


        private int selectedPersonID { get; set; }
        private char radioActivityTypeSelected { get; set; }

        private void FillActivityList(int selectedID)
        {
            activityList.DataContext = (from e in dbContext.employeeActivity

                                        where e.empID == selectedPersonID
                                        select e).ToList();
        }


        void IContent.OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            selectedPersonID = Convert.ToInt32(e.Fragment);

            if (GlobalCache.currentPersonID != 0)
            {
                selectedPersonID = GlobalCache.currentPersonID;
                FillActivityList(selectedPersonID);
            }

        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;

            radioActivityTypeSelected = radioButton.Content.ToString()[0];
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
        private int selectedActivity = 0;
        private void ActivityList_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (activityList.SelectedCells.Count > 0)
            {

                Object selectedItem = ((DataGrid)sender).SelectedItem;
                if (selectedItem != null)
                {
                    Type type = selectedItem.GetType();
                    selectedActivity = (int)type.GetProperty("ID").GetValue(selectedItem, null);

                    if (selectedActivity > 0)
                    {
                        var activityRecord = GetActivityByID(selectedActivity);

                        TextRange textRange = new TextRange(activityText.Document.ContentStart, activityText.Document.ContentEnd);
                        textRange.Text = activityRecord.activityText;
                        // activityText. Text = activityRecord.activityText;

                        issueDate.Text = activityRecord.issueDate.HasValue ? activityRecord.issueDate.Value.ToShortDateString() : string.Empty;

                        activityEditor.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private employeeActivity GetActivityByID(int selectedActivityID)
        {
            return dbContext.employeeActivity.Where(k => k.ID == selectedActivityID).FirstOrDefault();
        }


        private void AddNewEducation_Click(object sender, RoutedEventArgs e)
        {
            activityEditor.Visibility = Visibility.Visible;
            selectedActivity = 0;
            TextRange textRange = new TextRange(activityText.Document.ContentStart, activityText.Document.ContentEnd);
            activityText.Document.Blocks.Clear();
            // positionName.Text = string.Empty;
            issueDate.Text = DateTime.Now.ToShortDateString();

        }

        private void SaveNewWorkHistory(object sender, RoutedEventArgs e)
        {

            try
            {
                TextRange textRange = new TextRange(activityText.Document.ContentStart, activityText.Document.ContentEnd);
                if (selectedActivity != 0)
                {
                    employeeActivity uActivity = dbContext.employeeActivity.SingleOrDefault(k => k.ID == selectedActivity);
                    if (uActivity != null)
                    {

                        uActivity.activityText = textRange.Text;
                        uActivity.activityType = radioActivityTypeSelected.ToString();
                        uActivity.issueDate = !string.IsNullOrEmpty(issueDate.Text) ? DateTime.Parse(issueDate.Text) : (DateTime?)null;
                    }
                }
                else
                {
                    employeeActivity wkh = new employeeActivity();

                    wkh.empID = selectedPersonID;
                    wkh.activityType = radioActivityTypeSelected.ToString();
                    wkh.activityText = textRange.Text;
                    wkh.issueDate = !string.IsNullOrEmpty(issueDate.Text) ? DateTime.Parse(issueDate.Text) : (DateTime?)null;


                    dbContext.employeeActivity.Add(wkh);
                }
                dbContext.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Problem yarandı" + ex.ToString());
            }
            finally
            {
                FillActivityList(selectedPersonID);
                MessageBox.Show("Melumatlar saxlanıldı");
            }


        }

        
        private void DeletePosition(object sender, RoutedEventArgs e)
        {
            var matchedActivity = (from ed in dbContext.employeeActivity
                                   where ed.ID == selectedActivity
                                   select ed).SingleOrDefault();
            if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                e.Handled = true;
            }
            else
            {
                dbContext.Entry(matchedActivity).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();

                FillActivityList(selectedPersonID);

            }
        }
    }
}
