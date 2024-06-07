﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class UserWork : EntityBase
    {
        public int WorkId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }

        public Work? Work { get; set; }
        public Employee? Employee { get; set; }
        public Client? Client { get; set; }
    }
}