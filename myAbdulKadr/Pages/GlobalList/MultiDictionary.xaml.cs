using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using PersonMotion.Common;
using PersonMotion.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PersonMotion.Pages.GlobalList
{
    /// <summary>
    /// Interaction logic for MultiDictionary.xaml
    /// </summary>
    public partial class MultiDictionary : UserControl, IContent, INotifyPropertyChanged
    {

        private peopleEntities dbContext = DBContextResolver.Instance;

        public MultiDictionary()
        {
            InitializeComponent();
        }

        private SortedDictionary<string, string> GetMetaTypes()
        {
            SortedDictionary<string, string> MDCache = new SortedDictionary<string, string>
{
 
  {"edtp", "Təhsil növləri"},
   {"edgd", "Təhsil dərəcələri"},
                {"natgl","Milliyətlər" },
                {"pltpr", "Siyasi partiyalar" }

};

            return MDCache;
        }


        private ObservableCollection<metaData> GetMetaDataList(string metaCode)
        {
            var list = from dp in dbContext.metaData
                       where dp.code == metaCode
                       select dp;
            return new ObservableCollection<metaData>(list);
        }


        private void DgMD_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                metaData dept = e.Row.DataContext as metaData;

                var matchedData = (from dp in dbContext.metaData
                                   where dp.ID == dept.ID
                                   select dp).SingleOrDefault();
                if (matchedData == null)
                {
                    metaData rMetaData = new metaData();
                    rMetaData.value = dept.value;
                    rMetaData.code = selectedMDictCode;
                    //rDepartment.organizationID = selectedMDictCode;
                    dbContext.metaData.Add(rMetaData);
                    dbContext.SaveChanges();
                    dgMetaData.ItemsSource = GetMetaDataList(selectedMDictCode);
                    txtStatus.Text = rMetaData.value + " has being added!";

                }
                else
                {
                    matchedData.value = dept.value;
                    dbContext.SaveChanges();
                    dgMetaData.ItemsSource = GetMetaDataList(selectedMDictCode);
                    txtStatus.Text = "Success. Info updated";
                }
            }

        }

        private void DgMD_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            metaData dMetaData = dgMetaData.SelectedItem as metaData;

            if (dMetaData != null)
            {
                var matchedMetaData = (from o in dbContext.metaData
                                         where o.ID == dMetaData.ID
                                         select o).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Əminsiniz mi?", "Məlumat silinməsini təsdiqləyin!", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        dbContext.Entry(matchedMetaData).State = System.Data.Entity.EntityState.Deleted;
                        dbContext.SaveChanges();


                    }
                }
            }
        }

        private string selectedMDictCode = string.Empty;

        //private ObservableCollection<organization> orgList;

        //public ObservableCollection<organization> OrgList
        //{
        //    get
        //    {
        //        return orgList;
        //    }
        //    set
        //    {
        //        if (value != orgList)
        //        {
        //            orgList = value;
        //            NotifyPropertyChanged("OrgList"); // method implemented below
        //        }
        //    }
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CmbMDict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMDictCode = cmbMDict.SelectedValue.ToString();
            dgMetaData.ItemsSource = GetMetaDataList(selectedMDictCode);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {

        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {

            cmbMDict.Items.Clear();
            //OrgList = GetOrganizationList();

            cmbMDict.ItemsSource = GetMetaTypes();
            cmbMDict.DisplayMemberPath = "Value";
            cmbMDict.SelectedValuePath = "Key";
            dgMetaData.ItemsSource = GetMetaDataList(selectedMDictCode);
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            cmbMDict.ItemsSource = null;
            dgMetaData.ItemsSource = null;
        }


    }
}

