using FirstFloor.ModernUI.Windows;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PersonMotion.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalFamily.xaml
    /// </summary>
    public partial class personalFamily : UserControl, IContent
    {
        private int selectedPersonID { get; set; }


        private static peopleEntities dbContext = DBContextResolver.Instance;
        private static ControlData cd = new ControlData();
        public personalFamily()
        {
            InitializeComponent();

        }

        private void FillFamilyList(int selectedID)
        {
            familiaList.DataContext = (from e in dbContext.familia

                                           where e.empID == selectedPersonID
                                           select e).ToList();


            //dbContext.education.Where(v => v.empID == selectedID).ToList();
        }


        void IContent.OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

            selectedPersonID = Convert.ToInt32(e.Fragment);
            if (selectedPersonID != 0)
            {
                FillFamilyList(selectedPersonID);
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

        }
        private int selectedFamilyMember = 0;
        private void FamilyList_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (familiaList.SelectedCells.Count > 0)
            {

                Object selectedItem = ((DataGrid)sender).SelectedItem;
                if (selectedItem != null)
                {
                    Type type = selectedItem.GetType();
                    selectedFamilyMember = (int)type.GetProperty("ID").GetValue(selectedItem, null);
  

                    if (selectedFamilyMember > 0)
                    {
                        var familyMember = getFamilyMemberByID(selectedFamilyMember);
                        familyTypeName.Text = familyMember.familyTypeName;
                        familyMemberName.Text = familyMember.familyname;
                        familiMemberBirthYear.Text = familyMember.birthDate.ToString();
                        familiaEditor.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private familia getFamilyMemberByID(int selectedFamilyMemberID)
        {
            return dbContext.familia.Where(k => k.ID == selectedFamilyMemberID).FirstOrDefault();
        }



        private void AddNewEducation_Click(object sender, RoutedEventArgs e)
        {
            familiaEditor.Visibility = Visibility.Visible;
            selectedFamilyMember = 0;
            familyMemberName.Text = string.Empty;
            familyTypeName.Text = string.Empty;
            familiMemberBirthYear.Text = DateTime.Now.ToShortDateString();
            
        }

        private void SaveNewFamilia(object sender, RoutedEventArgs e)
        {

            try
            {
                if (selectedFamilyMember != 0)
                {
                    familia uFamilia = dbContext.familia.SingleOrDefault(k => k.ID == selectedFamilyMember);
                    if (uFamilia != null)
                    {

                        uFamilia.familyTypeName = familyTypeName.Text;
                        uFamilia.familyname = familyMemberName.Text;
                        uFamilia.birthDate = DateTime.Parse(familiMemberBirthYear.Text);
                       
                    }
                }
                else
                {
                    familia fml = new familia();

                    fml.empID = selectedPersonID;
                    fml.familyTypeName = familyTypeName.Text;
                    fml.familyname = familyMemberName.Text;
                    fml.birthDate = DateTime.Parse(familiMemberBirthYear.Text);
                   

                    dbContext.familia.Add(fml);
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
                FillFamilyList(selectedPersonID);
                MessageBox.Show("Melumatlar saxlanıldı");
            }


        }

     

        private void DeleteFamilia(object sender, RoutedEventArgs e)
        {
            var matchedFamilyMember = (from ed in dbContext.familia
                                      where ed.ID == selectedFamilyMember
                                      select ed).SingleOrDefault();
            if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                e.Handled = true;
            }
            else
            {
                dbContext.Entry(matchedFamilyMember).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();

                FillFamilyList(selectedPersonID);

            }
        }

        
    }
}
