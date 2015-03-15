namespace PhonebookSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Contracts;

    internal class Phonebook
    {
        private const string DefaultCountryCode = "+359";
        private const char PlusSign = '+';

        private static readonly IPhonebookRepository PhonebookRepository = new SlowPhonebookRepository();

        private static readonly StringBuilder Output = new StringBuilder();

        private static void Main()
        {
            while (true)
            {
                var data = Console.ReadLine();
                if (data == "End" || data == null)
                {
                    break;
                }

                var i = data.IndexOf('(');
                if (i == -1)
                {
                    Console.WriteLine("error!");
                    Environment.Exit(0);
                }

                var k = data.Substring(0, i);
                if (!data.EndsWith(")"))
                {
                    Main();
                }

                var s = data.Substring(i + 1, data.Length - i - 2);
                var strings = s.Split(',');
                for (var j = 0; j < strings.Length; j++)
                {
                    strings[j] = strings[j].Trim();
                }

                if (k.StartsWith("AddPhone") && strings.Length >= 2)
                {
                    ExecuteCommand("Cmd3", strings);
                }
                else if (k == "ChangeРhone" && strings.Length == 2)
                {
                    ExecuteCommand("Cmd2", strings);
                }
                else if (k == "List" && strings.Length == 2)
                {
                    ExecuteCommand("Cmd1", strings);
                }
                else
                {
                    throw new StackOverflowException();
                }
            }

            Console.Write(Output);
        }

        private static void ExecuteCommand(string commandName, string[] commandParams)
        {
            switch (commandName)
            {
                case "AddPhone":
                {
                    var name = commandParams[0];
                    var phoneNumbers = commandParams.Skip(1).ToList();
                    var result = ExecuteAddPhoneCommand(name, phoneNumbers);

                    Print(result);
                }
                    break;
                case "Cmd2":
                {
                    var oldPhoneNumber = commandParams[0];          
                    var newPhoneNUmber = commandParams[1];

                    Print(PhonebookRepository.ChangePhone(
                        ConvetToCanonicalForm(oldPhoneNumber), 
                        ConvetToCanonicalForm(newPhoneNUmber)) + " numbers changed");
                }
                    break;
                default:
                {
                    try
                    {
                        IEnumerable<PhonebookEntry> entries =
                            PhonebookRepository.ListEntries(int.Parse(commandParams[0]), int.Parse(commandParams[1]));
                        foreach (var entry in entries)
                        {
                            Print(entry.ToString());
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Print("Invalid range");
                    }
                }
                    break;
            }
        }

        private static string ExecuteAddPhoneCommand(string name, IList<string> phoneNumber)
        {
            for (var i = 0; i < phoneNumber.Count; i++)
            {
                phoneNumber[i] = ConvetToCanonicalForm(phoneNumber[i]);
            }

            var isNewEntry = PhonebookRepository.AddPhone(name, phoneNumber);
            return isNewEntry ? "Phone entry created" : "Phone entry merged";
        }

        private static string ConvetToCanonicalForm(string phoneNumber)
        {
            var digitsAndPlusPhoneNumber = GetDigitsAndPlus(phoneNumber);
            var leadinghZerosRemovedPhoneNumber = digitsAndPlusPhoneNumber.TrimStart('0');
            var canonicalFormPhoneNumber = leadinghZerosRemovedPhoneNumber;

            if (!canonicalFormPhoneNumber.StartsWith(PlusSign.ToString(CultureInfo.InvariantCulture)))
            {
                return string.Format("{0}{1}", DefaultCountryCode, canonicalFormPhoneNumber);
            }

            return canonicalFormPhoneNumber;
        }

        private static string GetDigitsAndPlus(string phoneNumber)
        {
            var result = new StringBuilder();
            foreach (var symbol in phoneNumber)
            {
                if (char.IsDigit(symbol) || symbol == PlusSign)
                {
                    result.Append(symbol);
                }
            }

            return result.ToString();
        }

        private static void Print(string text)
        {
            Output.AppendLine(text);
        }
    }
}
