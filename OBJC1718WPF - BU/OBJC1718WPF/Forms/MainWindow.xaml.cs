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
    //Delegates
    public delegate bool CheckFieldInputAllLetter(string input);
    public delegate bool CheckFieldInputAllNumber(string input);
    public delegate bool CheckFieldInputNumberOrLetterShort(string input);
    public delegate bool CheckFieldInputDate(string input);
    public delegate bool CheckFieldInput(string input);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DBManager dBManager = new DBManager();

        public MainWindow()
        {
            InitializeComponent();
            StudentTab.DataContext = dBManager.Students;
            LecturerTab.DataContext = dBManager.Lecturers;
            CourseTab.DataContext = dBManager.Courses;
        }

        private void AddStudentMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow(dBManager);
            addStudentWindow.ShowDialog();
        }

        private void AddCourseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddCourseWindow addCourseWindow = new AddCourseWindow(dBManager);
            addCourseWindow.ShowDialog();
        }
    }
}
