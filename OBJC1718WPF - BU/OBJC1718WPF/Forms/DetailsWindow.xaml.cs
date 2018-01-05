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
using System.Windows.Shapes;

namespace StudentManager
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
            private DBManager dBManager;
            private RuntimeTempData tempData = new RuntimeTempData();

        public DetailsWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
        }
    }
}
