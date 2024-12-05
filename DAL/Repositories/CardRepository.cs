using BankConsoleApp.Entities;
using BankConsoleApp.Interfaces.Repository_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        BankDbContext _bankdb;
        public CardRepository(BankDbContext bankdb)
        {
            _bankdb = bankdb;
        }

        
        public void Bardasht(string idcart, double amount)
        {
            _bankdb.Cards.FirstOrDefault(o => o.CardNumber == idcart).Balance -= amount;


            _bankdb.SaveChanges();
        }


        public void Variz(string idcart, double amount)
        {
            _bankdb.Cards.FirstOrDefault(o => o.CardNumber == idcart).Balance += amount;


            _bankdb.SaveChanges();
        }

        public Card GetById(int id)
        {
            return _bankdb.Cards.FirstOrDefault(o => o.Id == id);
        }

        public bool PassowrdIsValid(string cardnumber, string password)
        {
            return _bankdb.Cards.Any(u => u.CardNumber == cardnumber && u.Password == password);
        }

        public double Mojodi(int id)
        {
            var user = _bankdb.Cards.FirstOrDefault(o => o.Id == id);
            return user.Balance;
        }

        public Card GetByCardNumber(string cardnumber)
        {
            return _bankdb.Cards.FirstOrDefault(o => o.CardNumber == cardnumber);
        }

        public void ClearWongPassword(string cardnumber)
        {
            _bankdb.Cards.FirstOrDefault(p => p.CardNumber == cardnumber).TryToEnterTimes = 0;
        }

        public int GetWrongPasswordTry(string cardnumber)
        {
            return _bankdb.Cards.FirstOrDefault(p=>p.CardNumber == cardnumber).TryToEnterTimes;
        }

        public void SetWrongPasswordTry(string cardnumber)
        {
            _bankdb.Cards.FirstOrDefault(p => p.CardNumber == cardnumber).TryToEnterTimes++;
            _bankdb.SaveChanges();
        }

        public void ChangePass(string cardnumber,string password)
        {
            var card = GetByCardNumber(cardnumber);
            card.Password = password;
            _bankdb.SaveChanges();
        }
    }
}
