using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class Client : EntityBase
    {
        public string Login { get; set; }

        [NotMapped]
        private string _phoneNumber;
        
        public string PhoneNumber 
        { 
            get => _phoneNumber;
            set
            {
                if(Regex.IsMatch(value, "^8\\d{10}$"))
                {
                    _phoneNumber = value;
                }
            }
        }

        public ICollection<UserWork> UserWorks { get; set; }
    }
}
