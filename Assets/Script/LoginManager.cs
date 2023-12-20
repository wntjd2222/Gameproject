using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class LoginManager : MonoBehaviour
{
    [Header("LoginPanel")]
    public InputField IDInputField;
    public InputField PWInputFiled;

    [Header("CreateAccountPanel")]
    public InputField New_IDInputField;
    public InputField New_PWInputField;

    public string LoginUrl = "http://localhost/EquipmentGame/addequipment.php?";

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void btn_login_Click(object sender, EventArgs e)
    {
        try
        {
            MySqlConnection connection = new MySqlConnection("Server = localhost;Database=login;Uid=root;Pwd=ljss0117;");

            connection.Open();

            int login_status = 0;
            string loginid = IDInputField.text;
            string loginpwd = PWInputFiled.text;
            string selectQuery = "SELECT * FROM login_info WHERE id = \'" + loginid + "\' ";

            MySqlCommand Selectcommand = new MySqlCommand(selectQuery, connection);

            MySqlDataReader userAccount = Selectcommand.ExecuteReader();

            while (userAccount.Read())
            {
                if (loginid == (string)userAccount["id"] && loginpwd == (string)userAccount["pwd"])
                {
                    login_status = 1;
                }
            }
            connection.Close();

            if(login_status == 1)
            {
                SceneManager.LoadScene("DBscene");
            }
            else
            {
                SceneManager.LoadScene("Error");
            }
            
        }
        catch(Exception ex)
        {
            SceneManager.LoadScene("Error");
        }
    }
    
    private void btn_newmember_Click(object sender, EventArgs e)
    {
        try
        {
            MySqlConnection connection = new MySqlConnection("Server = localhost;Database=login;Uid=root;Pwd=ljss0117;");

            connection.Open();

            string insertQuery = "INSERT INTO login_info (id, pwd) VALUES ('" + IDInputField.text + "', '" + PWInputFiled.text + "');";

            MySqlCommand command = new MySqlCommand(insertQuery, connection);

            if(command.ExecuteNonQuery() == 1)
            {
                connection.Close();
            }
            else
            {
                SceneManager.LoadScene("Error");
            }

        }
        catch (Exception ex)
        {
            SceneManager.LoadScene("Error");
        }
    }
}
