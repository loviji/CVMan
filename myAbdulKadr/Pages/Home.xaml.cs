using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using myAbdulKadr.Model;
using myAbdulKadr.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace hydrogen.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl, IContent
    {
        private  peopleEntities dbContext = new peopleEntities();

      

        public Home()
        {
            InitializeComponent();

            //GetData(string.Empty, string.Empty, string.Empty);
        }

        private ObservableCollection<employee> GetData(string name, string surname, string midname)
        {
            List<employee> employeeList = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(midname))
            {
               // employeeList = new EmployeeViewModel();
            }
            else
            {
                //employeeList = (from m in dbContext.employee
                //                where m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)
                //                select m).ToList();

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
            perList.DataContext = GetData(pName.Text, pSurname.Text, pMidName.Text);
        }

        private void PerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (perList.SelectedCells.Count > 0)
            {
                var cellInfo = perList.SelectedCells[0];
                var selectedEmp = (employee)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedEmp.ID > 0)
                {


                    string url = "../PersonalDataPages/PersonDataEditor.xaml#" + selectedEmp.ID.ToString();
                    NavigationCommands.GoToPage.Execute(url, this);

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
            // perList.ItemsSource = null;
            // perList.ItemsSource = GetData(string.Empty, string.Empty, string.Empty);
            //perList.Items.Refresh();
            // var d = new EmployeeViewModel();
            perList.DataContext = new EmployeeViewModel();

            //perList.ItemsSource = null;

   //         perList.ItemsSource = new EmployeeViewModel().Employees;//Preferably do this somewhere else, not in the add method.
            //perList.Items.Refresh();

        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
          
        }
    }
}
