using System;
using System.Collections.Generic;

using SeeShellsV2.Data;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Repositories
{
    public class ShellEventCollection : ObservableSortedList<IShellEvent>, IShellEventCollection
    {
        public ShellEventCollection() : base(new ShellEventComparer()) { }
    }

    internal class ShellEventComparer : IComparer<IShellEvent>
    {
        public int Compare(IShellEvent a, IShellEvent b)
        {
            int datetime = DateTime.Compare(a.TimeStamp, b.TimeStamp);
            if (datetime != 0)
                return datetime;

            int username = string.Compare(a.User.Name, b.User.Name);
            if (username != 0)
                return username;

            int location = string.Compare(a.Place.Name, b.Place.Name);
            if (location != 0)
                return location;

            int typename = string.Compare(a.TypeName, b.TypeName);
            if (typename != 0)
                return typename;

            int description = string.Compare(a.Description, b.Description);
            if (description != 0)
                return description;

            return 0;
        }
    }
}
