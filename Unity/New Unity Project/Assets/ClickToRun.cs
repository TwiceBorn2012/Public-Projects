using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToRun : MonoBehaviour
{
    private NavMeshAgent mNavMeshAgent;
    private bool running;
    private Animator anim;
    private Ray ray;
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

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(Physics.Raycast(ray, out hit, 100))
        //    {
        //        Debug.Log(hit);
        //        mNavMeshAgent.destination = hit.point;
        //    }
        //}

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //RaycastHit[]  hits;

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            int i = 0;
            while (i < hits.Length)
            {
                RaycastHit hit = hits[i];
                if(hit.collider.gameObject.tag == "Ground")
                {
                    mNavMeshAgent.destination = hit.point;
                }
                //Debug.Log(hit.collider.gameObject.name);
                i++;
            }
        }

    }
}
