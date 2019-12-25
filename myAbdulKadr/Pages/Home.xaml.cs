﻿using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using PersonMotion.Common;
using PersonMotion.Model;
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
        private peopleEntities dbContext = DBContextResolver.Instance;



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
                employeeList = new EmployeeViewModel().Employees.ToList();
            }
            else
            {
                employeeList = new EmployeeViewModel().Employees.Where(m => m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)).ToList();
                //where m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)
                //select m).ToList();

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

        private const string mainWindowHeader = "Person motion";
        private string personFullName = string.Empty;
        private void PerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (perList.SelectedCells.Count > 0)
            {
                var cellInfo = perList.SelectedCells[0];
                var selectedEmp = (employee)(cellInfo.Column.GetCellContent(cellInfo.Item).DataContext);

                if (selectedEmp.ID > 0)
                {

                    
                    string url = "../PersonalDataPages/PersonDataEditor.xaml#" + selectedEmp.ID.ToString();
                    Window mainWindow = Application.Current.MainWindow;
                    personFullName = dbContext.employee.Where(s => s.ID == selectedEmp.ID).Select(s=>s.name+" "+s.surname+" "+s.secondname).SingleOrDefault();
                    mainWindow.Title = mainWindowHeader + " "+personFullName;
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
