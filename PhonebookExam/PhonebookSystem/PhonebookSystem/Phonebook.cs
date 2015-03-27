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
        private const string ChangePhoneCommand = "ChangePhone";
        private const string AddPhoneCommand = "AddPhone";

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

        private static string ExecuteCommand(string commandName, string[] commandParams)
        {
            string commandResult;
            switch (commandName)
            {
                case AddPhoneCommand:
                {
                    var name = commandParams[0];
                    var phoneNumbers = commandParams.Skip(1).ToList();
                    commandResult = ExecuteAddPhoneCommand(name, phoneNumbers);

                    return commandResult;
                }
                    break;
                case ChangePhoneCommand:
                {
                    var oldPhoneNumber = ConvetToCanonicalForm(commandParams[0]);          
                    var newPhoneNumber = ConvetToCanonicalForm(commandParams[1]);

                    var entriesChangedCount = PhonebookRepository.ChangePhone(oldPhoneNumber, newPhoneNumber);
                    commandResult = string.Format("{0} numbers changed", entriesChangedCount);
                    return commandResult;
                }
                default:
                {
                    try
                    {
                        IEnumerable<PhonebookEntry> entries =
                            PhonebookRepository.ListEntries(int.Parse(commandParams[0]), int.Parse(commandParams[1]));
                        foreach (var entry in entries)
                        {
                            return entry.ToString();
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return "Invalid range";
                    }
                }
                    break;
            }
            return;
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

        private static void AppendOutput(string text)
        {
            Output.AppendLine(text);
        }
    }
}
