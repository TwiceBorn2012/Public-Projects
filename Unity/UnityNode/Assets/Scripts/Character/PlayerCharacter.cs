using System.Collections;
using System.Collections.Generic;
using System.Web;
using SocketIO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Data;
using Newtonsoft.Json;
using System.Globalization;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject player;
    public SocketIOComponent socket;
    public static float brakingDistance = 1.5f;
    public List<InventorySlot> playerInventory ;
    public Text text;
    public Canvas canvas;
    public GameObject copperOrderPrfab;
    private float timer = 0.0f;
    private float waitTime = 5.0f;
    public string sGUID;

    void Start()
    {
        sGUID = GetComponent<NetworkEntity>().GetSGUID();
        UpdateInventory();
        GetPlayerStartPosition();
        // Emit player location to Server
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            UpdatePosition();
            Debug.Log("Stored position");
            timer = 0.0f;
        }
    }

    public void AddPlayerItemToInv (string gameItem)
    {
        socket.Emit("addPlayerItemToInv", new JSONObject(string.Format(@"{{""id"":""{0}"", ""sGUID"":""{1}""}}", gameItem, sGUID)));

    }

    public void UpdateInventory()
    {
        //socket.Emit("updatePlayerInventory", new JSONObject(string.Format(@"{{""id"":""{0}""}}", )));
        StartCoroutine(UpdateInventroyReq("http://btsdev.azurewebsites.net/WebService.asmx/UpdatePlayerInventory?sGUID=" + sGUID));
    }

    IEnumerator UpdateInventroyReq(string uri)
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
                Debug.Log(response);
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

    public void UpdatePosition()
    {
        //socket.Emit("updatePlayerInventory", new JSONObject(string.Format(@"{{""id"":""{0}""}}", )));
        Vector3 playerPos = player.transform.position;
        string playerPosX = HttpUtility.UrlEncode(string.Format("{0:N15}", playerPos.x));
        string playerPosY = HttpUtility.UrlEncode(string.Format("{0:N15}", playerPos.y));
        string playerPosZ = HttpUtility.UrlEncode(string.Format("{0:N15}", playerPos.z));
        StartCoroutine(UpdatePositionReq("http://btsdev.azurewebsites.net/WebService.asmx/UpdatePlayerPosition?sGUID=" + sGUID + "&positionX=" + playerPosX + "&positionY=" + playerPosY + "&positionZ=" + playerPosZ));
    }

    IEnumerator UpdatePositionReq(string uri)
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
                
            }
        }
    }

    public void GetPlayerStartPosition()
    {
        StartCoroutine(GetPlayerStartPositionReq("http://btsdev.azurewebsites.net/WebService.asmx/GetPlayerPosition?sGUID=" + sGUID));
    }

    IEnumerator GetPlayerStartPositionReq(string uri)
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
                JSONObject playerPosition = new JSONObject(webRequest.downloadHandler.text);

                NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
                agent.enabled = false;
                Vector3 startingPoint = new Vector3(float.Parse(playerPosition["x"].ToString().Replace("\"", "")), float.Parse(playerPosition["y"].ToString().Replace("\"", "")), float.Parse(playerPosition["z"].ToString().Replace("\"", "")));
                player.transform.position = startingPoint;
                agent.enabled = true;

                

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