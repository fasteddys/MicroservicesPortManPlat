using System;
using System.Collections.Generic;

#nullable disable

namespace PMaP.Models.DBModels
{
    public partial class Efmigrationshistory
    {
        public string MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
}
