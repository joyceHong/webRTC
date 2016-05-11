using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ClassLibrary1
{
    public class entityPatient : commonDB
    {
        /// <summary>
        /// 病患姓名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 上次就診日
        /// </summary>
        public string LastViewClinicDate
        {
            get;
            set;
        }

        /// <summary>
        /// 生日
        /// </summary>
        public string BirthDay
        {
            get;
            set;
        }

        /// <summary>
        /// 取得所有病患
        /// </summary>
        /// <returns></returns>
        public IList<entityPatient> getAllPatients()
        {
            try
            {

                IList<entityPatient> listAllPatients = new List<entityPatient>();
                DataTable dt = commonDB.selectQueryWithDataTable("select 病患姓名,出生日期,身份證號,上次就診日 from patient");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        listAllPatients.Add(new entityPatient()
                        {
                            Name = dr["病患姓名"].ToString(),
                            BirthDay = dr["出生日期"].ToString(),
                            LastViewClinicDate = dr["上次就診日"].ToString(),
                        });
                    }
                }
                return listAllPatients;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
