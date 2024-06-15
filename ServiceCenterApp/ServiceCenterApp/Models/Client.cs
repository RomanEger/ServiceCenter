using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

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

        public ICollection<Work> Works { get; set; }
    }
}
