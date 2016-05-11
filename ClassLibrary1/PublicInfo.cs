using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class PublicInfo
    {
        public static string Cooper_Path = "";

        public static void CheckClientSide()
        {
            try
            {
                String strFile = Path.Combine(Directory.GetCurrentDirectory(), "CP_SERVER.txt");
                if (File.Exists(strFile))
                {
                    Cooper_Path = System.IO.File.ReadAllText(strFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
