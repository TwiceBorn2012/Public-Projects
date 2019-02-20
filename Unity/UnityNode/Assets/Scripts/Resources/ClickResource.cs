using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickResource : MonoBehaviour, IClickable
{
    private GameObject player;
    private Animator anim;
    private NavMeshAgent agent;
    private bool isMoving;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        agent = player.GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        isMoving = player.GetComponent<Animator>().GetBool("Moving");
        
    }

    IEnumerator mineRock(GameObject rock)
    {
        isMoving = true;
        yield return new WaitUntil(() => isMoving == false);
        anim.SetTrigger("MineTrigger");
        var script = rock.GetComponent<copperRockBehavior>();
        script.MineRock(player);
        
    }

    public void OnClick(RaycastHit hit)
    {

        //Debug.Log("Gathering " + hit.collider.gameObject.name);
        if (hit.collider.tag == "MineRock")
        {
            var navPos = player.GetComponent<NavigatePosition>();
            navPos.NavigatTo(hit.point);

            StartCoroutine(mineRock(hit.collider.gameObject));

        }
    }
}
