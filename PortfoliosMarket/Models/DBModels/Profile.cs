using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class Profile
    {
        public Profile()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Privileges { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
