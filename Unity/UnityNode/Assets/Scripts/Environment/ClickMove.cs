using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public GameObject player;

    public void OnClick(Vector3 position)
    {
        var navPos = player.GetComponent<NavigatePosition>();
        navPos.NavigatTo(position);
        var netMove = player.GetComponent<NetworkMove>();
        netMove.OnMove(position);
    }
}
