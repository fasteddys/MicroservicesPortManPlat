using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
    }
}
