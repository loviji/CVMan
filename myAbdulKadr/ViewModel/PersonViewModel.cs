using PersonMotion.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace myAbdulKadr.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        MyRepository mr = new MyRepository();

        public ObservableCollection<EMP_LIST> Employees
        {
            get { return mr.Employees; }

        }



        public EmployeeViewModel()
        {
            ReadEmployees();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ReadEmployees()
        {
            mr.ReadEmployees();
            InternalPropertyChanged("Employees");

        }



        private void InternalPropertyChanged(string v)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(v));
        }


    }

    public class MyRepository
    {
        private peopleEntities dbContext = new peopleEntities();
        public ObservableCollection<EMP_LIST> Employees { get; private set; }

        public void ReadEmployees()
        {
            // db is the Entity Framework Context
            // In the real word I would use a separate DAL object
            Employees = new ObservableCollection<EMP_LIST>(from m in dbContext.EMP_LIST orderby m.isfired select m);
        }
    }


}
