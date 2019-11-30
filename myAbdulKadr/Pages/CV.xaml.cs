using myAbdulKadr.ViewModel;
using PersonMotion.Common;
using PersonMotion.Model;
using PersonMotion.WF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace myAbdulKadr.Pages
{
    /// <summary>
    /// Interaction logic for CV.xaml
    /// </summary>
    public partial class CV : UserControl
    {
        private peopleEntities dbContext = DBContextResolver.Instance;



        public CV()
        {
            InitializeComponent();

           
        }

        private ObservableCollection<employee> GetData(string name, string surname, string midname)
        {
            List<employee> employeeList = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(midname))
            {
                employeeList = new EmployeeViewModel().Employees.ToList();
            }
            else
            {
                employeeList = new EmployeeViewModel().Employees.Where(m => m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)).ToList();

            }
            //new { m.ID, m.name, m.surname, m.secondname, m.birthdate, m.birthplace });
            return new ObservableCollection<employee>(employeeList);
            //return ObservableCollection<d;
        }


        private void AddNewPersonal_Click(object sender, RoutedEventArgs e)
        {
            string url = "../PersonalDataPages/PersonDataEditor.xaml#0";
            NavigationCommands.GoToPage.Execute(url, this);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            perList.ItemsSource = GetData(pName.Text, pSurname.Text, pMidName.Text);
        }

        private void PerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (perList.SelectedCells.Count > 0)
            {
                var cellInfo = perList.SelectedCells[0];
                var selectedEmp = (employee)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedEmp.ID > 0)
                {


                    // string url = "../PersonalDataPages/PersonDataEditor.xaml#" + selectedEmp.ID.ToString();
                    // NavigationCommands.GoToPage.Execute(url, this);
                    HRCard hc = new HRCard();
                   // WindowInteropHelper wih = new WindowInteropHelper();
                    hc.personID = "18470";// selectedEmp.ID.ToString();
                    //hc.WindowState = System.Windows.Forms.WindowState.Maximized;
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
       
            perList.DataContext = new EmployeeViewModel();

       
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}
