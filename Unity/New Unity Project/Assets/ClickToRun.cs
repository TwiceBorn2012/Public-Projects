using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToRun : MonoBehaviour
{
    private NavMeshAgent mNavMeshAgent;
    private bool running;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        running = mNavMeshAgent.isStopped;
        
        Vector3 standing = new Vector3(0,0,0);
        if(mNavMeshAgent.velocity == standing)
        {
            running = false;
        }
        else
        {
            running = true;
        }

        anim.SetBool("isRunning", running);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, 100))
            {
                mNavMeshAgent.destination = hit.point;
            }
        }

    }
}
