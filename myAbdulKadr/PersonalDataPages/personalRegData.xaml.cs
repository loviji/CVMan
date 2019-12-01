using FirstFloor.ModernUI.Windows;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace myAbdulKadr.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalRegData.xaml
    /// </summary>
    public partial class personalRegData : UserControl, IContent
    {
        public personalRegData()
        {
            InitializeComponent();
        }
        private static peopleEntities dbContext = DBContextResolver.Instance;
        private int selectedPersonID { get; set; }

        private void SaveNewPosition(object sender, RoutedEventArgs e)
        {
            try
            {

                address uAddress = dbContext.address.SingleOrDefault(k => k.empID == selectedPersonID);
                employee uEmployee = dbContext.employee.SingleOrDefault(k => k.ID == selectedPersonID);
                if (uAddress != null)
                {

                    uAddress.countryName = RegCountry.Text;
                    uAddress.cityName = RegDistrict.Text;
                    uAddress.streetName = RegAddr.Text;
                    uAddress.empID = selectedPersonID;

                }

                else
                {
                    address addr = new address();

                    addr.countryName = RegCountry.Text;
                    addr.cityName = RegDistrict.Text;
                    addr.streetName = RegAddr.Text;
                    addr.empID = selectedPersonID;
                    dbContext.address.Add(addr);
                }
                uEmployee.salary = Convert.ToDecimal(salary.Text);
                uEmployee.FINCODE = pincode.Text;
                uEmployee.workPhone = phone.Text;
                uEmployee.mobilePhone = mobilePhone.Text;
                uEmployee.emailaddr = email.Text;
                dbContext.SaveChanges();

                //  txtStatus.Text = rOrganization.organizationName + " has being added!";
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Problem yarandı" + ex.ToString());
            }
            finally
            {
                
                MessageBox.Show("Melumatlar saxlanıldı");
            }

        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {



            selectedPersonID = Convert.ToInt32(e.Fragment);
            if (selectedPersonID != 0)
            {
                populateFormControls(selectedPersonID);
            }
        }

        private void populateFormControls(int selectedPersonID)
        {
            var addr = dbContext.address.Where(d => d.empID == selectedPersonID).SingleOrDefault();
            if (addr != null)
            {
                RegCountry.Text = addr.countryName;
                RegDistrict.Text = addr.cityName;
                RegAddr.Text = addr.streetName;
                
            }
            var emp = dbContext.employee.SingleOrDefault(s => s.ID == selectedPersonID);
            if (emp != null)
            {
                pincode.Text = emp.FINCODE;
                salary.Text = emp.salary.ToString();
                email.Text = emp.emailaddr;
                phone.Text = emp.workPhone;
                mobilePhone.Text = emp.mobilePhone;
            }
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {

        }
    }
}
