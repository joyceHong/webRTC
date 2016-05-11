using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            writeEvent writeObj = new writeEvent();
            try
            {
                DataTable dt = new DataTable();
                ClassLibrary2.Startup aa = new ClassLibrary2.Startup();
                dt = aa.test();

                MongoDB saveData = new MongoDB();
                saveData.save(dt);
                //string strJsonFile = JsonConvert.SerializeObject(dt);
                //string path = Directory.GetCurrentDirectory();
                //writeObj.writeToFile("tempFile.txt", path, strJsonFile);    
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile(ex.Message.ToString());
            }
        }
    }
}
