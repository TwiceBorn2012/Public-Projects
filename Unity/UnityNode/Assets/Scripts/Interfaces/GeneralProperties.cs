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
    public Gathering playerGathering;
    public Skills playerSkills;

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

                playerStats.Maxhits = Int32.Parse(top["MaxHits"].ToString());
                playerStats.Currenthits = Int32.Parse(top["CurrentHit"].ToString());
                playerStats.Strength = Int32.Parse(top["Strength"].ToString());
                playerStats.Dexterity = Int32.Parse(top["Strength"].ToString());
                playerStats.Intelligence = Int32.Parse(top["Strength"].ToString());
                playerStats.Spirit = Int32.Parse(top["Strength"].ToString());

                playerGathering.Mining = Int32.Parse(top["Strength"].ToString());
                playerGathering.Farming = Int32.Parse(top["Strength"].ToString());
                playerGathering.Fishing = Int32.Parse(top["Strength"].ToString());
                playerGathering.Woodcutting = Int32.Parse(top["Strength"].ToString());
                playerGathering.Hunting = Int32.Parse(top["Strength"].ToString());

                playerSkills.Cooking = Int32.Parse(top["Strength"].ToString());
                playerSkills.Smithing = Int32.Parse(top["Strength"].ToString());
                playerSkills.Leatherworking = Int32.Parse(top["Strength"].ToString());
                playerSkills.Tailoring = Int32.Parse(top["Strength"].ToString());
                playerSkills.Imbuing = Int32.Parse(top["Strength"].ToString());
                playerSkills.Construction = Int32.Parse(top["Strength"].ToString());
                playerSkills.Carpentry = Int32.Parse(top["Strength"].ToString());
                playerSkills.Alchemy = Int32.Parse(top["Strength"].ToString());

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
