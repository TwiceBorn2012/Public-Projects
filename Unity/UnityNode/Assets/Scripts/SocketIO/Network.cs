using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Network : MonoBehaviour
{

    public static SocketIOComponent socket;

    public GameObject playerPrefab;

    public GameObject myPlayer;

    Dictionary<string, GameObject> players;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnect);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
        players = new Dictionary<string, GameObject>();
    }

    void OnMove(SocketIOEvent obj)
    {
        Debug.Log("player is moving:" + obj.data);
        var position = new Vector3(GetFloatFromJSON(obj.data, "x"), 0, GetFloatFromJSON(obj.data, "y"));

        var player = players[obj.data["id"].ToString()];
        
        var navigatePos = player.GetComponent<NavigatePosition>();
        navigatePos.NavigatTo(position);

        
    }

    void OnConnected(SocketIOEvent e)
    {
        //Debug.Log("connected");
    }

    void OnDisconnect(SocketIOEvent e)
    {
        var id = e.data["id"].ToString();
        Debug.Log("Player disconnected id: " + e.data["id"]);
        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }

    void OnUpdatePosition(SocketIOEvent e)
    {
        var position = new Vector3(GetFloatFromJSON(e.data, "x"), 0, GetFloatFromJSON(e.data, "y"));

        var player = players[e.data["id"].ToString()];
        player.transform.position = position;
    }

    void OnRequestPosition(SocketIOEvent e)
    {
        socket.Emit("updatePosition", new JSONObject(Network.VectorToJSON(myPlayer.transform.position)));
    }

    void OnSpawned (SocketIOEvent e)
    {
        Debug.Log("Spawned " + e.data["id"]);
        var player = Instantiate(playerPrefab);

        players.Add(e.data["id"].ToString(), player);
    }

    public static float GetFloatFromJSON(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    public static string VectorToJSON(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }
}
