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
        private Lecturer lecturer;
        private RuntimeTempData tempData = new RuntimeTempData();

        public LecturerDetailWindow(DBManager dBManager, Lecturer member)
        {
            this.dBManager = dBManager;
            lecturer = member;
            DataContext = member;

            InitializeComponent();
            Title = member.ToString();
            FirstnameTextbox.Text = member.FirstName;
            LastnameTextbox.Text = member.LastName;
            BirthdateDatepPicker.SelectedDate = member.Birthdate;
            StreetTextbox.Text = member.Street;
            HouseNumberTextbox.Text = member.HouseNumber;
            ZIPTextbox.Text = member.Zip.ToString();

            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            DegreeComboBox.SelectedItem = member.Degree;

            var holdingCoursesID = from Holds in dBManager.Holds
                                    where (Holds.LecturerID == lecturer.ID)
                                    select Holds.CourseIDs;

            var holdingCourses = from Course in dBManager.Courses
                                  where (holdingCoursesID.Contains(Course.ID))
                                  select Course;

            CourseListbox.ItemsSource = holdingCourses;
            CourseComboBox.ItemsSource = dBManager.Courses.Except(holdingCourses);
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lecturer.FirstName = FirstnameTextbox.Text;
                lecturer.LastName = LastnameTextbox.Text;
                lecturer.Birthdate = (DateTime)BirthdateDatepPicker.SelectedDate;
                lecturer.Degree = (Degree)DegreeComboBox.SelectedItem;
                lecturer.Street = StreetTextbox.Text;
                lecturer.HouseNumber = HouseNumberTextbox.Text;
                lecturer.Zip = new DBManager.ZIP(ZIPTextbox.Text).Number;
                lecturer.City = CityTextbox.Text;
                // TODO: Update der Listens
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
