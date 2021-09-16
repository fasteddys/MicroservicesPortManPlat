using System;
using System.Collections.Generic;

#nullable disable

namespace Portfolios.Models.DBModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
