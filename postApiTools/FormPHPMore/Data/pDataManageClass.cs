﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// data数据库管理类
/// </summary>
namespace postApiTools.FormPHPMore.Data
{
    public class pDataManageClass
    {
        public static string dbPath = Config.dataPath + "data-manage.db";

        public static lib.pSqlite sqlite = new lib.pSqlite(dbPath);

        /// <summary>
        /// 错误信息
        /// </summary>
        public string error = "";

        /// <summary>
        /// 存储数据库表名
        /// </summary>
        public static string dataTable = "data_config";

        /// <summary>
        /// hash
        /// </summary>
        public string hash = "";

        /// <summary>
        /// 实例化
        /// </summary>
        public pDataManageClass()
        {
            sqlite.executeNonQuery("CREATE TABLE IF NOT EXISTS " + dataTable + "(hash varchar(200),name varchar(200),type  varchar(200), path varchar(200), ip varchar(2000),port varchar(200),username varchar(20000),password varchar(200),addtime integer);");//创建详情表
        }

        /// <summary>
        /// 添加sqlite到配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool addSqliteDataBase(string name, string path)
        {
            string hash = lib.pBase.getHash();
            this.hash = hash;
            string sql = string.Format("insert into {0} (hash,name,type,path,addtime)values('{1}','{2}','{3}','{4}','{5}')", dataTable, hash, name, DataBaseType.Sqlite, path, lib.pDate.getTimeStamp());
            bool b = sqlite.executeNonQuery(sql) > 0 ? true : false;
            error = sqlite.error;
            return b;
        }


        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, object> getDataBaseAll()
        {
            Dictionary<int, object> d = sqlite.getRows(string.Format("select *from {0}", dataTable));
            error = sqlite.error;
            return d;
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, object> getSqliteTable(string path)
        {
            Dictionary<int, object> list = new Dictionary<int, object> { };
            lib.pSqlite p = new lib.pSqlite(path);
            DataTable data = p.getTable();
            p.close();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Dictionary<string, string> d = new Dictionary<string, string> { };
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    d.Add(data.Columns[j].ToString(), data.Rows[i][j].ToString());
                }
                list.Add(i, d);
            }
            return list;
        }


       

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public Dictionary<string, string> getDataBaseHash(string hash)
        {
            return sqlite.getOne(string.Format("select *from {0} where hash='{1}'", dataTable, hash));
        }

    }
}
