using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkMove : MonoBehaviour
{

    public SocketIOComponent socket;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnMove (Vector3 position)
    {
        socket.Emit("move", new JSONObject(Network.VectorToJSON(position)));
        Debug.Log("player is moving" + Network.VectorToJSON(position));
        
    }

}
