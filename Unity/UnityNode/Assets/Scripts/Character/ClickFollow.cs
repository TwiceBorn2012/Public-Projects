using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFollow : MonoBehaviour, IClickable
{
    public void OnClick(RaycastHit hit)
    {
        Debug.Log("following" + hit.collider.gameObject.name);
    }
}
