using System.Collections.Generic;
using Node.Core.Models;

namespace Explorer.Models
{
    public class AddressInformationViewModel
    {
        public List<Transaction> Transactions { get; set; }

        public decimal Balance { get; set; }

        public string Address { get; set; }
    }
}