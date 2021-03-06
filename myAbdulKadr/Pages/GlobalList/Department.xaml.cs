﻿using PersonMotion.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using System;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using PersonMotion.Common;

namespace myAbdulKadr.Pages.GlobalList
{
    /// <summary>
    /// Interaction logic for Department.xaml
    /// </summary>
    public partial class Department : UserControl, IContent, INotifyPropertyChanged
    {

        private peopleEntities dbContext = DBContextResolver.Instance;
        
        public Department()
        {
            InitializeComponent();
        }

        private ObservableCollection<organization> GetOrganizationList()
        {
            var list = from e in dbContext.organization where e.isdeleted==false select e;
            return new ObservableCollection<organization>(list.ToList());
        }

        private List<organization> GetOrganizationList2()
        {
            var list = from e in dbContext.organization where e.isdeleted==false select e;
            return list.ToList();
        }


        private ObservableCollection<department> GetDepartmentList(int orgID)
        {
            var list = from dp in dbContext.department
                       where dp.organizationID == orgID && dp.isdeleted == false
                        select dp;
            return new ObservableCollection<department>(list);
        }


        private void dgDept_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                department dept = e.Row.DataContext as department;

                var matchedData = (from dp in dbContext.department
                                   where dp.ID == dept.ID
                                   select dp).SingleOrDefault();
                if (matchedData == null)
                {
                    department rDepartment = new department();
                    rDepartment.departmentName = dept.departmentName;
                    rDepartment.organizationID = selectedOrgID;
                    dbContext.department.Add(rDepartment);
                    dbContext.SaveChanges();
                    dgDept.ItemsSource = GetDepartmentList(selectedOrgID);
                    txtStatus.Text = rDepartment.departmentName + " has being added!";

                }
                else
                {
                    matchedData.departmentName = dept.departmentName;
                    dbContext.SaveChanges();
                    dgDept.ItemsSource = GetDepartmentList(selectedOrgID);
                    txtStatus.Text = "Success. Info updated";
                }
            }

        }
   
        private void DgDept_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            department dDept = dgDept.SelectedItem as department;

            if (dDept != null)
            {
                var matchedDepartment= (from o in dbContext.department
                                           where o.ID ==dDept.ID 
                                           select o).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {

                        matchedDepartment.isdeleted = true;
                        dbContext.SaveChanges();
                        txtStatus.Text = "Success. Info deleted";
                  


                    }
                }
            }
        }

        private int selectedOrgID = 0;

        private ObservableCollection<organization> orgList;

        public ObservableCollection<organization> OrgList
        {
            get
            {
                return orgList;
            }
            set
            {
                if (value != orgList)
                {
                    orgList = value;
                    NotifyPropertyChanged("OrgList"); // method implemented below
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CmbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrgID = Convert.ToInt32(cmbOrganization.SelectedValue);
            dgDept.ItemsSource = GetDepartmentList(selectedOrgID);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
      
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
           
            cmbOrganization.Items.Clear();
            //OrgList = GetOrganizationList();
            
            cmbOrganization.ItemsSource = GetOrganizationList2();
            cmbOrganization.DisplayMemberPath = "organizationName";
            cmbOrganization.SelectedValuePath = "ID";
            dgDept.ItemsSource = GetDepartmentList(selectedOrgID);
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            cmbOrganization.ItemsSource = null;
            dgDept.ItemsSource = null;
        }
    }
}
