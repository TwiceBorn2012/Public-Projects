using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class NetworkLogin : MonoBehaviour
{
    public static SocketIOComponent socket;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("auth", OnAuth);
        socket.On("connected", OnConnected);

    }

    void OnAuth(SocketIOEvent obj)
    {
        Debug.Log("Tried to auth");
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("Login screen connected");
    }

}
