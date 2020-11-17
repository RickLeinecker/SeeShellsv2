#region copyright
// SeeShells Copyright (c) 2019-2020 Aleksandar Stoyanov, Bridget Woodye, Klayton Killough, 
// Richard Leinecker, Sara Frackiewicz, Yara As-Saidi
// SeeShells is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// SeeShells is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, see <https://www.gnu.org/licenses>
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeeShells.UI.Templates
{
    /// <summary>
    /// Interaction logic for CheckBoxWithLabel.xaml
    /// </summary>
    public partial class CheckBoxWithLabel : UserControl
    {

        public event EventHandler CheckClicked;
        public event EventHandler CheckUnclicked;
        public CheckBoxWithLabel()
        {
            InitializeComponent();
        }

        public string LabelContent
        {
            get { return box.Content.ToString(); }
            set
            {
                box.Content = value;
            }
        }

        public bool? IsChecked
        { 
            get { return box.IsChecked; }
        }

        private void box_Checked(object sender, RoutedEventArgs e)
        {
            CheckClicked?.Invoke(this, EventArgs.Empty);
        }

        private void box_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckUnclicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
