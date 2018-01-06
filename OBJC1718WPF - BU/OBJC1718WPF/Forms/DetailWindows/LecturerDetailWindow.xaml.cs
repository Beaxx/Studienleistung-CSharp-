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
    /// Interaction logic for LecturerDetailWindow.xaml
    /// </summary>
    public partial class LecturerDetailWindow : Window
    {
            private DBManager dBManager;
            private RuntimeTempData tempData = new RuntimeTempData();

        public LecturerDetailWindow(DBManager dBManager, Lecturer member)
        {
            this.dBManager = dBManager;
            DataContext = member;

            InitializeComponent();
            Title = member.ToString();
            FirstnameTextbox.Text = member.FirstName;
            LastnameTextbox.Text = member.LastName;
            BirthdateDatepPicker.DisplayDate = member.Birthdate;
            StreetTextbox.Text = member.Street;
            HouseNumberTextbox.Text = member.HouseNumber;
            ZIPTextbox.Text = member.Zip.ToString();

            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            DegreeComboBox.SelectedItem = member.Degree;

            CourseComboBox.ItemsSource = dBManager.Courses;
            // TODO: LINQ Befehel, der eine Collection aus den Einträgen der Listens-Klasse Erstellt.
        }
    }
}
