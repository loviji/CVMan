using System.Windows;
using System.Windows.Controls;

namespace hydrogen.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for UserControlPersonID.xaml
    /// </summary>
    public partial class UserControlPersonID : UserControl
    {
        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(UserControlPersonID), new UIPropertyMetadata(null));

        



        public UserControlPersonID()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
