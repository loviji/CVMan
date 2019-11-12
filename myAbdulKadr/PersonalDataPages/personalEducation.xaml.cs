using FirstFloor.ModernUI.Windows;
using myAbdulKadr.Common;
using myAbdulKadr.Model;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace hydrogen.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalPosition.xaml
    /// </summary>
    /// 


    public partial class personalEducation : UserControl, IContent
    {
        private int selectedPersonID { get; set; }


        private static peopleEntities dbContext = new peopleEntities();
        private static ControlData cd = new ControlData();
        public personalEducation()
        {
            InitializeComponent();

        }

        private void FillEducationList(int selectedID)
        {
            educationList.DataContext = (from e in dbContext.education
                                         join m in dbContext.metaData
                                               on new { ID = (int)e.educationType, code = "edtp" }
                                           equals new { m.ID, m.code }
                                         join mg in dbContext.metaData
                                               on new { ID = (int)e.grade, code = "edgd" }
                                           equals new { mg.ID, mg.code }
                                         where e.empID == selectedPersonID
                                         select new
                                         {
                                             e.ID,
                                             e.empID,
                                             educationTypeName = m.value,
                                             gradeName = mg.value,
                                             e.educationCentreName,
                                             e.faculty,
                                             e.speciality,
                                             e.endYear,
                                             e.diplomNumber
                                         }).ToList();


            //dbContext.education.Where(v => v.empID == selectedID).ToList();
        }


        void IContent.OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

            selectedPersonID = Convert.ToInt32(e.Fragment);
            if (selectedPersonID != 0)
            {
                FillEducationList(selectedPersonID);
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
        private int selectedEducation = 0;
        private void EducationList_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (educationList.SelectedCells.Count > 0)
            {

                Object selectedItem = ((DataGrid)sender).SelectedItem;
                if (selectedItem != null)
                {
                    Type type = selectedItem.GetType();
                    selectedEducation = (int)type.GetProperty("ID").GetValue(selectedItem, null);
                    //var selectedGridEducation =cellInfo.Column.GetCellContent(cellInfo.Item).DataContext
                    //                            select new { ID=m.id};

                    if (selectedEducation > 0)
                    {

                        var educationRecord = getEducationByID(selectedEducation);

                        cmbEducationType.ItemsSource = cd.GetMetaDataByType("edtp");
                        cmbEducationType.DisplayMemberPath = "value";
                        cmbEducationType.SelectedValuePath = "ID";
                        cmbEducationType.SelectedValue = educationRecord.educationType;

                        cmbEducationGrade.ItemsSource = cd.GetMetaDataByType("edgd");
                        cmbEducationGrade.DisplayMemberPath = "value";
                        cmbEducationGrade.SelectedValuePath = "ID";
                        cmbEducationGrade.SelectedValue = educationRecord.grade;

                        ceducationCentreName.Text = educationRecord.educationCentreName;
                        faculty.Text = educationRecord.faculty;
                        speciality.Text = educationRecord.speciality;
                        endYear.Text = educationRecord.endYear.HasValue ? educationRecord.endYear.Value.ToString() : string.Empty;
                        diplomNumber.Text = educationRecord.diplomNumber;

                        educationEditor.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private education getEducationByID(int selectedEducationID)
        {
            return dbContext.education.Where(k => k.ID == selectedEducationID).FirstOrDefault();
        }

        //private void CmbEducationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedOrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
        //    cmbDepartment.ItemsSource = cd.GetDepartmentList(selectedOrgID);
        //    cmbDepartment.DisplayMemberPath = "departmentName";
        //    cmbDepartment.SelectedValuePath = "ID";

        //}

        //private void CmbEducationGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedDeptID = Convert.ToInt32(cmbDepartment.SelectedValue);
        //    cmbSection.ItemsSource = cd.GetSectionList(selectedDeptID);
        //    cmbSection.DisplayMemberPath = "sectionName";
        //    cmbSection.SelectedValuePath = "ID";

        //}


        private void AddNewEducation_Click(object sender, RoutedEventArgs e)
        {
            educationEditor.Visibility = Visibility.Visible;
            selectedEducation = 0;
            cmbEducationGrade.SelectedIndex = cmbEducationType.SelectedIndex = -1;
            cmbEducationGrade.ItemsSource = null;
            cmbEducationType.ItemsSource = null;
            ceducationCentreName.Text = string.Empty;
            faculty.Text = string.Empty;
            endYear.Text = string.Empty;
            diplomNumber.Text = string.Empty;
            cmbEducationGrade.ItemsSource = cd.GetMetaDataByType("edgd");
            cmbEducationGrade.DisplayMemberPath = "value";
            cmbEducationGrade.SelectedValuePath = "ID";
            cmbEducationType.ItemsSource = cd.GetMetaDataByType("edtp");
            cmbEducationType.DisplayMemberPath = "value";
            cmbEducationType.SelectedValuePath = "ID";
        }

        private void SaveNewPosition(object sender, RoutedEventArgs e)
        {

            try
            {
                if (selectedEducation != 0)
                {
                    education uEducation = dbContext.education.SingleOrDefault(k => k.ID == selectedEducation);
                    if (uEducation != null)
                    {
                        uEducation.grade = cmbEducationGrade.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbEducationGrade.SelectedValue);
                        uEducation.educationType = cmbEducationType.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbEducationType.SelectedValue);
                        uEducation.educationCentreName = ceducationCentreName.Text;
                        uEducation.faculty = faculty.Text;

                        uEducation.speciality = speciality.Text;
                        uEducation.diplomNumber = diplomNumber.Text;
                        uEducation.endYear = Convert.ToInt32(endYear.Text);
                    }
                }
                else
                {
                    education edu = new education();
                    edu.grade = cmbEducationGrade.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbEducationGrade.SelectedValue);
                    edu.educationType = cmbEducationType.SelectedValue == null ? (int?)null : Convert.ToInt32(cmbEducationType.SelectedValue);
                    edu.educationCentreName = ceducationCentreName.Text;
                    edu.faculty = faculty.Text;
                    edu.empID = selectedPersonID;
                    edu.speciality = speciality.Text;
                    edu.diplomNumber = diplomNumber.Text;
                    edu.endYear = Convert.ToInt32(endYear.Text);

                    dbContext.education.Add(edu);
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
            var matchedEducation = (from ed in dbContext.education
                                    where ed.ID == selectedEducation
                                    select ed).SingleOrDefault();
            if (!(MessageBox.Show("Are you sure?", "Confirm Delete!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                e.Handled = true;
            }
            else
            {
                dbContext.Entry(matchedEducation).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();

                FillEducationList(selectedPersonID);

            }
        }

        private void EndYear_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            //int min = 1950;
            //int max = DateTime.Now.Year;
            //bool isValid = false;
            //if (e.Text.Length>min || e.Text.Length < max || Regex.IsMatch(e.Text, "[^0-9]+"))
            //    isValid = true;

            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
    }
}
