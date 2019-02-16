using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Network : MonoBehaviour
{

    static SocketIOComponent socket;

    public GameObject playerPrefab;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
    }

    private void OnMove(SocketIOEvent obj)
    {
        Debug.Log("player is moving " + obj.data);
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected");
    }

    void OnSpawned (SocketIOEvent e)
    {
        Debug.Log("Spawned");
        Instantiate(playerPrefab);
    }
}
