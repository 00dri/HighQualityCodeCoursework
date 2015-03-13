namespace PhonebookSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Wintellect.PowerCollections;

    internal class PhonebookRepository : IPhonebookRepository
    {
        private OrderedSet<PhonebookEntry> sorted = new OrderedSet<PhonebookEntry>();

        private Dictionary<string, PhonebookEntry> dict = new Dictionary<string, PhonebookEntry>();

        private MultiDictionary<string, PhonebookEntry> multidict = new MultiDictionary<string, PhonebookEntry>(false);

        public bool AddPhone(string name, IEnumerable<string> nums)
        {
            var name2 = name.ToLowerInvariant();
            PhonebookEntry entry;
            var flag = !this.dict.TryGetValue(name2, out entry);

            if (flag)
            {
                entry = new PhonebookEntry();
                entry.Name = name;
                this.dict.Add(name2, entry);
                this.sorted.Add(entry);
            }

            foreach (var num in nums)
            {
                this.multidict.Add(num, entry);
            }

            entry.PhoneNumbers.UnionWith(nums);
            return flag;
        }

        public int ChangePhone(string oldent, string newent)
        {
            var found = this.multidict[oldent].ToList();
            foreach (var entry in found)
            {
                entry.PhoneNumbers.Remove(oldent);
                this.multidict.Remove(oldent, entry);
                entry.PhoneNumbers.Add(newent);
                this.multidict.Add(newent, entry);
            }

            return found.Count;
        }

        public PhonebookEntry[] ListEntries(int first, int num)
        {
            if (first < 0 || first + num > this.dict.Count)
            {
                Console.WriteLine("Invalid start index or count.");
                return null;
            }

            var list = new PhonebookEntry[num];

            for (var i = first; i <= first + num - 1; i++)
            {
                var entry = this.sorted[i];
                list[i - first] = entry;
            }

            return list;
        }
    }
}