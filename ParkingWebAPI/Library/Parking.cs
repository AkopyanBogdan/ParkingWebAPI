using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SQLitePCL;

namespace ParkingWebAPI.Library
{
    public class Parking
    {
        private static readonly Lazy<Parking> instance = new Lazy<Parking>(() => new Parking());
        private int _parkingSpace;
        public List<Transaction> _transactions;
        private DateTime _dateOfLog;
        public List<Car> _cars;

        public double Balance { get; private set; }
        public static Parking Instance { get { return instance.Value; } }

        public Parking()
        {
            _parkingSpace = Settings._parkingSpace;
            _transactions = new List<Transaction>();
            _dateOfLog = DateTime.Now;
            _cars = new List<Car>();
        }

        // Add car to the parking
        public void AddCar(Car car)
        {
            if (_parkingSpace > 0)
            {
                _parkingSpace--;
                _cars.Add(car);
            }

            void IncreaseBalanceCarByTimer(Object obj)
            {
                if (car.Balance > Settings._price[car.CarType.ToString()])
                {
                    car.IncreaseBalance(Settings._price[car.CarType.ToString()], false);
                    _transactions.Add(new Transaction(car.Id, Settings._price[car.CarType.ToString()]));
                    Balance += Settings._price[car.CarType.ToString()];
                }
                else
                {
                    car.IncreaseBalance(Settings._price[car.CarType.ToString()] * Settings._fine, false);
                    _transactions.Add(new Transaction(car.Id, Settings._price[car.CarType.ToString()] * Settings._fine));
                    Balance += Settings._price[car.CarType.ToString()] * Settings._fine;
                }
            }

            TimerCallback tm = new TimerCallback(IncreaseBalanceCarByTimer);
            Timer timer = new Timer(tm, null, 0, Settings._timeout);
            WriteLog();
        }

        // Remove car from the parking by object Car
        public void RemoveCar(Car car)
        {
            if (_cars.Contains(car))
            {
                _parkingSpace++;
                _cars.Remove(car);
            }
        }

        // Remove car from the parking by Car Id
        public void RemoveCarById(string carId)
        {
            int count = 0;
            Car car = new Car();
            foreach (Car item in _cars)
            {
                if (item.Id == carId)
                {
                    car = item;
                    count++;
                }
            }
            if (count > 0)
            {
                int index = _cars.IndexOf(car);
                if (_cars[index].Balance >= 0)
                {
                    _cars.Remove(car);
                }
                else
                {
                    String.Format($"You must pay not less {(-1) * _cars[index].Balance} $");
                }
            }
            else
            {
                String.Format("Car not found");
            }
        }


        ////    Increase cars balance  by Id Car
        public void IncreaseBalanceCarById(string idCar, double sum, bool isAdding)
        {
            int count = 0;
            Car _car = new Car();
            foreach (var item in _cars)
            {
                if (item.Id == idCar)
                {
                    _car = item;
                    count++;
                }
            }
            if (count > 0)
            {
                int index = _cars.IndexOf(_car);
                _cars[index].IncreaseBalance(sum, isAdding);
            }
            else
            {
                String.Format("Car not found");
            }
        }

        // Show Balance of the parking
        public string ShowBalance()
        {
            return String.Format("The balance of parking: {0}", Balance);
        }

        // Find Count of free places
        public int CountOfFreePlaces()
        {
            return _parkingSpace;
        }

        // Write every minute to the file Transactions.log sum of transactions with date of writing
        public void WriteLog()
        {
            void WriteSumTransactionsForLastMinute(Object obj)
            {
                double sum = 0;
                foreach (var x in _transactions)
                {
                    if ((DateTime.Now - x.DateTimeOfTransaction).TotalMinutes <= 1) sum += x.SpentFunds;
                }

                using (StreamWriter writer = File.AppendText(Settings.url))
                {
                    writer.WriteLine("Log Entry : ");
                    writer.WriteLine("\r\nDate: {0}", DateTime.Now.ToString());
                    writer.WriteLine("Sum of transactions: {0}", sum);
                    writer.WriteLine("----------------------------\r\n");
                }
            }

            TimerCallback tm = new TimerCallback(WriteSumTransactionsForLastMinute);
            Timer timer = new Timer(tm, null, 0, 60000);
        }

        //Show to console data of  Transactions.log 
        public void ShowLog()
        {
            string[] data = File.ReadAllLines(Settings.url);

            foreach (string item in data)
            {
                Console.WriteLine(item);
            }
        }

        //Return data of  Transactions.log 
        public List<string> ReturnLogString()
        {
            string[] data = File.ReadAllLines(Settings.url);

            return data.ToList();
        }
        //Return the list of transactions of last minute
        public List<Transaction> ReturnTransactionsPerLastMinute()
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (var x in _transactions)
                if ((DateTime.Now - x.DateTimeOfTransaction).TotalMinutes <= 1)
                    transactions.Add(x);

            return transactions;
        }
        //Return the list of transactions of last minute by Id car
        public List<Transaction> ReturnTransactionsPerLastMinute(string id)
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (var item in _transactions)
                if ((DateTime.Now - item.DateTimeOfTransaction).TotalMinutes <= 1 && item.Id == id)
                    transactions.Add(item);

            return transactions;

        }

        //Show the list of transactions of last minute 
        public void ShowTransactionsPerLastMinute()
        {
            foreach (var transaction in _transactions)
                if ((DateTime.Now - transaction.DateTimeOfTransaction).TotalMinutes <= 1)
                    Console.WriteLine(transaction);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Car item in _cars)
            {
                sb.Append(item + "\r\n");
            }
            return String.Format(sb.ToString());
        }
    }
}
