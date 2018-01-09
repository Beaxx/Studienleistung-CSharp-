using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    //public static class Validation
    //{
    //    public static bool CheckInputAllLetters(string input)
    //    {
    //        if (input.Length < 3)
    //            throw new ArgumentException("Namen müssen länger als drei Zeichen sein und \n dürfen ausschließlich Buchstaben enthalten.");
    //        else return true;
    //    }

    //    public static bool CheckMemberBirthdateInput(DateTime? input)
    //    {
    //        //Eingabeüberprüfung Datum
    //        if (input.Value.AddYears(14) >= DateTime.Today.Date)
    //            throw new ArgumentException("Studenten müssen mindestens 14 Jahre alt sein.");
    //        else return true;
    //    }
       
    //    public static bool CheckMemberZIPInput(string input)
    //    {
    //        //Eingabeüberprüfung Adresse
    //        if (input.Length != 5 | !input.All(Char.IsDigit))
    //            throw new ArgumentException("Bitte überprüfen Sie die Adresseingaben.");
    //        else return true;
    //    }

    //    public static bool CheckMemberHouseNumberInput(string input)
    //    {
    //        if (input.Length > 4 | !input.All(Char.IsLetterOrDigit))
    //            throw new ArgumentException("Hausnummer falsch");
    //        else return true;
    //    }

    //    public static bool CheckSemesterComboboxInput(Semester input)
    //    {
    //        if (input == 0)
    //            throw new ArgumentException("Bitte Semester Auswählen");
    //        else return true;
    //    }

    //    public static bool CheckDegreeComboboxInput(Degree input)
    //    {
    //        if (input == 0)
    //            throw new ArgumentException("Bitte Abschluss Auswählen");
    //        else return true;
    //    }

    //}
}
