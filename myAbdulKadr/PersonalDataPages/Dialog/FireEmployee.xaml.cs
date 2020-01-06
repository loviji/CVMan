using PersonMotion.Common;
using PersonMotion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonMotion.PersonalDataPages.Dialog
{
    /// <summary>
    /// Interaction logic for FireEmployee.xaml
    /// </summary>
    public partial class FireEmployee : UserControl
    {
        public FireEmployee(int selectedEmployee)
        {
            selectedEmp = selectedEmployee;
            InitializeComponent();
        }
        private static peopleEntities dbContext = DBContextResolver.Instance;

        public int selectedEmp { get; set; }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var fEmployeePosition = dbContext.position.OrderBy(k => k.isMain).Where(k => k.empID == selectedEmp);
            var fireDate = DateTime.Parse(personFireDate.Text);
            if (fireDate.Year >= DateTime.Now.Year)
            {
                if (fEmployeePosition.Count() > 0)
                {
                    try
                    {
                        foreach (var f in fEmployeePosition)
                        {
                            f.endDate = fireDate;
                        }

                        var employer = dbContext.employee.SingleOrDefault(j => j.ID == selectedEmp);
                        employer.isfired = true;
                      
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
                else
                {
                    MessageBox.Show("Vəzifəsi olmayan işçini işdən çıxartmaq mümkün deyil");
                }
            }
            else
            {
                MessageBox.Show("Xitam tarixi təyin edilməmişdir");
            }
        }

    }
}

