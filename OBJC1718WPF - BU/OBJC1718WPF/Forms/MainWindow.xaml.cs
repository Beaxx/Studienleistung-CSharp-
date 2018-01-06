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
using System.Data;

namespace StudentManager
{
    ////Delegates
    //public delegate bool CheckFieldInputAllLetter(string input);
    //public delegate bool CheckFieldInputAllNumber(string input);
    //public delegate bool CheckFieldInputNumberOrLetterShort(string input);
    //public delegate bool CheckFieldInputDate(string input);
    //public delegate bool CheckFieldInput(string input);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// DBManager Instanzierung zum Programstart
        /// </summary>
        public DBManager dBManager = DBManager.LoadFromDatabase();
        private RuntimeTempData tempData = new RuntimeTempData();

        public MainWindow()
        {
            InitializeComponent();
            StudentTab.DataContext = dBManager.Students;
            LecturerTab.DataContext = dBManager.Lecturers;
            CourseTab.DataContext = dBManager.Courses;
        }

        //Detail Button
        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var rowMember = ((FrameworkElement)sender).DataContext as dynamic;

            if (rowMember is Student)
            {
                StudentDetailWindow detailsWindow = new StudentDetailWindow(dBManager, rowMember);
                detailsWindow.ShowDialog();
            }

            if (rowMember is Lecturer)
            {
                LecturerDetailWindow detailsWindow = new LecturerDetailWindow(dBManager, rowMember);
                detailsWindow.ShowDialog();
            }

            if (rowMember is Course)
            {
                CourseDetailWindow detailsWindow = new CourseDetailWindow(dBManager, rowMember);
                detailsWindow.ShowDialog();
            }
        }

        //Studenten Inhalte
        private void AddStudentMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow(dBManager);
            addStudentWindow.ShowDialog();
        }


        //Dozenten Inhalte
        private void AddLecturerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddLecturerWindow addLecturer = new AddLecturerWindow(dBManager);
            addLecturer.ShowDialog();
        }

        //Kursinhalte
        private void AddCourseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddCourseWindow addCourseWindow = new AddCourseWindow(dBManager);
            addCourseWindow.ShowDialog();
        }

        //Fensster Schließen
        private void ExitApplicationMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dBManager.SaveToDatabase();
        }

        
    }
}
