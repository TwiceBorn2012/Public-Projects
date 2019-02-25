using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigatePosition : MonoBehaviour
{
    NavMeshAgent agent;
    private bool moving = false;
    private float brakingDistance = PlayerCharacter.brakingDistance;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void NavigatTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    void Update()
    {
        float currentDistance = agent.remainingDistance - brakingDistance;

        if (currentDistance > 0.0f)
        {
            moving = true;
            GetComponent<Animator>().SetBool("Moving", moving);
        }
        else
        {
            moving = false;
            GetComponent<Animator>().SetBool("Moving", moving);
        }

    }
}
