using PersonMotion.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace myAbdulKadr.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        MyRepository mr = new MyRepository();
       
        public ObservableCollection<employee> Employees
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

        //public ObservableCollection<PersonViewModel> Employees { get; set; }

        //public EmployeeViewModel()
        //{
        //    Employees = new ObservableCollection<PersonViewModel>(from m in dbContext.employee
        //                                                   select new PersonViewModel { Name=m.name }  );
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(prop));
        //}
    }

    public class MyRepository
    {
        private peopleEntities dbContext = new peopleEntities();
        public ObservableCollection<employee> Employees { get; private set; }

        public void ReadEmployees()
        {
            // db is the Entity Framework Context
            // In the real word I would use a separate DAL object
            Employees = new ObservableCollection<employee>(from m in dbContext.employee select m);
        }
    }

    //public class PersonViewModel : INotifyPropertyChanged
    //{
    //    private employee person;

    //    public PersonViewModel()
    //    {
    //        //person = e;
    //    }

    //    public string Name
    //    {
    //        get { return person.name; }
    //        set
    //        {
    //            person.name = value;
    //            OnPropertyChanged("Name");
    //        }
    //    }
        

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void OnPropertyChanged([CallerMemberName]string prop = "")
    //    {
    //        if (PropertyChanged != null)
    //            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    //    }
    //}
}
