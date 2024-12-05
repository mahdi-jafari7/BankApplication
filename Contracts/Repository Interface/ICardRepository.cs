using BankConsoleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Interfaces.Repository_Interface
{
    public interface ICardRepository
    {
        public bool PassowrdIsValid(string cardnumber, string password);

        public void Bardasht(string idcart, double amount);
        public void Variz(string idcart, double amount);

        public Card GetById(int id);

        public double Mojodi(int id);

        public Card GetByCardNumber(string cardnumber);

        public void ClearWongPassword(string cardnumber);
        public int GetWrongPasswordTry (string cardnumber);
        public void SetWrongPasswordTry (string cardnumber);

        public void ChangePass(string cardnumber, string password);
    }
}
