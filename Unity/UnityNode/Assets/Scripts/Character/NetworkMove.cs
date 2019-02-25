using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkMove : MonoBehaviour
{

    public SocketIOComponent socket;
    private string sGUID;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnMove (Vector3 position)
    {
        sGUID = GetComponent<NetworkEntity>().GetSGUID();
        //socket.Emit("move", new JSONObject(string.Format(@"{{""hash"":""{0}"", ""positionx"":""{1}"", ""positiony"":""{2}""}}", sGUID, position.x.ToString(), position.y.ToString())));
        socket.Emit("move", new JSONObject(string.Format(@"{{""hash"":""{0}"", ""positionx"":""{1}"", ""positiony"":""{2}"", ""positionz"":""{3}""}}", sGUID, position.x.ToString(), position.y.ToString(), position.z.ToString())));

    }

}
