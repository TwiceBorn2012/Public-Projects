using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //public float speed = 6f;            // Standard speed for character
    //public float turnSpeed = 60f;       // Character turn speed
    //public float turnSmoothing = 15f;

    //private Vector3 movement;
    //private Vector3 turning;
    //private Animator anim;
    //private Rigidbody playerRigidbody;

    //void Awake()
    //{
    //    anim = GetComponent<Animator>();
    //    playerRigidbody = GetComponent<playerRigidbody>();
    //}

    //void FixedUpdate()
    //{
    //    // Store input axes
    //    float lh = Input.GetAxisRaw("Horizontal");
    //    float lv = Input.GetAxisRaw("Vertical");

    //}

    public LayerMask whatCanBeClickedOn;

    private UnityEngine.AI.NavMeshAgent myAgent;

    void Start()
    {
        myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
            {
                myAgent.SetDestination(hitInfo.point);
                Debug.Log(hitInfo.point);
            }
        }
    }

}
