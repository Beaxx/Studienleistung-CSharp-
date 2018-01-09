using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace StudentManager
{
    public class RuntimeTempData : INotifyPropertyChanged
    {
        /// <summary>
        /// Liste zur temporären Erfassung von Studenten im "AddStudent" Form.
        /// </summary
        public ObservableCollection<Student> StudentTempCollection { get; set; } = new ObservableCollection<Student>();

        /// <summary>
        /// Liste zur temporären Erfassung von Dozenten im "AdLecturer" Form.
        /// </summary
        public ObservableCollection<Lecturer> LecturerTempCollection { get; set; } = new ObservableCollection<Lecturer>();
       
        /// <summary>
        /// Liste zur temporären Erfassung von Kursen im "AddCOurse" Form.
        /// </summary
        public ObservableCollection<Course> CourseTempCollection { get; set; } = new ObservableCollection<Course>();

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
