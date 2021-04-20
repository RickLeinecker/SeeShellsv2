using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ShellEvent : IShellEvent
    {
        public string TypeName { get; init; }
        public string Description { get; init; }
        public DateTime TimeStamp { get; init; }
        public User User { get; init; }
        public Place Place { get; init; }
        public IEnumerable<IShellItem> Evidence { get; init; }
        public RegistryHive RegistryHive { get => Evidence.FirstOrDefault()?.RegistryHive ?? null; }

        public virtual string LongDescription => LongDescriptionPattern
            .Replace("{TYPENAME}", TypeName)
            .Replace("{USER}", User.Name)
            .Replace("{PLACE}", "\"" + Place.Name + "\" (" + Place.Type + ")")
            .Replace("{TIMESTAMP}", TimeStamp.ToString());

        /// <summary>
        /// A pattern for creating a LongDescription. Use {USER}, {PLACE}, {TYPENAME}, and {TIMESTAMP} to insert field strings into
        /// the description returned by LongDescription { get; }
        /// </summary>
        public virtual string LongDescriptionPattern => "A {TYPENAME} event, providing evidence that {USER} interacted with {PLACE} at {TIMESTAMP}.";

        public int CompareTo(IShellEvent other)
        {
            return (TimeStamp, User, Place, TypeName).CompareTo((other.TimeStamp, other.User, other.Place, other.TypeName));
        }
    }
}
