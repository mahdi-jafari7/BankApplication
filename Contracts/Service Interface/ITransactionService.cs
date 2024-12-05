using BankConsoleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Interfaces.Service_Interface
{
    public interface ITransactionService
    {
        public void Add(Transaction transaction);
        public List<Transaction> GetById(int id);
        public bool Transfer(string sourseId, string destinationId, double amount);


    }
}
