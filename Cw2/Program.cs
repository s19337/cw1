//Zadanie 2 rozwiązanie

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cw2
{

    public class Student
    {
        public string Email { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string IndexNumber { get; set; }
        public string Birthdate { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string StudiesName { get; set; }
        public string StudiesMode { get; set; }

        public Student(string Imie, string Nazwisko, string IndexNumber, string Email, string Birthdate,
                          string MotherName, string FatherName, string StudiesName, string StudiesMode )
        {
            this.Imie = Imie;
            this.Nazwisko = Nazwisko;
            this.IndexNumber = IndexNumber;
            this.Email = Email;
            this.Birthdate = Birthdate;
            this.MotherName = MotherName;
            this.FatherName = FatherName;
            this.StudiesName = StudiesName;
            this.StudiesMode = StudiesMode;
        }

    }



    class Program
    {

        public static void ErrorLogging(string exMessage)
        {
            
            string logpath = @"Z:\APBD\cw2\ErrorLogging.txt";

            //file exist

            StreamWriter sw = File.AppendText(logpath);
            sw.WriteLine("logging errors");
            sw.WriteLine("Start " + DateTime.Now);
            sw.WriteLine("Error: " + exMessage);
            sw.WriteLine("End " + DateTime.Now);
            sw.Close();
        }


        static void Main(string[] args)
        {
            try
            {
                string csvpath =
                     Console.ReadLine(); 
                   // @"Z:\APBD\cw2\dane.csv";
                string xmlpath =
                     Console.ReadLine(); 
                    //@"Z:\APBD\cw2";
                string format =
                    Console.ReadLine();
                   // "xml";

                if (File.Exists(csvpath) && Directory.Exists(xmlpath))
                {

                    string textForLog = "";

                    string[] source = File.ReadAllLines(csvpath);

                    var studentsList = new List<Student>();
                    var uniqueStudents = new List<string>();
                    IDictionary<string, int> ActiveStudies = new Dictionary<string, int>();

                    string duplikaty = "";

                    for(int i=0; i<source.Length; i++)
                    {
                        string[] f = source[i].Split(',');

                        if (f.Length == 9)
                        {
                            if (!uniqueStudents.Contains(f[0] + f[1] + f[4]))
                            {
                                uniqueStudents.Add(f[0] + f[1] + f[4]);
                                studentsList.Add(new Student(f[0], f[1], f[4], f[6], f[5], f[7], f[8], f[2], f[3]));

                                if (ActiveStudies.ContainsKey(f[2]))
                                {
                                    ActiveStudies[f[2]] += 1;
                                }
                                else ActiveStudies.Add(f[2], 1);

                            }
                            else duplikaty += "[" + source[i] + "]" + "\n";
                            
                        }
                        else textForLog += "["+source+"]" + " - wrong row\n";
                    }

                    if(!duplikaty.Equals(""))
                    {
                        duplikaty = "\nDuplicate:\n" + duplikaty;
                        textForLog += duplikaty;
                    }



                    XElement xml = new XElement("uczelnia", 
                        new XAttribute("createAt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")),
                        new XAttribute ("author", "Nelia Kostei"),

                        new XElement("studenci",

                          from str in studentsList

                          select new XElement("student",

                            new XAttribute("indexNumber", "s" + str.IndexNumber),
                            new XElement("fname", str.Imie),
                            new XElement("lname", str.Nazwisko),
                            new XElement("birthdate", str.Birthdate),
                            new XElement("email", str.Email),
                            new XElement("motherName", str.MotherName),
                            new XElement("fatherName", str.FatherName),
                            new XElement("studies",
                                new XElement("name", str.StudiesName),
                                new XElement("mode", str.StudiesMode)
                                )
                            )
                        ),

                        new XStreamingElement("activeStudies",

                        from it in ActiveStudies

                        select new XElement("studies" ,
                               new XAttribute ("name", it.Key),
                               new XAttribute ("numberOfStudents", it.Value) )  )                   

                    );

                   if(!textForLog.Equals("")) ErrorLogging(textForLog);
                    xml.Save(String.Concat(xmlpath + "result.xml"));

                }
                else
                {

                    if (!File.Exists(csvpath))
                    {
                        throw new Exception("File dosent exist");
                    }

                    if (!File.Exists(xmlpath))
                    {
                        throw new Exception("Directory dosent exist");
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex.Message);
            }


        }
    }
}
