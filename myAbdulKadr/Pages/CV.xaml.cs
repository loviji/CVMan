using myAbdulKadr.ViewModel;
using PersonMotion.Common;
using PersonMotion.Model;
using PersonMotion.WF;
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
    public partial class CV : UserControl
    {
        private peopleEntities dbContext = DBContextResolver.Instance;



        public CV()
        {
            InitializeComponent();

            perList.DataContext = new EmployeeViewModel();
        }

        private ObservableCollection<EMP_LIST> GetData(string name, string surname, string midname)
        {
            List<EMP_LIST> employeeList = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(midname))
            {
                employeeList = new EmployeeViewModel().Employees.ToList();
            }
            else
            {
                employeeList = new EmployeeViewModel().Employees.Where(m => m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)).ToList();

            }
            //new { m.ID, m.name, m.surname, m.secondname, m.birthdate, m.birthplace });
            return new ObservableCollection<EMP_LIST>(employeeList);
            //return ObservableCollection<d;
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
                var selectedEmp = (EMP_LIST)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedEmp.ID > 0)
                {


                   
                    HRCard hc = new HRCard();
                 
                    hc.personID =  selectedEmp.ID.ToString();
                   
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
    }
}
