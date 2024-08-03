using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SearchForTheCriminal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Detective detective = new Detective(new BaseCriminals());
            detective.Work();
        }
    }

    class Detective
    {
        private BaseCriminals _baseCriminals;

        public Detective(BaseCriminals baseCriminals)
        {
            _baseCriminals = baseCriminals;
        }

        public void Work()
        {
            const string CommandExit = "exit";

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine($"Введите {CommandExit}, чтобы выйти или что-нибудь, чтобы продолжить.");

                switch (Console.ReadLine())
                {
                    case CommandExit:
                        isRunning = false;
                    break;

                    default:
                        Search();
                        break;
                }

                Console.ReadLine();
                Console.Clear();
            }
        }

        private void Search()
        {
            Console.WriteLine("Введите рост:");
            int height = UserUtils.ReadInt();

            Console.WriteLine("Введите вес:");
            int weight = UserUtils.ReadInt();

            Console.WriteLine("Введите национальность.");
            string nationality = Console.ReadLine();

            _baseCriminals.TryShowCriminals(height, weight, nationality);
        }
    }

    class BaseCriminals
    {
        private List<Criminal> criminals = new List<Criminal>();

        public BaseCriminals()
        {
            CreatorCriminal creatorCriminal = new CreatorCriminal();

            int amountBaseCriminals = 100;

            criminals.Add(new Criminal("Иванов", "Иван", "Иванович", "русский", false, 180, 80));
            criminals.Add(new Criminal("Иванов", "Иван", "Иванович", "русский", true, 180, 80));

            for (int i = 0; i < amountBaseCriminals; i++)
                criminals.Add(creatorCriminal.GenerateCriminal());
        }

        public void TryShowCriminals(int height, int weight, string nationality)
        {
            List<Criminal> filteredCriminals = criminals.Where(criminal => criminal.Height == height && 
            criminal.Weight == weight && criminal.Nationality == nationality && criminal.IsImprisoned == false).Select(criminal => criminal).ToList<Criminal>();

            if (filteredCriminals.Count != 0)
            {
                foreach (Criminal criminal in filteredCriminals)
                    criminal.ShowCriminal();
            }
            else
            {
                Console.WriteLine("Человек с такими параметрами не найден.");
            }
        }
    }

    class CreatorCriminal
    {
        public Criminal GenerateCriminal()
        {
            bool isImprisoned = true;

            if (UserUtils.GenerateRandomNumber(0,1) == 1)
                isImprisoned = false;

            int minLimitRandomHeight = 150;
            int maxLimitRandomHeight = 220;

            int minLimitRandomWeight = 50;
            int maxLimitRandomWeight = 120;

            return new Criminal(GetLastName(), GetFirstName(), GetSurname(), GetNationality(), isImprisoned,
                UserUtils.GenerateRandomNumber(minLimitRandomHeight,maxLimitRandomHeight), UserUtils.GenerateRandomNumber(minLimitRandomWeight,maxLimitRandomWeight));
        }

        private string GetLastName()
        {
            List<string> names = new List<string>() { "Громов", "Johnson", "Смирнов", "Мартиросян", "Smith", "Аракчеев", "Шевченко"};

            return names[UserUtils.GenerateRandomNumber(0, names.Count - 1)];
        }

        private string GetFirstName()
        {
            List<string> names = new List<string>() { "Иван", "Боб", "Зураб", "Смит", "Гомер", "Михаил" };

            return names[UserUtils.GenerateRandomNumber(0, names.Count - 1)];
        }

        private string GetSurname()
        {
            List<string> names = new List<string>() { "John", "Владимирович", "Павлович", "Зурабович" };

            return names[UserUtils.GenerateRandomNumber(0, names.Count - 1)];
        }

        private string GetNationality()
        {
            List<string> names = new List<string>() { "русский", "узбек", "американец", "татарин", "украинец", "цыган", "грузин"};

            return names[UserUtils.GenerateRandomNumber(0, names.Count - 1)];
        }
    }

    class Criminal
    {
        private string _lastName;
        private string _firstName;
        private string _surname;

        public Criminal(string lastName, string firstName, string surname, string nationality, bool isImprisoned, int height, int weight)
        {
            _lastName = lastName;
            _firstName = firstName;
            _surname = surname;
            Nationality = nationality;

            IsImprisoned = isImprisoned;

            Height = height;
            Weight = weight;
        }

        public bool IsImprisoned { get; private set; }

        public string Nationality { get; private set; }

        public int Height { get; private set; }
        public int Weight { get; private set; }

        public void ShowCriminal()
        {
            Console.WriteLine($"{_lastName}-{_firstName}-{_surname}-{Nationality}-рост {Height}- вес {Weight}- задержан {IsImprisoned}");
        }
    }

    class UserUtils
    {
        private static Random random = new Random();

        public static int GenerateRandomNumber(int minLimitRandom, int maxLimitRandom)
        {
            return random.Next(minLimitRandom, maxLimitRandom);
        }

        public static int ReadInt()
        {
            int number;

            while (int.TryParse(Console.ReadLine(),out number) == false)
                Console.WriteLine("Это не число.");

            return number;
        }
    }
}
