﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PortfolioValuation.Models.DBModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}