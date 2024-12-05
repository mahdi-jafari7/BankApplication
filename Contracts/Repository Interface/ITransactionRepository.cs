using BankConsoleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Interfaces.Repository_Interface
{
    public interface ITransactionRepository
    {

        public List<Transaction> GetById(int uesrid);
        void Add(Transaction transaction);
    }
}
