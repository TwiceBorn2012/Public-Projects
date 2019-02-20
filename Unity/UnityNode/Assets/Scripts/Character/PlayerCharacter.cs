using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Data;
using Newtonsoft.Json;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject player;
    public SocketIOComponent socket;
    public static float brakingDistance = 1.5f;
    public List<InventorySlot> playerInventory ;
    public static string playerAccount = "liy2v346Af13";
    public Text text;
    public Canvas canvas;
    public GameObject copperOrderPrfab; 

    void Start()
    {
        UpdateInventory();
    }

    public void AddPlayerItemToInv (string gameItem)
    {
        socket.Emit("addPlayerItemToInv", new JSONObject(string.Format(@"{{""id"":""{0}"", ""aid"":""{1}""}}", gameItem, playerAccount)));

    }

    public void UpdateInventory()
    {
        //socket.Emit("updatePlayerInventory", new JSONObject(string.Format(@"{{""id"":""{0}""}}", playerAccount)));
        StartCoroutine(GetRequest("http://btsdev.azurewebsites.net/WebService.asmx/UpdatePlayerInventory?aid=" + playerAccount));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                playerInventory = JsonConvert.DeserializeObject<List<InventorySlot>>(response);

                for (int i = 0; i < 40; i++)
                {
                    if (playerInventory[i].ItemHash.Length > 0)
                    {
                        GameObject target = canvas.transform.GetChild(0).GetChild(i).gameObject;
                        GameObject slotObj = Instantiate(copperOrderPrfab, canvas.transform);
                        slotObj.transform.SetParent(target.transform, false);
                    }

                }
                
                //JavaScriptSerializer js = new JavaScriptSerializer();
                //Person[] persons = js.Deserialize<Person[]>(json);

                //string response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                //Debug.Log(JsonConvert.DeserializeObject<InventorySlot>(response));

                //JSONObject invJSON = new JSONObject(webRequest.downloadHandler.text);
                //Debug.Log(invJSON);
                //

                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                //playerInventory = webRequest.downloadHandler.data;
            }
        }
    }


    public class Account
    {
        public string AccountInt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class InventorySlot
    {
        public string InvSlot { get; set; }
        public string ItemHash { get; set; }
        public string ItemStackInt { get; set; }

    }
}