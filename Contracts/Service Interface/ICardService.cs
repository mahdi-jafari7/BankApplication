using BankConsoleApp.Entities;
using BankConsoleApp.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp.Interfaces.Service_Interface
{
    public interface ICardService
    {
        public void Bardasht(string sourseid, double amount);
        public void Variz(string idcart, double amount);
        public Card GetById(int id);
        public Result IsPasswordValid(string cardnumber, string password);

        public double Mojodi(int id);
        Card GetByCardNumber(string cardnumber);
        public bool ChangePassword(string cardnumber, string oldpass, string newpass);

    }
}
