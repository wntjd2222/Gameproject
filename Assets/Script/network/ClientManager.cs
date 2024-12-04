using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public static class PacketNumber
{
    public const int CONNECTSUCCESS = 1000; // 서버연결
    public const int SIGNIN = 1313; // 로그인
    public const int CHAT = 8282; //채팅
    public const int H_COORDINATE = 3142; //좌표
    public const int SIGNUP = 4646; // 회원가입
    public const int NEWCLIENT = 7979; // 인원 추가
}

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;
    float packet_time;

    public UnityEngine.UI.Text chatBox;
    public UnityEngine.UI.Text curClient;
    public GameObject chatlist;

    [SerializeField] InputField ID;
    [SerializeField] InputField PW;

    [SerializeField] UnityEngine.UI.Text errorMessages;

    [SerializeField] Button SigninBtn;
    [SerializeField] Button SignupBtn;

    public const string IP = "127.0.0.1";
    public const int Port = 9000;

    public InputField textInput;
    public bool isChat;

    void Awake()
    {
        instance = this;
        packet_time = Time.time;

        Manager.ConnectServer(IP, Port);
    }
    void Update()
    {
        if (Manager.connection_state == Manager.ConnectionState.Disconnect)
        {
            curClient.text = "Disconnected.";

            if (Input.GetMouseButtonDown(0))
                Manager.ConnectServer(IP, Port);

        }

        isChat = textInput.isFocused;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (textInput.text != "")
            {
                KeySend();
                textInput.ActivateInputField();
            }
            else
            { textInput.Select(); }
        }
        
    }
    
    public void ClientChat(string text)
    {
        UnityEngine.UI.Text chatText = Instantiate(chatBox, chatlist.transform).GetComponent<UnityEngine.UI.Text>();
        chatText.text = text;

    }
            
    public void KeySend()
    {
        string a = textInput.text;
        PacketManager.Send(PacketNumber.CHAT, a);
        textInput.text = "";
    }

    public void SigninBtnClicked(object sender, EventArgs e)
    {
        SigninBtn.interactable = false;
        StartCoroutine(Signin());
    }

    IEnumerator Signin()
    {
        string IDdata = ID.text.Length + ID.text + PW.text.Length + PW.text;
        PacketManager.Send(PacketNumber.SIGNIN, IDdata);

        while (Manager.SigninSuccess == 0)
        {
            yield return null;
        }

        if (Manager.SigninSuccess == 1)
        {
            SceneManager.LoadScene("MainMenu");
            UnityEngine.Debug.Log("Load to Scene MainMenu");
        }
        
        SigninBtn.interactable = true;
    }

    /*
    public void SignupBtnClicked(object sender, EventArgs e)
    {
        SignupBtn.interactable = false;
        SceneManager.LoadScene("MainMenu");
    }
    */
}