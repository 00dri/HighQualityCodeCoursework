namespace PhonebookSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    internal class SlowPhonebookRepository : IPhonebookRepository
    {
        private List<PhonebookEntry> entries = new List<PhonebookEntry>();

        public bool AddPhone(string name, IEnumerable<string> nums)
        {
            var old = 
                from e in this.entries where e.Name.ToLowerInvariant() == name.ToLowerInvariant() select e;

            bool flag;
            
            if (old.Count() == 0)
            {
                var obj = new PhonebookEntry();
                obj.Name = name;

                foreach (var num in nums)
                {
                    obj.PhoneNumbers.Add(num);
                }

                this.entries.Add(obj);

                flag = true;
            }
            else if (old.Count() == 1)
            {
                var obj2 = old.First();
                foreach (var num in nums)
                {
                    obj2.PhoneNumbers.Add(num);
                }

                flag = false;
            }
            else
            {
                Console.WriteLine("Duplicated name in the phonebook found: " + name);
                return false;
            }

            return flag;
        }

        public int ChangePhone(string oldent, string newent)
        {
            var list = from e in this.entries where e.PhoneNumbers.Contains(oldent) select e;

            var nums = 0;
            foreach (var entry in list)
            {
                entry.PhoneNumbers.Remove(oldent);
                entry.PhoneNumbers.Add(newent);
                nums++;
            }

            return nums;
        }

        public PhonebookEntry[] ListEntries(int start, int num)
        {
            if (start < 0 || start + num > this.entries.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid start index or count.");
            }

            this.entries.Sort();
            var ent = new PhonebookEntry[num];

            for (var i = start; i <= start + num - 1; i++)
            {
                var entry = this.entries[i];
                ent[i - start] = entry;
            }

            return ent;
        }
    }
}