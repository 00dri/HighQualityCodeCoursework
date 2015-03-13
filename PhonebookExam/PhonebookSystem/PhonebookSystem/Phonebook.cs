namespace PhonebookSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Contracts;

    internal class Phonebook
    {
        private const string DefaultCountryCode = "+359";

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
            if (commandName == "Cmd1") 
            {
                var str0 = commandParams[0];
                var str1 = commandParams.Skip(1).ToList();
                for (var i = 0; i < str1.Count; i++)
                {
                    str1[i] = Conv(str1[i]);
                }

                var flag = PhonebookRepository.AddPhone(str0, str1);
                if (flag)
                {
                    Print("Phone entry created.");
                }
                else
                {
                    Print("Phone entry merged");
                }
            }
            else if (commandName == "Cmd2") 
            {
                Print(PhonebookRepository.ChangePhone(Conv(commandParams[0]), Conv(commandParams[1])) + " numbers changed");
            }
            else 
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
        }

        private static string Conv(string num)
        {
            var sb = new StringBuilder();
            for (var i = 0; i <= Output.Length; i++)
            {
                sb.Clear();
                foreach (var ch in num)
                {
                    if (char.IsDigit(ch) || (ch == '+'))
                    {
                        sb.Append(ch);
                    }
                }

                if (sb.Length >= 2 && sb[0] == '0' && sb[1] == '0')
                {
                    sb.Remove(0, 1);
                    sb[0] = '+';
                }

                while (sb.Length > 0 && sb[0] == '0')
                {
                    sb.Remove(0, 1);
                }

                if (sb.Length > 0 && sb[0] != '+')
                {
                    sb.Insert(0, DefaultCountryCode);
                }

                sb.Clear();

                foreach (var ch in num)
                {
                    if (char.IsDigit(ch) || (ch == '+'))
                    {
                        sb.Append(ch);
                    }
                }

                if (sb.Length >= 2 && sb[0] == '0' && sb[1] == '0')
                {
                    sb.Remove(0, 1);
                    sb[0] = '+';
                }

                while (sb.Length > 0 && sb[0] == '0')
                {
                    sb.Remove(0, 1);
                }

                if (sb.Length > 0 && sb[0] != '+')
                {
                    sb.Insert(0, DefaultCountryCode);
                }

                sb.Clear();
                foreach (var ch in num)
                {
                    if (char.IsDigit(ch) || (ch == '+'))
                    {
                        sb.Append(ch);
                    }
                }

                if (sb.Length >= 2 && sb[0] == '0' && sb[1] == '0')
                {
                    sb.Remove(0, 1);
                    sb[0] = '+';
                }

                while (sb.Length > 0 && sb[0] == '0')
                {
                    sb.Remove(0, 1);
                }

                if (sb.Length > 0 && sb[0] != '+')
                {
                    sb.Insert(0, DefaultCountryCode);
                }
            }

            return sb.ToString();
        }

        private static void Print(string text)
        {
            Output.AppendLine(text);
        }
    }
}
