using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class equiplist : MonoBehaviour
{
    
    private string secretKey = "LJS";
    public string addequipmentsURL = "http://localhost/EquipmentGame/addequipment.php?";
    public string bestequipURL = "http://localhost/EquipmentGame/display.php";
    public Text nameTextInput;
    public Text powerTextInput;
    public Text healthTextInput;
    public Text nameResultText;
    public Text powerResultText;
    public Text healthResultText;
    public Text updatenameTextInput;
    
    public float atkDmg = 50.0f;
    public float maxHp = 50.0f;

    public void GetequipBtn()
    {
        nameResultText.text = "Name: \n \n";
        powerResultText.text = "Power: \n \n";
        healthResultText.text = "Health: \n \n";
        StartCoroutine(Getequips());
    }

    public void SendequipBtn()
    {
        StartCoroutine(Postequips(nameTextInput.text, Convert.ToInt32(powerTextInput.text), Convert.ToInt32(healthTextInput.text)));
        nameTextInput.gameObject.transform.parent.GetComponent<InputField>().text = "";
        powerTextInput.gameObject.transform.parent.GetComponent<InputField>().text = "";
        healthTextInput.gameObject.transform.parent.GetComponent<InputField>().text = "";
    }
    IEnumerator Getequips()
    {
        UnityWebRequest be_get = UnityWebRequest.Get(bestequipURL);
        yield return be_get.SendWebRequest();
        if (be_get.error != null)
            Debug.Log("There was an error getting the best equipment: " + be_get.error);
        else
        {
            string dataText = be_get.downloadHandler.text;
            MatchCollection mc = Regex.Matches(dataText, @"_");
            if(mc.Count > 0)
            {
                string[] splitData = Regex.Split(dataText, @"_");
                for (int i = 0; i< mc.Count; i++)
                {
                    if (i % 3 == 0)
                        nameResultText.text += splitData[i];
                    else if (i % 3 == 1)
                        powerResultText.text += splitData[i];
                    else
                        healthResultText.text += splitData[i];
                }
            }
        }
    }
    
    IEnumerator Postequips(string name, float power, float health)
    {
        string hash = HashInput(name + power + health + secretKey);
        string post_url = addequipmentsURL + "name=" + UnityWebRequest.EscapeURL(name) 
            + "&power=" + power + "&health" + health + "&hash=" + hash;
        UnityWebRequest be_post = UnityWebRequest.PostWwwForm(post_url, hash);
        yield return be_post.SendWebRequest();
        if(be_post.error != null)
        {
            Debug.Log("There was an error posting the equipments: " + be_post.error);
        }
    }
    

    public string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }
    
    public void UpdateequipBtn()
    {
        // ���� �ٸ� ������ mysql ���ӹ�
        try
        {
            MySqlConnection connection = new MySqlConnection("Server = localhost;Database=login;Uid=root;Pwd=ljss0117;");

            connection.Open();

            string equipname = updatenameTextInput.text;
            string selectQuery = "SELECT * FROM equipment WHERE name = \'" + equipname + "\' ";

            MySqlCommand Selectcommand = new MySqlCommand(selectQuery, connection);

            MySqlDataReader equiplist = Selectcommand.ExecuteReader();

            while (equiplist.Read())
            {
                if (equipname == (string)equiplist["name"])
                {
                    atkDmg = (float)equiplist["power"];
                    maxHp = (float)equiplist["health"];
                }
            }
            connection.Close();


        }
        catch (Exception ex)
        {
            SceneManager.LoadScene("Error");
        }
    }
    

    public void ReturnToGame()
    {
        SceneManager.LoadScene("main");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
