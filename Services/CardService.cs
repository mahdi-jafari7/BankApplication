using BankConsoleApp.DAL;
using BankConsoleApp.DAL.Repositories;
using BankConsoleApp.Entities;
using BankConsoleApp.Framework;
using BankConsoleApp.Interfaces.Repository_Interface;
using BankConsoleApp.Interfaces.Service_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharprompt;
using Spectre.Console;


namespace BankConsoleApp.Services
{
    public class CardService : ICardService
    {
        ICardRepository _cardRepository;
        BankDbContext _db;
        public CardService(BankDbContext db)
        {
            _db = db;
            _cardRepository = new CardRepository(db);
        }




        public void Bardasht(string sourseid, double amount)
        {
            _cardRepository.Bardasht(sourseid, amount);
        }


        public void Variz(string idcart, double amount)
        {
            _cardRepository.Variz(idcart, amount);
        }

        public Card GetById(int id)
        {
            return _cardRepository.GetById(id);
        }

        public Result IsPasswordValid(string cardnumber, string password)
        {
            if (GetByCardNumber(cardnumber) is not null)
            {
                var WrongPasswordTry = _cardRepository.GetWrongPasswordTry(cardnumber);
                if (WrongPasswordTry >= 3)
                {
                    var user = _cardRepository.GetByCardNumber(cardnumber);
                    user.IsActive = false;
                    return new Result()
                    {
                        IsSuccess = false,
                        Message = "your entered wrong password more than 3 times! your account [bold red]Blocked![/]"
                    };
                }
                else
                {
                    var LoginResult = _cardRepository.PassowrdIsValid(cardnumber, password);
                    var card = _cardRepository.GetByCardNumber(cardnumber);
                    if (LoginResult)
                    {
                        InMemoryDB.CurrentCard = _cardRepository.GetByCardNumber(cardnumber);
                        return new Result() { IsSuccess = true, Message = $"[bold green]Welcome {InMemoryDB.CurrentCard.HolderName} [/] now you can use this card (card number : {InMemoryDB.CurrentCard.CardNumber})" };
                    }
                    else if (cardnumber.Length != 16)
                    {
                        return new Result() { IsSuccess = false, Message = "Your Card number is less or more than [red]16 digits[/]" };
                    }
                    else
                    {
                        _cardRepository.SetWrongPasswordTry(cardnumber);
                        return new Result() { IsSuccess = false, Message = "Your password is [red]wrong[/] " };
                    }
                }
            }
            else
            {
                return new Result() { IsSuccess = false, Message = " [red]No acoount with this cart number[/]" };
             }
        }
        public double Mojodi(int id)
        {
            var user = GetById(id);
            if (user.IsActive && user is not null)
            {
                return _cardRepository.Mojodi(user.Id);
            }
            throw new InvalidOperationException("User not found or not active.");
        }

        public Card GetByCardNumber(string cardnumber)
        {
            return _cardRepository.GetByCardNumber(cardnumber);

        }

        public bool ChangePassword(string cardnumber,string oldpass,string newpass)
        {
            var card = GetByCardNumber(cardnumber);

            if(card.Password == oldpass)
            {
                _cardRepository.ChangePass(card.CardNumber,newpass);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
