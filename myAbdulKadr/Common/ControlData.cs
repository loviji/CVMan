using PersonMotion.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace PersonMotion.Common
{
    public class ControlData
    {
        private  peopleEntities dbContext = DBContextResolver.Instance;

        public ObservableCollection<organization> GetOrganizationList()
        {
            var list = from e in dbContext.organization
                       where e.isdeleted==false  select e;
            return new ObservableCollection<organization>(list);
        }

        public ObservableCollection<department> GetDepartmentList(int orgID)
        {
            var list = from dp in dbContext.department
                       where dp.organizationID == orgID && dp.isdeleted == false
                       select dp;
            return new ObservableCollection<department>(list);
        }


        public ObservableCollection<section> GetSectionList(int deptID)
        {
            var list = from sc in dbContext.section
                       where sc.departmentID == deptID && sc.isdeleted == false
                       select sc;
            return new ObservableCollection<section>(list);
        }


        public ObservableCollection<metaData> GetMetaDataByType(string metaType)
        {
            var list = from sc in dbContext.metaData
                       where sc.code == metaType && sc.isdeleted==false
                       select sc;
            return new ObservableCollection<metaData>(list);
        }
    }
}
