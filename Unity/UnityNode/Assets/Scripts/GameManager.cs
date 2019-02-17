using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SocketIO;

public class GameManager : MonoBehaviour
{
    public SocketIOComponent socket;
    public Text uField;
    public Text pField;
    public GameObject passField;

    void Start()
    {
        passField.gameObject.GetComponent<InputField>().contentType = InputField.ContentType.Password;
    }

    public void Authenticate()
    {
        var user = uField.text;
        var pass = passField.gameObject.GetComponent<InputField>().text;
        //Debug.Log("Auth test: " + user + " - " + pass);

        socket.Emit("auth", new JSONObject(string.Format(@"{{""user"":""{0}"", ""pass"":""{1}""}}", user, pass)));
    }

    public void GoToScene ()
    {
        SceneManager.LoadScene(1);
    }
}
