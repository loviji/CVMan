using ExtensionMethods;
using FirstFloor.ModernUI.Windows;
using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PersonMotion.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personaSalary.xaml
    /// </summary>
    public  partial  class personaSalary : UserControl, IContent
    {
        public personaSalary()
        {
            InitializeComponent();
        }


        private static peopleEntities dbContext = DBContextResolver.Instance;
        private int selectedPersonID { get; set; }

        private void SaveSalaryData(object sender, RoutedEventArgs e)
        {
            try
            {

                salary uSalary = dbContext.salary.SingleOrDefault(k => k.employeeID == selectedPersonID);

                if (uSalary != null)
                {
                    uSalary.baseSalary = Convert.ToDecimal(string.IsNullOrEmpty(baseSalary.Text) ? "0" : baseSalary.Text);

                    uSalary.dosentBase = Convert.ToDecimal(string.IsNullOrEmpty(dosentBase.Text) ? "0" : dosentBase.Text);
                    uSalary.professorBase = Convert.ToDecimal(string.IsNullOrEmpty(professorBase.Text) ? "0" : professorBase.Text);
                    uSalary.academicBase = Convert.ToDecimal(string.IsNullOrEmpty(academicBase.Text) ? "0" : academicBase.Text);
                    uSalary.scientceDoctorBase = Convert.ToDecimal(string.IsNullOrEmpty(scientceDoctorBase.Text) ? "0" : scientceDoctorBase.Text);
                    uSalary.scienceCandadateBase = Convert.ToDecimal(string.IsNullOrEmpty(scienceCandidateBase.Text) ? "0" : scienceCandidateBase.Text);

                    uSalary.dosentPercent = Convert.ToDecimal(string.IsNullOrEmpty(dosentPercent.Text) ? "0" : dosentPercent.Text);
                    uSalary.professorPercent = Convert.ToDecimal(string.IsNullOrEmpty(professorPercent.Text) ? "0" : professorPercent.Text);
                    uSalary.academicPercent = Convert.ToDecimal(string.IsNullOrEmpty(academicPercent.Text) ? "0" : academicPercent.Text);
                    uSalary.scientceDoctorPercent = Convert.ToDecimal(string.IsNullOrEmpty(scientceDoctorPercent.Text) ? "0" : scientceDoctorPercent.Text);
                    uSalary.scienceCandadatePercent = Convert.ToDecimal(string.IsNullOrEmpty(scienceCandadatePercent.Text) ? "0" : scienceCandadatePercent.Text);



                    //uSalary.employeeID = selectedPersonID;
                }

                else
                {
                    salary eSalary = new salary();

                    eSalary.baseSalary = Convert.ToDecimal(string.IsNullOrEmpty(baseSalary.Text) ? "0" : baseSalary.Text);
                    eSalary.dosentBase = Convert.ToDecimal(string.IsNullOrEmpty(dosentBase.Text) ? "0" : dosentBase.Text);
                    eSalary.professorBase = Convert.ToDecimal(string.IsNullOrEmpty(professorBase.Text) ? "0" : professorBase.Text);
                    eSalary.academicBase = Convert.ToDecimal(string.IsNullOrEmpty(academicBase.Text) ? "0" : academicBase.Text);
                    eSalary.scientceDoctorBase = Convert.ToDecimal(string.IsNullOrEmpty(scientceDoctorBase.Text) ? "0" : scientceDoctorBase.Text);
                    eSalary.scienceCandadateBase = Convert.ToDecimal(string.IsNullOrEmpty(scienceCandidateBase.Text) ? "0" : scienceCandidateBase.Text);
                    eSalary.employeeID = selectedPersonID;
                    dbContext.salary.Add(eSalary);
                }

                dbContext.SaveChanges();

                //  txtStatus.Text = rOrganization.organizationName + " has being added!";
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

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

            selectedPersonID = Convert.ToInt32(e.Fragment);

            if (GlobalCache.currentPersonID != 0)
            {
                selectedPersonID = GlobalCache.currentPersonID;
                populateFormControls(selectedPersonID);
            }


            //selectedPersonID = Convert.ToInt32(e.Fragment);
            //if (selectedPersonID != 0)
            //{
            //    populateFormControls(selectedPersonID);
            //}
        }

        private void populateFormControls(int selectedPersonID)
        {
            var rSalary = dbContext.salary.Where(d => d.employeeID == selectedPersonID).SingleOrDefault();
            if (rSalary != null)
            {

                decimal baseSalaryAmount = rSalary.baseSalary;
                baseSalary.Text = baseSalaryAmount.ToString();

                dosentBase.Text = rSalary.dosentBase.ToString();
                professorBase.Text = rSalary.professorBase.ToString();
                academicBase.Text = rSalary.academicBase.ToString();
                scientceDoctorBase.Text = rSalary.scientceDoctorBase.ToString();
                scienceCandidateBase.Text = rSalary.scienceCandadateBase.ToString();


                if (baseSalaryAmount > 0)
                {
                    dosentPercent.Text = PersentageCalc(rSalary.dosentBase, baseSalaryAmount);
                    professorPercent.Text = PersentageCalc(rSalary.professorBase, baseSalaryAmount);
                    academicPercent.Text = PersentageCalc(rSalary.academicBase, baseSalaryAmount);
                    scientceDoctorPercent.Text = PersentageCalc(rSalary.scientceDoctorBase, baseSalaryAmount);
                    scienceCandadatePercent.Text = PersentageCalc(rSalary.scienceCandadateBase, baseSalaryAmount);
                }

            }

        }

        private string PersentageCalc(decimal salary, decimal baseSalary)
        {
            return (salary / baseSalary * 100).ToString("0.##");

        }

        private string ReversePersentageCalc(decimal percent, decimal baseSalary)
        {
            return (percent * baseSalary / 100).ToString("0.##");
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

        private void DosentPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                dosentBase.Text = ReversePersentageCalc(Convert.ToDecimal(dosentPercent.Text), Convert.ToDecimal(baseSalary.Text));
        }

        private void ProfessorPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                professorBase.Text = ReversePersentageCalc(Convert.ToDecimal(professorPercent.Text), Convert.ToDecimal(baseSalary.Text));
        }

        private void AcademicPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                academicBase.Text = ReversePersentageCalc(Convert.ToDecimal(academicPercent.Text), Convert.ToDecimal(baseSalary.Text));
        }

        private void ScientceDoctorPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                scientceDoctorBase.Text = ReversePersentageCalc(Convert.ToDecimal(scientceDoctorPercent.Text), Convert.ToDecimal(baseSalary.Text));
        }

        private void ScienceCandadatePercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                scienceCandidateBase.Text = ReversePersentageCalc(Convert.ToDecimal(scienceCandadatePercent.Text), Convert.ToDecimal(baseSalary.Text));
        }



        private void DosentBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                dosentPercent.Text = PersentageCalc(Convert.ToDecimal(dosentBase.Text), Convert.ToDecimal(baseSalary.Text));
            sumSalary.Text = CalcSumSalary();
        }

        private void ProfessorBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                professorPercent.Text = PersentageCalc(Convert.ToDecimal(professorBase.Text), Convert.ToDecimal(baseSalary.Text));
            sumSalary.Text = CalcSumSalary();
        }

        private void AcademicBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                academicPercent.Text = PersentageCalc(Convert.ToDecimal(academicBase.Text), Convert.ToDecimal(baseSalary.Text));
            sumSalary.Text = CalcSumSalary();
        }

        private void ScientceDoctorBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                scientceDoctorPercent.Text = PersentageCalc(Convert.ToDecimal(scientceDoctorBase.Text), Convert.ToDecimal(baseSalary.Text));
            sumSalary.Text = CalcSumSalary();
        }

        private void ScienceCandadateBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(baseSalary.Text))
                scienceCandadatePercent.Text = PersentageCalc(Convert.ToDecimal(scienceCandidateBase.Text), Convert.ToDecimal(baseSalary.Text));
            sumSalary.Text = CalcSumSalary();
        }

        private void BaseSalary_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(dosentPercent.Text))
                dosentBase.Text = ReversePersentageCalc(Convert.ToDecimal(dosentPercent.Text), Convert.ToDecimal(baseSalary.Text));
            if (!string.IsNullOrEmpty(professorPercent.Text))
                professorBase.Text = ReversePersentageCalc(Convert.ToDecimal(professorPercent.Text), Convert.ToDecimal(baseSalary.Text));
            if (!string.IsNullOrEmpty(academicPercent.Text))
                academicBase.Text = ReversePersentageCalc(Convert.ToDecimal(academicPercent.Text), Convert.ToDecimal(baseSalary.Text));
            if (!string.IsNullOrEmpty(scientceDoctorPercent.Text))
                scientceDoctorBase.Text = ReversePersentageCalc(Convert.ToDecimal(scientceDoctorPercent.Text), Convert.ToDecimal(baseSalary.Text));
            if (!string.IsNullOrEmpty(scienceCandadatePercent.Text))
                scienceCandidateBase.Text = ReversePersentageCalc(Convert.ToDecimal(scienceCandadatePercent.Text), Convert.ToDecimal(baseSalary.Text));

            sumSalary.Text=CalcSumSalary();
        }

        private string CalcSumSalary()
        {
          

            decimal?[] values = new decimal?[] {baseSalary.Text.ZeroIfEmpty(),
                dosentBase.Text.ZeroIfEmpty(),
                professorBase.Text.ZeroIfEmpty(),
                academicBase.Text.ZeroIfEmpty(),
                scientceDoctorBase.Text.ZeroIfEmpty(),
                scienceCandidateBase.Text.ZeroIfEmpty()};
            //str sum1 = values.Sum(); // returns 1
            return values.Sum().Value.ToString("0.##");
        }

        
    }
}


namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static decimal ZeroIfEmpty(this string s)
        {
            return Convert.ToDecimal(string.IsNullOrEmpty(s) ? "0" : s);
        }
    }
}
