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
    public partial class StudentDetailWindow : Window
    {
        private DBManager dBManager;
        private Student student;
        private RuntimeTempData tempData = new RuntimeTempData();

        public StudentDetailWindow(DBManager dBManager, Student member)
        {
            this.dBManager = dBManager;
            student = member;
            DataContext = member;

            InitializeComponent();
            Title = member.ToString();
            FirstnameTextbox.Text = member.FirstName;
            LastnameTextbox.Text = member.LastName;
            BirthdateDatepPicker.SelectedDate = member.Birthdate;
            StreetTextbox.Text = member.Street;
            HouseNumberTextbox.Text = member.HouseNumber;
            ZIPTextbox.Text = member.Zip.ToString();
            CityTextbox.Text = member.City;

            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            DegreeComboBox.SelectedItem = member.Degree;

            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            SemesterComboBox.SelectedItem = member.Semester;

            //Querry für Kursliste
            var enrolledCoursesID = from Listens in dBManager.Listens
                                    where (Listens.StudentID == student.ID)
                                    select Listens.CourseIDs;

            var enrolledCourses = from Course in dBManager.Courses
                                  where (enrolledCoursesID.Contains(Course.ID))
                                  select Course;

            CourseListbox.ItemsSource = enrolledCourses;
            CourseComboBox.ItemsSource = dBManager.Courses.Except(enrolledCourses);
        }


        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                student.FirstName = FirstnameTextbox.Text;
                student.LastName = LastnameTextbox.Text;
                student.Birthdate = (DateTime)BirthdateDatepPicker.SelectedDate;
                student.Degree = (Degree)DegreeComboBox.SelectedItem;
                student.Street = StreetTextbox.Text;
                student.HouseNumber = HouseNumberTextbox.Text;
                student.Zip = new DBManager.ZIP(ZIPTextbox.Text).Number;
                student.City = CityTextbox.Text;
                student.Semester = (Semester)SemesterComboBox.SelectedItem;

                // TODO: LINQ zuweisung über querry möglich?
                // für die in der lsite hinzugefügten
                //dBManager.JoinStudentAndCourse(student, );

            }
            catch (Exception)
            {
                throw;
            }

           

            Close();
        }
    }
}
