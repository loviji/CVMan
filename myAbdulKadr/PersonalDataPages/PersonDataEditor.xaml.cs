using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;


namespace hydrogen
{

    public partial class PersonDataEditor : UserControl, IContent
    {
        private string selectedID { get; set; }

        public LinkCollection IPList { get; private set; }

        public void FillList(LinkCollection IPList)
        {
            //burda elə etmək lazımdır ki, hər dəfə təzə userə click olunanda yenilənsin
            if (IPList.Count() == 0)
            {
                papulateList(IPList);
            }
            else
            {
                IPList.Clear();
                papulateList(IPList);
            }
        }

        private void papulateList(LinkCollection IPList)
        {
            IPList.Add(new Link() { DisplayName = "Şəxsi məlumatlar", Source = new Uri("/PersonalDataPages/personalData.xaml#" + selectedID, UriKind.Relative) });
            IPList.Add(new Link() { DisplayName = "Vəzifə", Source = new Uri("/PersonalDataPages/personalPosition.xaml#" + selectedID, UriKind.Relative) });
            IPList.Add(new Link() { DisplayName = "Təhsil", Source = new Uri("/PersonalDataPages/personalEducation.xaml#" + selectedID, UriKind.Relative) });
            IPList.Add(new Link() { DisplayName = "Əmək fəaliyyəti", Source = new Uri("/PersonalDataPages/personalWorkHistory.xaml#" + selectedID, UriKind.Relative) });
            IPList.Add(new Link() { DisplayName = "Digər məlumatlar", Source = new Uri("/PersonalDataPages/personalRegData.xaml#" + selectedID, UriKind.Relative) });
            IPList.Add(new Link() { DisplayName = "Ailə tərkibi", Source = new Uri("/PersonalDataPages/personalFamily.xaml#" + selectedID, UriKind.Relative) });
        }

        public PersonDataEditor()
        {
            DataContext = this;
            IPList = new LinkCollection();
            //InitializeComponent();
            InitializeComponent();
        }
        public static readonly DependencyProperty SetTextProperty =
         DependencyProperty.Register("SetText", typeof(string), typeof(PersonDataEditor), new
            PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));

        private static void OnSetTextChanged(DependencyObject d,
         DependencyPropertyChangedEventArgs e)
        {
            PersonDataEditor UserControl1Control = d as PersonDataEditor;
            UserControl1Control.OnSetTextChanged(e);
        }


        public string SetText
        {
            get { return (string)GetValue(SetTextProperty); }
            set { SetValue(SetTextProperty, value); }
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var s = e.NewValue.ToString();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            selectedID = e.Fragment;
            FillList(IPList);
            PersonalOperations.SelectedSource = new Uri("/PersonalDataPages/personalData.xaml#" + selectedID, UriKind.Relative);
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Title = mainWindowHeader;

        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            // throw new NotImplementedException();
        }
        private const string mainWindowHeader = "Person motion";
        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (ModernDialog.ShowMessage("Səhifəni tərk edirsiniz?", "navigate", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;


            }
        }
    }
}

