using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankConsoleApp.Entities;

namespace BankConsoleApp.DAL
{
    public static class InMemoryDB
    {
        public static User CurrentUser { get; set; }
        public static Card CurrentCard { get; set; }

    }
}
