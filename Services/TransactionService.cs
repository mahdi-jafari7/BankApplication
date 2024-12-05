using BankConsoleApp.DAL;
using BankConsoleApp.DAL.Repositories;
using BankConsoleApp.Entities;
using BankConsoleApp.Interfaces.Repository_Interface;
using BankConsoleApp.Interfaces.Service_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Services
{
    public class TransactionService : ITransactionService
    {
        BankDbContext _db;
        ITransactionRepository _transactionRepo;
        ICardService _cardService;
        public TransactionService(BankDbContext db)
        {
            _db = db;
            _transactionRepo = new TransactionRepository(db);
            _cardService = new CardService(db); 
        }

        public void Add(Transaction transaction)
        {
            _transactionRepo.Add(transaction);
        }

        public List<Transaction> GetById(int id)
        {
            return _transactionRepo.GetById(id);
        }


        public bool Transfer(string sourseId, string destinationId, double amount)
        {
            var source = _cardService.GetByCardNumber(sourseId);
            var des = _cardService.GetByCardNumber(destinationId);

            if (source != null && des != null && amount < source.Balance &&
                source.CardNumber.Length == 16 && des.CardNumber.Length == 16)
            {
                if (amount > 1000)
                {
                    amount = amount + (amount * 0.015);
                    _cardService.Bardasht(sourseId,amount);
                }
                else
                {
                    amount = amount + (amount * 0.005);
                    _cardService.Bardasht(sourseId, amount);

                }

                _cardService.Variz(destinationId, amount);
                var tranaction = new Transaction
                {
                    SourceCardNumber = sourseId,
                    DestinationCardNumber = destinationId,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    isSuccessful = true,
                    forCardId = source.Id
                };
                _transactionRepo.Add(tranaction);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
