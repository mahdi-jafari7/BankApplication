using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Entities
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public double Balance { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public int TryToEnterTimes { get; set; } = 0;
        public List<Transaction> transactions { get; set; }
        public User UserCardOwner { get; set; }
        public int UserCardOwnerId { get; set; }
    }
}
