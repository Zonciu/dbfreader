using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class Program
    {
        const string badData = @"C:\Users\Matt\Downloads\Abel's North_20160611 - bad data\EMP.DBF";
        const string goodData = @"C:\Users\Matt\Downloads\Abel's North_20150422\EMP.DBF";

        public static void Main(string[] args)
        {
            var table = DbfReader.DbfReader.OpenTable(badData);
            //var table = DbfReader.DbfReader.OpenTable(goodData);

            foreach (var row in table) {
                Console.WriteLine("{0}: {1} {2}", row["ID"].GetInt(), row["FIRSTNAME"].GetString(), row["LASTNAME"].GetString());
            }

            //readAllDbfs();

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        static void readAllDbfs()
        {
            foreach (var dbf in Directory.GetFiles(@"C:\Users\Matt\Downloads\Abel's North_20160611 - bad data", "*.dbf")) {
                Console.WriteLine("opening " + dbf);
                DbfReader.DbfReader.OpenTable(dbf);
            }
        }
    }
}
