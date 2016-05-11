using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Data;

namespace ClassLibrary2
{
    public class Startup
    {

        public DataTable test()
        {
            try
            {
                PublicInfo.Cooper_Path = "D:\\cooper";
                DataTable dt= ClassLibrary1.commonDB.selectQueryWithDataTable("select * from patient");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> Invoke(dynamic input)
        {
            if (input == null)
            {
                return "this is my first test node.js";
            }
            else
            {
                PublicInfo.Cooper_Path="d:\\Cooper";
                DataTable dt = commonDB.selectQueryWithDataTable("select * from doctor", false);
                return dt;
            }
        }
    }
}
