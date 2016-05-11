using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MongoDB.Driver;
using MongoDB.Bson;
namespace ConsoleApplication1
{
    class MongoDB
    {
        public void save(DataTable dt)
        {
            try
            {
                string connString = "mongodb://192.168.1.63:27017/cooper";
                MongoClient _client = new MongoClient(connString);
                MongoServer _server = _client.GetServer();
                _server.Connect();
                // 連接到 db
                MongoDatabase db = _server.GetDatabase("cooper");
                MongoCollection collection = db.GetCollection("patient");
                
                foreach (DataRow dr in dt.Rows)
                {
                    BsonDocument inserData1 = new BsonDocument();
                    foreach (DataColumn column in dt.Columns)
                    {
                        Dictionary<string, object> keyValue = new Dictionary<string, object>();
                        keyValue[column.ColumnName] = dr[column.ColumnName];
                        inserData1.AddRange(keyValue.ToBsonDocument());
                    }
                    collection.Insert(inserData1);
                }
                _server.Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
