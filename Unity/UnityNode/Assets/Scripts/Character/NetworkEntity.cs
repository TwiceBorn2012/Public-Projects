using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Web;

public class NetworkEntity : MonoBehaviour
{
    public static string SGUID = "";
    public static string playerUN = "";
    public static Vector3 startingPoint = new Vector3();


    public void UpdateSGUID(string sGUID)
    {
        SGUID = sGUID.Replace("\"","");
    }

    public string GetSGUID()
    {
        return SGUID;
    }

    public void UpdatePlayerUN(string un)
    {
        playerUN = un.Replace("\"", "");
    }

    public string GetPlayerUN()
    {
        return playerUN;
    }

    public void UpdatePlayerSP(Vector3 pos)
    {
        startingPoint = pos;
    }

    public Vector3 GetPlayerSP()
    {
        return startingPoint;
    }

}
