﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace StudentManager
{
    public class RuntimeTempData
    {
        /// <summary>
        /// Liste zur temporären Erfassung von Studenten im "AddStudent" Form.
        /// </summary
        public ObservableCollection<Student> StudentTempCollection { get; set; } = new ObservableCollection<Student>();

        /// <summary>
        /// Liste zur temporären Erfassung von Kursen im "AddCOurse" Form.
        /// </summary
        public ObservableCollection<Course> CourseTempCollection { get; set; } = new ObservableCollection<Course>();
    }
}