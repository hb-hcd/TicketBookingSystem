using System;
using System.Configuration;
using System.Collections.Specialized;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookingApp app = new BookingApp();
            app.Booking();

        }
    }

    public class BookingApp
    {
        string baseAddress = @"C:\Users\hhbry\Desktop\";
        string date = DateTime.Now.ToString("yyyyMMdd");
        bool IsValidate = false;
        string order;
        double OriginalFee = 50;
        public BookingApp()
        {

        }

        public void Booking()
        {
            bool IsQuit = false;
            do
            {
                Start();
                Console.WriteLine("Press y to buy another ticket or press any other keys to quit.");
                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine("");
                if ( cki.Key != ConsoleKey.Y )
                {
                    IsQuit = true;
                }
            } while ( !IsQuit );
            Console.WriteLine("Bye!");
        }
        public void Start()
        {           
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            Console.WriteLine("Welcome to Winnipeg-Toronto Express Booking System!");
            while ( !IsValidate )
            {
                Console.WriteLine("Please enter the month you want to travel: ");
                string input = Console.ReadLine();
                if ( !ValidateInput(input) )
                {
                    Console.WriteLine("Sorry, your input should be an integer.");
                    continue;
                }

                if ( !ValidateInteger(int.Parse(input)) )
                {
                    Console.WriteLine("Sorry, your input should between 1 to 12.");
                    continue;
                }
                order = CalculateFee(input);
            }

            Console.WriteLine($"Your order information is: {order}");
            RecordOrder(order);
            IsValidate = false;
        }


        public bool ValidateInput(string month)
        {
            int i;
            bool IsInteger = int.TryParse(month, out i);
            return IsInteger;

        }

        public bool ValidateInteger(int number)
        {
            if ( number < 0 || number > 12 )
            {
                return false;
            }
            else
            {
                IsValidate = true;
                return IsValidate;
            }

        }
        public string CalculateFee(string number)
        {
            double fee;
            string discount;
            switch ( number )
            {
                case "12":
                    discount = ConfigurationManager.AppSettings["12"];
                    fee = OriginalFee * ( double.Parse(discount) / 100 );
                    discount = "0";
                    break;
                case "6":
                case "7":
                    discount = ConfigurationManager.AppSettings["6"];
                    fee = OriginalFee * ( 1 - double.Parse(discount) / 100 );
                    break;
                default:
                    discount = ConfigurationManager.AppSettings["1"];
                    fee = OriginalFee;
                    break;
            }

            return $"{OriginalFee}  {number}  {discount}  {fee}";
        }

        public void RecordOrder(string orderDetails)
        {
            string fileName = baseAddress + date.ToString() + "OrderHistory.txt";
            Console.WriteLine(fileName);
            if ( !File.Exists(fileName) )
            {
                FileStream fs = File.Create(fileName);
                File.WriteAllText(fileName, orderDetails);
                fs.Close();
            }
            else
            {
                using StreamWriter file = new(fileName, append: true);
                file.WriteLine(orderDetails);
                file.Close();
            }

        }

    }
}