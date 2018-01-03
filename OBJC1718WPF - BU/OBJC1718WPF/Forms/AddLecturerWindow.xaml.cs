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
    /// Interaktionslogik des Hinzufügen Dialogs
    /// </summary>
    public partial class AddLecturerWindow : Window
    {
        private DBManager dBManager;

        public AddLecturerWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            //CourseComboBox.ItemsSource = dBManager.Courses;
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dBManager.AddLecturer(
                FirstnameTextbox.Text,
                LastnameTextbox.Text,
                BirthdateDatepicker.SelectedDate,
                (Degree)DegreeComboBox.SelectedItem,
                StreetTextbox.Text,
                HouseNumberTextbox.Text,
                ZIPTextbox.Text,
                CityTextbox.Text);
            }
            catch (Exception)
            {
                throw;
            }
            Close();
            
        }
    }
}
