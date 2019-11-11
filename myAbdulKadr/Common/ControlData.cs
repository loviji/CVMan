using myAbdulKadr.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAbdulKadr.Common
{
    public class ControlData
    {
        private static peopleEntities dbContext = new peopleEntities();

        public ObservableCollection<organization> GetOrganizationList()
        {
            var list = from e in dbContext.organization select e;
            return new ObservableCollection<organization>(list);
        }

        public ObservableCollection<department> GetDepartmentList(int orgID)
        {
            var list = from dp in dbContext.department
                       where dp.organizationID == orgID
                       select dp;
            return new ObservableCollection<department>(list);
        }


        public ObservableCollection<section> GetSectionList(int deptID)
        {
            var list = from sc in dbContext.section
                       where sc.departmentID == deptID
                       select sc;
            return new ObservableCollection<section>(list);
        }
    }
}
