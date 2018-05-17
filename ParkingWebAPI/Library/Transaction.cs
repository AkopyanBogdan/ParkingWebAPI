using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParkingWebAPI.Library
{
    public class Transaction
    {
        private string _id;
        private double _spentfounds;
        public DateTime DateTimeOfTransaction = new DateTime();

        // Properties
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id == null)
                {
                    string pattern = @"^[A-Z]{2}[0-9]{4}[A-Z]{2}$";

                    Match match = Regex.Match(value, pattern);
                    if (match.Success)
                    {
                        _id = value;
                    }
                    else throw new TypeInitializationException("You must enter: XX1111XX", new Exception("Bad value"));
                }
            }
        }
        public double SpentFunds
        {
            get { return _spentfounds; }
            set { _spentfounds = value; }
        }

        // ctors
        public Transaction(string id, double spentfounds)
        {
            DateTimeOfTransaction = DateTime.Now;
            Id = id;
            SpentFunds = spentfounds;
        }

        public override string ToString()
        {
            return String.Format($"Time of Transac: {DateTimeOfTransaction}\tId: {Id}\tSpend Founds: {SpentFunds}");
        }
    }
}
