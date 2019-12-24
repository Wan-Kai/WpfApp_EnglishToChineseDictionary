using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using System.Text.RegularExpressions;

namespace WpfApp_EnglishToChineseDictionary
{
    class DataUtil
    {
        //// 定义接口
        //protected static IMongoDatabase _database;
        //// 定义客户端
        //protected static IMongoClient _client;

        
        private static string conn = "mongodb://localhost";
        //数据库名称
        private static string dbName = "Dictionary";
        //集合名称
        private static string colName = "User";
        private static string wordName = "Words";
        //连接服务端
        static MongoClient client = new MongoClient(conn);
        //获取指定数据库
        static IMongoDatabase db = client.GetDatabase(dbName);
        //获取指定集合   BsonDocument数据库文档对象

        private int Login_(string userName, string password)
        {
            //0 错误用户信息
            //1 普通用户
            //2 管理员
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(colName);
            
            string getPassword = "";
            string identityS = "";
            //创建约束生成器
            FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
            //约束条件
            FilterDefinition<BsonDocument> filter = builderFilter.Eq("userName", userName);
            //获取数据
            var result = coll.Find<BsonDocument>(filter).ToList();
            foreach (var item in result)
            {
                //取出整条值
                //Console.WriteLine(item.AsBsonValue);
                getPassword = item["password"].AsString;
                identityS = item["type"].AsString;
            }

            if (getPassword.Equals(password))
            {
                if (identityS.Equals("manager"))
                    return 2;
                return 1;
            }
            return 0;
        }
        public int Login(string userName, string password)
        {
            return Login_(userName, password);
        }

        private string GetWord_(string name)
        {
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(wordName);

            string explain = "";
            //创建约束生成器
            FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;

            if (IsChinese(name))
            {
                //约束条件
                FilterDefinition<BsonDocument> filter = builderFilter.Eq("Chinese", name);
                //获取数据
                var result = coll.Find<BsonDocument>(filter).ToList();
                foreach (var item in result)
                {
                    //取出整条值
                    //Console.WriteLine(item.AsBsonValue);
                    explain = item["English"].AsString;
                    explain += "?";
                    explain += item["paraphraseE"].AsString;
                }

                return explain;
            }
            else if (IsEnglish(name))
            {
                //约束条件
                FilterDefinition<BsonDocument> filter = builderFilter.Eq("English", name);
                //获取数据
                var result = coll.Find<BsonDocument>(filter).ToList();
                foreach (var item in result)
                {
                    //取出整条值
                    //Console.WriteLine(item.AsBsonValue);
                    explain = item["Chinese"].AsString;
                    explain += "?";
                    explain += item["paraphraseC"].AsString;
                }

                return explain;
            }
            return "";
        }
        public string GetWord(string name)
        {
            string explain = GetWord_(name);
            return explain;
        }

        private DataTable ViewData_()
        {
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(wordName);

            //创建生成器
            FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
            //排序生成器
            SortDefinitionBuilder<BsonDocument> builderSort = Builders<BsonDocument>.Sort;
            //排序约束   Ascending 正序    Descending 倒序
            SortDefinition<BsonDocument> sort = builderSort.Ascending("English");
            var result = coll.Find<BsonDocument>(builderFilter.Empty).Sort(sort).ToList();          

            //创建一个空表
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(String));
            dt.Columns.Add("Chinese", typeof(String));
            dt.Columns.Add("English", typeof(String));
            dt.Columns.Add("paraphraseC", typeof(String));
            dt.Columns.Add("paraphraseE", typeof(String));

            foreach (var item in result)
            {
                var id = item["_id"].ToString();
                dt.Rows.Add(id, item["Chinese"].AsString, item["English"].AsString, item["paraphraseC"].AsString, item["paraphraseE"].AsString);
            }

            return dt;
        }

        public DataTable ViewData()
        {
            return ViewData_();
        }

        public void UpdateData(string id,string Chinese,string English,string paraC,string paraE)
        {
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(wordName);

            var query = new BsonDocument("_id", new ObjectId(id));
            var dict = new Dictionary<string, object> {
                { "Chinese",Chinese},
                { "English",English},
                { "paraphraseC",paraC},
                { "paraphraseE",paraE}
              };
            var data = new BsonDocument(dict);
            coll.UpdateOne(query, new BsonDocument("$set", data));
        }

        public void AddData(string Chinese, string English, string paraC, string paraE)
        {
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(wordName);

            BsonDocument insertData = new BsonDocument
           {
               { "Chinese",Chinese},
               {"English",English },
               {"paraphraseC", paraC},
               {"paraphraseE",paraE }
           };
            coll.InsertOne(insertData);
        }

        public void DeleteData(string id)
        {
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(wordName);
            ObjectId _id = new ObjectId(id);
            coll.DeleteOne(new BsonDocument("_id", _id));
        }

        private bool check_(string Chinese, string English)
        {
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>(wordName);

            //创建约束生成器
            FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
            //约束条件
            FilterDefinition<BsonDocument> filter = builderFilter.Eq("Chinese", Chinese);
            //获取数据
            var result = coll.Find<BsonDocument>(filter).ToList();
            foreach (var item in result)
            {
                if (item["English"].AsString.Equals(English))
                    return false;
            }
            return true;
        }

        public bool check(string Chinese,string English)
        {
            if (check_(Chinese,English))
                return true;
            return false;
        }

        public bool IsChinese(string str)
        {
            Regex rx = new Regex("^[\u4e00-\u9fa5]$");
            for (int i = 0; i < str.Length; i++)
            {
                if (rx.IsMatch(str.Substring(i, 1)))
                    continue;
                else
                    return false;
            }
            return true;
        }

        public bool IsEnglish(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] <= 'Z' & str[i] >= 'A') || (str[i] <= 'z' & str[i] >= 'a'))
                    continue;
                else
                    return false;
            }
            return true;
        }
    }
}
   
