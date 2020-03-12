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
    public Equipment equipment;
    public EquipmentList equipmentList;
    public List<string> utilityEquipment;  // Populated to check for inventory
    public List<string> utilityAccess;

    void Start()
    {
        sGUID = GetComponent<NetworkEntity>().GetSGUID();
        GetPlayerStats();
        GetPlayerEquipment();
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
                playerStats.Dexterity = Int32.Parse(top["Decterity"].ToString());
                playerStats.Intelligence = Int32.Parse(top["Intelligence"].ToString());
                playerStats.Spirit = Int32.Parse(top["Spirit"].ToString());

                playerGathering.Mining = Int32.Parse(top["Mining"].ToString());
                playerGathering.Farming = Int32.Parse(top["Farming"].ToString());
                playerGathering.Fishing = Int32.Parse(top["Fishing"].ToString());
                playerGathering.Woodcutting = Int32.Parse(top["Woodcutting"].ToString());
                playerGathering.Hunting = Int32.Parse(top["Hunting"].ToString());

                playerSkills.Cooking = Int32.Parse(top["Cooking"].ToString());
                playerSkills.Smithing = Int32.Parse(top["Smithing"].ToString());
                playerSkills.Leatherworking = Int32.Parse(top["Leatherworking"].ToString());
                playerSkills.Tailoring = Int32.Parse(top["Tailoring"].ToString());
                playerSkills.Imbuing = Int32.Parse(top["Imbuing"].ToString());
                playerSkills.Construction = Int32.Parse(top["Construction"].ToString());
                playerSkills.Carpentry = Int32.Parse(top["Carpentry"].ToString());
                playerSkills.Alchemy = Int32.Parse(top["Alchemy"].ToString());

            }
        }
    }

    // Get Experience Totals


    public void GetPlayerEquipment()
    {
        StartCoroutine(GetPlayerEquipmentReq("http://btsdev.azurewebsites.net/WebService.asmx/GetPlayerEquipment?sGUID=" + sGUID));
    }

    IEnumerator GetPlayerEquipmentReq(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            //string[] pages = uri.Split('/');
            //int page = pages.Length - 1;

            //if (webRequest.isNetworkError)
            //{
            //    Debug.Log(pages[page] + ": Error: " + webRequest.error);
            //}
            //else
            //{
                //string response = webRequest.downloadHandler.text.ToString();

                //JSONObject top = new JSONObject(response.TrimStart('"').TrimEnd('"'));

                //playerStats.Maxhits = Int32.Parse(top["MaxHits"].ToString());
                //playerStats.Currenthits = Int32.Parse(top["CurrentHit"].ToString());
                //playerStats.Strength = Int32.Parse(top["Strength"].ToString());
                //playerStats.Dexterity = Int32.Parse(top["Decterity"].ToString());
                //playerStats.Intelligence = Int32.Parse(top["Intelligence"].ToString());
                //playerStats.Spirit = Int32.Parse(top["Spirit"].ToString());

                //playerGathering.Mining = Int32.Parse(top["Mining"].ToString());
                //playerGathering.Farming = Int32.Parse(top["Farming"].ToString());
                //playerGathering.Fishing = Int32.Parse(top["Fishing"].ToString());
                //playerGathering.Woodcutting = Int32.Parse(top["Woodcutting"].ToString());
                //playerGathering.Hunting = Int32.Parse(top["Hunting"].ToString());

                //playerSkills.Cooking = Int32.Parse(top["Cooking"].ToString());
                //playerSkills.Smithing = Int32.Parse(top["Smithing"].ToString());
                //playerSkills.Leatherworking = Int32.Parse(top["Leatherworking"].ToString());
                //playerSkills.Tailoring = Int32.Parse(top["Tailoring"].ToString());
                //playerSkills.Imbuing = Int32.Parse(top["Imbuing"].ToString());
                //playerSkills.Construction = Int32.Parse(top["Construction"].ToString());
                //playerSkills.Carpentry = Int32.Parse(top["Carpentry"].ToString());
                //playerSkills.Alchemy = Int32.Parse(top["Alchemy"].ToString());

            //}
        }
    }

    // Get Inventory Properies

    // Get Equipment Properties

    // Check Access and Utility List

    // Update Access and Utility List

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

    public class StatsTotals
    {
        public int Maxhits { get; set; }
        public int Currenthits { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Spirit { get; set; }
    }

    public class GatheringTotals
    {
        public int Mining { get; set; }
        public int Farming { get; set; }
        public int Fishing { get; set; }
        public int Woodcutting { get; set; }
        public int Hunting { get; set; }
    }

    public class SkillsTotals
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

    public class Equipment
    {
        public string Head1 { get; set; }
        public string Head2 { get; set; }
        public string Neck { get; set; }
        public string Back { get; set; }
        public string Shoulders { get; set; }
        public string Body { get; set; }
        public string Wrist1 { get; set; }
        public string Wrist2 { get; set; }
        public string Hands1 { get; set; }
        public string Hands2 { get; set; }
        public string Belt { get; set; }
        public string Legs1 { get; set; }
        public string Legs2 { get; set; }
        public string Boots1 { get; set; }
        public string Boots2 { get; set; }
        public string OneHandedWeapon1 { get; set; }
        public string OneHandedWeapon2 { get; set; }
        public string TwoHandedWeapon1 { get; set; }
        public string TwoHandedWeapon2 { get; set; }
        public string Accessory1 { get; set; }
        public string Accessory2 { get; set; }
    }

    public class EquipmentList
    {
        public string Name { get; set; }
        public int Slot { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
    }

        // Populated to check for equipped items

    /*
     
        Gets equipment

    SQL get the list
    For each, check with the SQL db for utility attributes

        Send to Get Properties
    

    */

    /*
     
     Derived Real Bonus and Stats

        Blunt
        Pierce
        Slashing

        Attack power
        spell power
        spell crit
        dodge
        block
        haste
        regen?

        Resistances? Elements
        Attack DMg for elements?
        SPell DMS for elements?

        
     
     */

}
