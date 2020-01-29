using System.Collections;
using System.Collections.Generic;
using System.Web;
using SocketIO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Data;
using System.Globalization;
using System;

public class GeneralProperties : MonoBehaviour
{
    public GameObject player;
    public SocketIOComponent socket;
    private string sGUID;
    public Stats playerStats;

    void Start()
    {
        sGUID = GetComponent<NetworkEntity>().GetSGUID();
        GetPlayerStats();
        // Pull on start
        // Derive on start
        // Fill UtilityEquipment
        // No classes for now
    }

    void Update ()
    {
        // Maybe nothing
    }

    public void GetPlayerStats()
    {
        StartCoroutine(GetPlayerStatsReq("http://btsdev.azurewebsites.net/WebService.asmx/GetPlayerStats?sGUID=" + sGUID));
    }

    IEnumerator GetPlayerStatsReq(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text.ToString();
                JSONObject top = new JSONObject(response.TrimStart('"').TrimEnd('"'));

                Debug.Log(top);

                //for (int i = 0; i < 40; i++)
                //{
                //    JSONObject test = new JSONObject(top[i].ToString().Substring(11, top[i].ToString().Length - 12));
                //    if (test[1].ToString().TrimStart('"').TrimEnd('"').Length > 0)
                //    {
                //        GameObject target = canvas.transform.GetChild(0).GetChild(i).gameObject;
                //        GameObject slotObj = Instantiate(copperOrderPrfab, canvas.transform);
                //        slotObj.transform.SetParent(target.transform, false);
                //    }
                //}




                //playerInventory = JsonConvert.DeserializeObject<List<InventorySlot>>(response);

                //for (int i = 0; i < 40; i++)
                //{
                //    if (playerInventory[i].ItemHash.Length > 0)
                //    {
                //        GameObject target = canvas.transform.GetChild(0).GetChild(i).gameObject;
                //        GameObject slotObj = Instantiate(copperOrderPrfab, canvas.transform);
                //        slotObj.transform.SetParent(target.transform, false);
                //    }

                //}

            }
        }
    }

    public class Stats
    {
        public int Maxhits { get; set; }
        public int Currenthits { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Spirit { get; set; }
    }
    
    public class Gathering
    {
        public int Mining { get; set; }
        public int Farming { get; set; }
        public int Fishing { get; set; }
        public int Woodcutting { get; set; }
        public int Hunting { get; set; }
    }

    public class Skills
    {
        public int Cooking { get; set; }
        public int Smithing { get; set; }
        public int Leatherworking { get; set; }
        public int Tailoring { get; set; }
        public int Imbuing { get; set; }
        public int Construction { get; set; }
        public int Carpentry { get; set; }
        public int Alchemy { get; set; }
    }


    public List<string> utilityEquipment;  // Populated to check for inventory
    public List<string> utilityAccess;     // Populated to check for equipped items


    /*
     
     Derived Real Bonus and Stats

        Attack power
        spell power
        spell crit
        dodge
        block
        haste

     
     
     */

}
