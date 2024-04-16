using DiyorMarket.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiyorMarket.Domain.Entities
{
    public class Customer : EntityBase
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
