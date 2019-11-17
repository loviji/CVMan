using myAbdulKadr.Model;
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
    public partial class Home : UserControl
    {
        private static peopleEntities dbContext = new peopleEntities();

        public object BindingSource { get; private set; }

        public Home()
        {
            InitializeComponent();

            perList.DataContext = GetData(string.Empty, string.Empty, string.Empty);
        }

        private ObservableCollection<employee> GetData(string name, string surname, string midname)
        {
            List<employee> employeeList = null;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(midname))
            {
                employeeList = (from m in dbContext.employee
                                select m).ToList();
            }
            else
            {
                employeeList = (from m in dbContext.employee
                                where m.name.Contains(name) && m.surname.Contains(surname) && m.secondname.Contains(midname)
                                select m).ToList();

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



        private void PerList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            perList.DataContext = GetData(pName.Text, pSurname.Text, pMidName.Text);
        }
    }
}
