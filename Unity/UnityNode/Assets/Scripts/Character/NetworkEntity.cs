using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Web;

public class NetworkEntity : MonoBehaviour
{
    public static string SGUID = "";

    public void UpdateSGUID(string sGUID)
    {
        SGUID = sGUID.Replace("\"","");
    }

    public string GetSGUID()
    {
        return SGUID;
    }
}
