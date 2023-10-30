using System.Collections;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;

public class Usingmysql : MonoBehaviour
{
    public static MySqlConnection SqlConn;

    static string ipAddress = "192.168.25.44";
    static string db_id = "root";
    static string db_pw = "0117";
    static string db_name = "equipment";

    string strConn = string.Format("server={0}; uid={1}; pwd={2}; database={3}; charset=utf8;", ipAddress, db_id, db_pw, db_name);

    private void Awake()
    {
        try
        {
            SqlConn = new MySqlConnection(strConn);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string query = "select * from equipment";
        DataSet ds = OnSelectRequest(query, "equipment");

        Debug.Log(ds.GetXml());
    }

    public static bool OnInsertOrUpdateRequest(string str_query)
    {
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = SqlConn;
            sqlCommand.CommandText = str_query;

            SqlConn.Open();

            sqlCommand.ExecuteNonQuery();
            SqlConn.Close();

            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    public static DataSet OnSelectRequest(string p_query, string table_name)
    {
        try
        {
            SqlConn.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = SqlConn;
            cmd.CommandText = p_query;

            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sd.Fill(ds, table_name);

            SqlConn.Close();

            return ds;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            return null;
        }
    }

    private void OnApplicationQuit()
    {
        SqlConn.Close();
    }

    /*
    public void sqlcmdall(string allcmd)
    { 
        sqlConnect();

        MySqlCommand dbcmd = new MySqlCommand(allcmd, sqlconn);
        dbcmd.ExecuteNonQuery();

        sqldisConnect();
    }

    public DataTable selsql(string sqlcmd) 
    {
        DataTable dt = new DataTable();

        sqlConnect();
        MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmd, sqlconn);
        adapter.Fill(dt);
        sqldisConnect();

        return dt; 
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
}
