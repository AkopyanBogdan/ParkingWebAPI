using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParkingWebAPI.Library
{
    public class Car
    {
        // fields
        private string _id;

        // Properties
        public CarType CarType { get; set; }
        public double Balance { get; private set; }
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

        // ctor
        public Car() { }
        public Car(CarType carType, string id, double balance)
        {
            Id = id;
            CarType = carType;
            Balance = balance;
        }

        // Methods
        public void IncreaseBalance(double sumIncrease, bool IsAdding)
        {
            if (sumIncrease < 0)
                return;

            if (IsAdding == true)
                Balance += sumIncrease;
            else
                Balance -= sumIncrease;
        }

        public override string ToString()
        {
            return String.Format($"Type: {CarType.ToString()}\tId: {Id}\tBalance:{Balance}");
        }

    }
}
