using BankConsoleApp.Entities;
using BankConsoleApp.Interfaces.Repository_Interface;
using BankConsoleApp.Interfaces.Service_Interface;
using BankConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        BankDbContext _bankdb;
        ICardService _cardService;
        public TransactionRepository(BankDbContext bankdb)
        {
            _bankdb = bankdb;
            _cardService = new CardService(bankdb);
            
        }

        public void Add(Transaction transaction)
        {
            _bankdb.Add(transaction);
            _bankdb.SaveChanges();
        }

        public List<Transaction> GetById(int uesrid)
        {
            var card = _cardService.GetById(uesrid);
            var transaction = _bankdb.Transactions.Where(t => t.forCardId == uesrid || t.DestinationCardNumber==card.CardNumber).ToList();
            
            return transaction;
        }


    }
}
