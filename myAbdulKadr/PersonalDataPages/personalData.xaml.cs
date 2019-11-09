using FirstFloor.ModernUI.Windows;
using hydrogen.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace hydrogen.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for PersonalData.xaml
    /// </summary>
    public partial class PersonalData : UserControl, IContent
    {
        private static peopleEntities dbContext = new peopleEntities();
        private char radioSexSelected;
        private string radioFamilyStatusSelected;
        private const int minimumTextLength = 3;
        private int insertedPhotoID = 1;
        private int selectedPersonID { get; set; }


        public PersonalData()
        {
            InitializeComponent();
            fillBasicControls();


        }

        private void fillBasicControls()
        {
            personalName.Text = "xzz";

            GetBitmapImage(insertedPhotoID);


            personalNationality.ItemsSource = FillMetaCombobox("natgl").ToList();
            personalNationality.DisplayMemberPath = "value";
            personalNationality.SelectedValuePath = "ID";

            personalPoliticalParty.ItemsSource = FillMetaCombobox("pltpr").ToList();
            personalPoliticalParty.DisplayMemberPath = "value";
            personalPoliticalParty.SelectedValuePath = "ID";
        }

        private employee getPersonData(int selectedPersonID)
        {
            return dbContext.employee.Where(k => k.ID == selectedPersonID).FirstOrDefault();
        }

        private void GetBitmapImage(int imageID)
        {

            var blob = dbContext.files.Where(z => z.ID == imageID).FirstOrDefault().img;

            //Store binary data read from the database in a byte array

            MemoryStream stream = new MemoryStream();
            stream.Write(blob, 0, blob.Length);
            stream.Position = 0;

            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            personalPhoto.Source = bi;

            insertedPhotoID = imageID;
        }

        private IQueryable<metaData> FillMetaCombobox(string metaType)
        {
            return dbContext.metaData.Where(r => r.code == metaType).OrderBy(f => f.value);
        }

        private byte getFamilyStatusID(string k)
        {

            return Convert.ToByte(dbContext.metaData.Where(r => r.code == "fmlst"
            && r.value.StartsWith(k)).FirstOrDefault().ID);
        }

        //private Dictionary<int, string> GetFamilyStatusByID(int familyStatusID)
        //{

        //    Dictionary<int, metaData> d= dbContext.metaData.Where(r => r.code == "fmlst" && r.ID == familyStatusID).ToList().ToDictionary(p => p.ID);
        //    return d.Where(s => s.Key == familyStatusID).First();

        //}

        private int getNationalityStatusID(string k)
        {
            return Convert.ToByte(dbContext.metaData.Where(r => r.code == ""
            && r.value == "").FirstOrDefault().ID);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var formIsValid = ValidateForm();
            if (!formIsValid)
            {
                MessageBox.Show("Please fill all form elements!");
                return;
            }

            createOREditPersonal();
        }

        private void createOREditPersonal()
        {
            //MessageBox.Show(personalNationality.SelectedItem.ToString(),.ToString());
            try
            {
                if (selectedPersonID == 0)
                {
                    var newEmployee = new employee()
                    {
                        name = personalName.Text,
                        surname = personalSurname.Text,
                        secondname = personalPatroynmic.Text,
                        sex = radioSexSelected.ToString(),
                        birthdate = DateTime.Parse(personalBirthDate.Text),
                        birthplace = personalBirthPlace.Text,
                        familyStatusID = getFamilyStatusID(radioFamilyStatusSelected.ToString()[0].ToString()),
                        photoID = insertedPhotoID,
                        nationalityID = Convert.ToInt32(personalNationality.SelectedValue),
                        partyID = Convert.ToInt32(personalPoliticalParty.SelectedValue)


                    };
                    dbContext.employee.Add(newEmployee);

                }
                else
                {
                    var uEmployee = dbContext.employee.SingleOrDefault(k => k.ID == selectedPersonID);
                    if (uEmployee != null)
                    {
                        uEmployee.name = personalName.Text;
                        uEmployee.surname = personalSurname.Text;
                        uEmployee.secondname = personalPatroynmic.Text;
                        uEmployee.sex = radioSexSelected.ToString();
                        uEmployee.birthdate = DateTime.Parse(personalBirthDate.Text);
                        uEmployee.birthplace = personalBirthPlace.Text;
                        uEmployee.familyStatusID = getFamilyStatusID(radioFamilyStatusSelected.ToString()[0].ToString());
                        uEmployee.photoID = insertedPhotoID;
                        uEmployee.nationalityID = Convert.ToInt32(personalNationality.SelectedValue);
                        uEmployee.partyID = Convert.ToInt32(personalPoliticalParty.SelectedValue);
                    }

                    //make update
                    // dbContext.SaveChanges();

                }
                dbContext.SaveChanges();
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





        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;

            radioSexSelected = radioButton.Content.ToString()[0];
        }




        private void familyStatusRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
            radioFamilyStatusSelected = radioButton.Content.ToString().Substring(0, 1);
        }


        private bool ValidateForm()
        {
            var isValid = true;
            if (string.IsNullOrEmpty(personalName.Text) || personalName.Text.Length < minimumTextLength)
            {
                isValid = false;
            }
            if (string.IsNullOrEmpty(personalSurname.Text) || personalSurname.Text.Length < minimumTextLength)
            {
                isValid = false;
            }
            if (string.IsNullOrEmpty(personalPatroynmic.Text) || personalPatroynmic.Text.Length < minimumTextLength)
            {
                isValid = false;
            }
            if (string.IsNullOrEmpty(personalBirthDate.Text))
            {
                isValid = false;
            }
            else
            {
                //15 yashdan balacalari ishletmek olmaz
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - DateTime.Parse(personalBirthDate.Text);
                // Because we start at year 1 for the Gregorian
                // calendar, we must subtract a year here.
                int years = (zeroTime + span).Year - 1;

                // 1, where my other algorithm resulted in 0.
                if (years <= 15)
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("hello");
        }
        string strName, imageName;



        private void ImgCommit_Click(object sender, RoutedEventArgs e)
        {
            insertImageData();
        }



        private void insertImageData()
        {
            try
            {
                if (imageName != "")
                {
                    //Initialize a file stream to read the image file
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                    //Initialize a byte array with size of stream
                    byte[] imgByteArr = new byte[fs.Length];

                    //Read data from the file stream and put into the byte array
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                    //Close a file stream
                    fs.Close();


                    try
                    {
                        var newImage = new files()
                        {
                            code = "foto",
                            img = imgByteArr
                        };
                        dbContext.files.Add(newImage);
                        dbContext.SaveChanges();

                        insertedPhotoID = newImage.ID;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int d = (int)((ComboBoxItem)personalNationality.SelectedItem).Tag;
            MessageBox.Show(personalNationality.SelectedItem.ToString());
        }

        private void ImgBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif;*.png)|*.jpg;*.bmp;*.gif;*.png";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    personalPhoto.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            selectedPersonID = Convert.ToInt32(e.Fragment);
            if (selectedPersonID != 0)
            {
                fillControlsWithSelectedPersonData();
            }
            txtIDSaver.Text = e.Fragment;

        }

        private void fillControlsWithSelectedPersonData()
        {
            fillBasicControls();
            var k = getPersonData(selectedPersonID);
            personalName.Text = k.name;
            personalSurname.Text = k.surname;
            personalPatroynmic.Text = k.secondname;
            var sex = k.sex;
            if (sex == "K")
            {
                personalSexMan.IsChecked = true;
            }
            else if (sex == "Q")
            {
                personalSexWoman.IsChecked = true;
            }
            personalBirthDate.Text = k.birthdate.ToShortDateString();
            personalBirthPlace.Text = k.birthplace;
            var familyStatus = k.familyStatusID;
            if (familyStatus.HasValue)
            {
                if (familyStatus == 9)
                    family.IsChecked = true;
                if (familyStatus == 10)
                    single.IsChecked = true;
                if (familyStatus == 11)
                    widow.IsChecked = true;
                if (familyStatus == 12)
                    divorced.IsChecked = true;

            }
            personalNationality.SelectedValue = k.nationalityID;
            personalPoliticalParty.SelectedValue = k.partyID;
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
