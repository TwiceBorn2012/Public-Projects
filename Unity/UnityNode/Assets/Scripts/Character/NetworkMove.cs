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
        // Send position to server
        //Debug.Log("sending position " + VectorToJSON(position));
        socket.Emit("move", new JSONObject(VectorToJSON(position)));
    }

    string VectorToJSON (Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }
}
