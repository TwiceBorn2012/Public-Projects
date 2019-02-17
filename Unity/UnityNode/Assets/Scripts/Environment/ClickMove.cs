using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour, IClickable
{
    public GameObject player;

    public void OnClick(RaycastHit hit)
    {
        var navPos = player.GetComponent<NavigatePosition>();
        navPos.NavigatTo(hit.point);
        var netMove = player.GetComponent<NetworkMove>();
        netMove.OnMove(hit.point);
    }
}
