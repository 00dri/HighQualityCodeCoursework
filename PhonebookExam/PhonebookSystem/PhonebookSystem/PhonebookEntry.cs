namespace PhonebookSystem
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class PhonebookEntry : IComparable<PhonebookEntry>
    {
        public PhonebookEntry()
        {
            this.PhoneNumbers = new SortedSet<string>();
        }

        public string Name { get; set; }

        public SortedSet<string> PhoneNumbers { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');

            sb.Append(this.Name);
            var flag = true;

            foreach (var phone in this.PhoneNumbers)
            {
                if (flag)
                {
                    sb.Append(": ");
                    flag = false;
                }
                else
                {
                    sb.Append(", ");
                }

                sb.Append(phone);
            }

            sb.Append(']');
            return sb.ToString();
        }

        public int CompareTo(PhonebookEntry other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}