using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using System.Windows.Data;

namespace SeeShellsV2.UI
{
    public class FilterControlViewVM : ViewModel, IFilterControlViewVM
    {
        [Dependency]
        public IDataRepository<User> UserCollection { get; set; }

        [Dependency]
        public IDataRepository<RegistryHive> RegistryHiveCollection { get; set; }

        private IShellEventCollection ShellEvents { get; set; }

        public User User
        {
            get => user;
            set
            {
                User old = user;
                user = value;

                if (old != user)
                    ShellEvents.FilteredView.Refresh();
            }
        }

        public RegistryHive RegistryHive
        {
            get => registryHive;
            set
            {
                RegistryHive old = registryHive;
                registryHive = value;

                if (old != registryHive)
                    ShellEvents.FilteredView.Refresh();
            }
        }

        public DateTime? Begin
        {
            get => begin;
            set
            {
                DateTime? old = begin;
                begin = value;

                if (old != begin)
                    ShellEvents.FilteredView.Refresh();
            }
        }

        public DateTime? End
        {
            get => end;
            set
            {
                DateTime? old = end;
                end = value;

                if (old != end)
                    ShellEvents.FilteredView.Refresh();
            }
        }

        private User user = null;
        private RegistryHive registryHive = null;
        private DateTime? begin = null;
        private DateTime? end = null;

        public FilterControlViewVM([Dependency] IShellEventCollection shellEvents)
        {
            ShellEvents = shellEvents;
            ShellEvents.Filter += new FilterEventHandler(FilterUser);
            ShellEvents.Filter += new FilterEventHandler(FilterRegistryHive);
            ShellEvents.Filter += new FilterEventHandler(FilterBeginDate);
            ShellEvents.Filter += new FilterEventHandler(FilterEndDate);
            Begin = null;
            End = null;
        }

        void FilterUser(object o, FilterEventArgs e)
        {
            if (User == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.User == User;
        }

        void FilterRegistryHive(object o, FilterEventArgs e)
        {
            if (RegistryHive == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.Evidence.Any() && se.Evidence.First().RegistryHive == RegistryHive;
        }

        void FilterBeginDate(object o, FilterEventArgs e)
        {
            if (Begin == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.TimeStamp >= Begin;
        }

        void FilterEndDate(object o, FilterEventArgs e)
        {
            if (End == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.TimeStamp <= End;
        }
    }
}
