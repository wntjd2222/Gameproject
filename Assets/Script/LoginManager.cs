using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("LoginPanel")]
    public InputField IDInputField;
    public InputField PWInputFiled;

    [Header("CreateAccountPanel")]
    public InputField New_IDInputField;
    public InputField New_PWInputField;

   // public string LoginUrl = "http://localhost/EquipmentGame/addequipment.php?";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginBtn()
    {
        SceneManager.LoadScene("main");
        //StartCoroutine(LoginCo());
    }

    /*
    IEnumerator LoginCo()
    {
        Debug.Log(IDInputField.text);
        Debug.Log(IDInputField.text);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", IDInputField.text);
        form.AddField("Input_pass", PWInputFiled.text);

        WWW webRequest = new WWW(LoginUrl, form);
        yield return webRequest;

        Debug.Log(webRequest.text);
        
    }
    */

    public void CreateAccountBtn()
    {
        //StartCoroutine(CreateAccountCo);
    }

    IEnumerator CreateAccountCo()
    {
        yield return null;
    }
}
