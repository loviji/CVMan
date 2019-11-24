using myAbdulKadr.Model;

namespace myAbdulKadr.Common
{
    public class DBContextResolver
    {
        private static peopleEntities _instance;
        public static peopleEntities Instance
        {
            get
            {
                return _instance ?? (_instance = new peopleEntities());
            }
        }

    }
}
