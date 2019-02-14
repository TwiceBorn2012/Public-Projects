using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrollingFG : MonoBehaviour
{
    private NavMeshAgent mNavMeshAgent;
    public GameObject giant;
    private float timer = 0.0f;
    private float waitTime = 3.0f;
    private string direction = "left";
    public GameObject player;
    private NavMeshAgent fg;
    private NavMeshAgent target;
    private bool aggrod = false;

    private void Start()
    {
        //mNavMeshAgent = GetComponent<NavMeshAgent>();
        
        giant = GameObject.Find("FireGiant");
        mNavMeshAgent = giant.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Low Poly Warrior");
        target = player.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckRadius(giant, player);

        if (aggrod == false)
        {
            timer += Time.deltaTime;

            if (timer > waitTime)
            {
                int roll = Random.Range(1, 6);
                if (roll == 1)
                {
                    Vector3 left = new Vector3(10, 0, 0);
                    mNavMeshAgent.destination = giant.transform.position + left;
                }
                if (roll == 2)
                {
                    Vector3 right = new Vector3(-10, 0, 0);
                    mNavMeshAgent.destination = giant.transform.position + right;
                }
                if (roll == 3)
                {
                    Vector3 right = new Vector3(0, 0, 10);
                    mNavMeshAgent.destination = giant.transform.position + right;
                }
                if (roll == 4)
                {
                    Vector3 right = new Vector3(0, 0, -10);
                    mNavMeshAgent.destination = giant.transform.position + right;
                }
                if (roll == 5)
                {
                    // Stand still
                }

                timer = 0.0f;
            }
            
        }
        else
        {

        }
        
        // Go left 8 times
        // Stand still for 3 seconds
        // Go right 8 times
        // Stand still for 3 seconds
        

    }

    void CheckRadius(GameObject center, GameObject target)
    {
        float dist = Vector3.Distance(center.transform.position, target.transform.position);
        if (dist < 20)
        {
            aggrod = true;
            mNavMeshAgent.destination = target.transform.position;

        }
        else
        {
            aggrod = false;
        }
        
    }

}
