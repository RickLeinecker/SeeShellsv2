using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeShells.UI
{

    /**
     *Aggregates multiple messages (logs) together for later retrieval of a compilation
     */
    public class LogAggregator
    {
        private List<string> logs;
        private LogAggregator()
        {
            logs = new List<string>();
        }

        public static LogAggregator Instance { get; } = new LogAggregator();

        public void Add(string message)
        {
            logs.Add(message);
        }

        /**
         * Clears all the logs in the aggregator
         * Very dangerous because LogAggregator is a Singleton.
         * High chance of deleting unknown logs.
         */
        public void Clear()
        {
            logs.Clear();
        }

        protected string CompileMessage()
        {

            StringBuilder displayMessage = new StringBuilder();
            logs.ForEach(log => displayMessage.AppendLine(log));

            return displayMessage.ToString();
        }

        /**
         * Shows all added log messages in a <seealso cref="MessageBox"/>
         * Will not display the messagebox if no logs have been recorded
         * Clears the accumulated logs afterwards.
         */
        public void ShowIfNotEmpty()
        {
            if (logs.Count == 0)
            {
                return;
            }

            MessageBox.Show(CompileMessage(), "SeeShells", MessageBoxButton.OK, MessageBoxImage.Information);
            Clear();
        }
    }
}
