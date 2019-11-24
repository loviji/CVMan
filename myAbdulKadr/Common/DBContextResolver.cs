using myAbdulKadr.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
