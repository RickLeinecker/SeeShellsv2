using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using System.Windows.Data;
using System.Globalization;

namespace SeeShellsV2.UI
{
    public class FilterControlViewVM : ViewModel, IFilterControlViewVM
    {
        [Dependency]
        public IUserCollection UserCollection { get; set; }

        [Dependency]
        public IRegistryHiveCollection RegistryHiveCollection { get; set; }

        public IShellEventCollection ShellEvents { get; private set; }

        public User User
        {
            get => user;
            set
            {
                User old = user;
                user = value;

                if (old != user)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
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

                NotifyPropertyChanged();
            }
        }

        public Type Type
        {
            get => type;
            set
            {
                Type old = type;
                type = value;

                if (old != type)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
            }
        }

        public string Path
        {
            get => path;
            set
            {
                string old = path;
                path = value;

                if (old != path)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
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

                NotifyPropertyChanged();
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

                NotifyPropertyChanged();
            }
        }

        private Type type = null;
        private User user = null;
        private RegistryHive registryHive = null;
        private DateTime? begin = null;
        private DateTime? end = null;
        private string path = null;

        public FilterControlViewVM([Dependency] IShellEventCollection shellEvents)
        {
            ShellEvents = shellEvents;
            ShellEvents.Filter += new FilterEventHandler(FilterType);
            ShellEvents.Filter += new FilterEventHandler(FilterPath);
            ShellEvents.Filter += new FilterEventHandler(FilterUser);
            ShellEvents.Filter += new FilterEventHandler(FilterRegistryHive);
            ShellEvents.Filter += new FilterEventHandler(FilterBeginDate);
            ShellEvents.Filter += new FilterEventHandler(FilterEndDate);
        }

        void FilterType(object o, FilterEventArgs e)
        {
            if (Type == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item.GetType() == Type;
        }

        void FilterPath(object o, FilterEventArgs e)
        {
            if (Path == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.Place != null && ((se.Place.PathName ?? string.Empty) + (se.Place.Name ?? string.Empty)).ToLower().StartsWith(Path.ToLower());
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

    internal class EventTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is IShellEventCollection shellEvents && values[1] is int count)
            {
                return shellEvents.Select(e => e.GetType()).Distinct();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
