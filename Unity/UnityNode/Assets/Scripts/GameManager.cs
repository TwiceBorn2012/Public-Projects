using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SocketIO;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public SocketIOComponent socket;
    public Text uField;
    public Text pField;
    public GameObject passField;
    public GameObject netEnt;

    void Start()
    {
        passField.gameObject.GetComponent<InputField>().contentType = InputField.ContentType.Password;
    }

    public void AuthenticatePlayer()
    {
        var user = uField.text;
        var pass = passField.gameObject.GetComponent<InputField>().text;

        StartCoroutine(AuthenticatePlayerRequest("http://btsdev.azurewebsites.net/WebService.asmx/CheckUserAuth?user=" + user + "&password=" + pass));
    }

    IEnumerator AuthenticatePlayerRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                JSONObject response = new JSONObject(webRequest.downloadHandler.text);

                netEnt.GetComponent<NetworkEntity>().UpdateSGUID(response.ToString());

                GoToScene();

            }
        }
    }

    public void GoToScene ()
    {
        SceneManager.LoadScene(1);
    }
}
