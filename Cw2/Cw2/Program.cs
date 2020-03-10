using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cw2
{
    class Program
    {

        public static void ErrorLogging(Exception ex)
        {
            string logpath = @"Z:\APBD\cw2";

            //file exist

            StreamWriter sw = File.AppendText(logpath);
            sw.WriteLine("logging errors");
            sw.WriteLine("Start "+DateTime.Now);
            sw.WriteLine("Error: "+ ex.Message);
            sw.WriteLine("End " + DateTime.Now);
            sw.Close();
        }


        static void Main(string[] args)
        {


            try
            {

                

                string csvpath = Console.ReadLine(); // Z:\APBD\cw2\dane.csv
                string xmlpath = Console.ReadLine(); //Z:\APBD\cw2
                string format = Console.ReadLine(); //xml

                if (File.Exists(csvpath) && Directory.Exists(xmlpath))
                {

                    // Read into an array of strings.  
                    string[] source = File.ReadAllLines(csvpath);
                    XElement xml = new XElement("Uczelnia",
                        from str in source
                        let fields = str.Split(',')
                        select new XElement("studenci",
                            new XAttribute("StudemtID", "s" + fields[4]),
                            new XElement("fname", fields[0]),
                            new XElement("lname", fields[1]),
                            new XElement("birthdate", fields[5]),
                            new XElement("email", fields[6]),

                            new XElement("fatherName", fields[7]),
                            new XElement("motherName", fields[8]),

                            new XElement("studies",
                                new XElement("name", fields[2]),
                                new XElement("mode", fields[3])
                            )
                        )
                    );
                    xml.Save(String.Concat(xmlpath + "result.xml"));

                }
                else
                {

                    if(!File.Exists(csvpath))
                    {
                        throw new Exception("File dosent exist");
                    }

                    if (!File.Exists(xmlpath))
                    {
                        throw new Exception("Directory dosent exist");
                    }

                }
            } catch(Exception ex)
            {
                ErrorLogging(ex);
            }


        }
    }
}
