using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Service.Models
{
    public class UserItem
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserItem ? ((UserItem)obj).Id.Equals(Id) : false;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, FirstName: {1}, LastName: {2}", Id, FirstName, LastName);
        }
    }
}
