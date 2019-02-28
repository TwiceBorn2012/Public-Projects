using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SocketIO;

public class TurtleAI : MonoBehaviour, IClickable
{
    private SocketIOComponent socket;
    public GameObject player;
    private Network network;
    private NavMeshAgent mNavMeshAgent;
    public Material[] material;
    Renderer rend;
    private float timer = 0.0f;
    private float waitTime = 10.0f;
    private string TurtleID = "";
    private string state = "";
    private GameObject target;
    private bool following = false;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("MyPlayer");
        socket = GameObject.FindGameObjectWithTag("Network").GetComponent<SocketIOComponent>();
        network = GameObject.FindGameObjectWithTag("Network").GetComponent<Network>();
    }

    private void Update()
    {
        if (state == "Auth")
        {
            rend.sharedMaterial = material[1];
            mNavMeshAgent.destination = player.transform.position;
            Vector3 position = gameObject.transform.position;
            socket.Emit("updatePositionEnemy", new JSONObject(string.Format(@"{{""id"":""{0}"",""posX"":""{1}"",""posY"":""{2}"",""posZ"":""{3}""}}", TurtleID, position.x, position.y, position.z)));

            
        }

        if (state == "Server" || state == "Aggro")
        {

            socket.Emit("requestEnemyState", new JSONObject(string.Format(@"{{""id"":""{0}""}}", TurtleID)));
            

            if (state == "Aggro")
            {
                rend.sharedMaterial = material[2];
            }
            else
            {
                rend.sharedMaterial = material[0];
            }

            socket.Emit("requestPositionEnemy", new JSONObject(string.Format(@"{{""id"":""{0}""}}", TurtleID)));

        }


    }

    public void MoveTo(Vector3 pos)
    {
        mNavMeshAgent.destination = pos;
    }

    public void SetFollow (GameObject _player)
    {
        following = true;
        target = _player;
    }

    public void SetID (string id)
    {
        TurtleID = id;
    }

    public void SetAggrod(string b)
    {
        state = b;
    }

    public void OnClick(RaycastHit hit)
    {
        // Player move to enemy
        // Set State to Auth

        var navPos = player.GetComponent<NavigatePosition>();
        navPos.NavigatTo(hit.point);

        if(state == "Server")
        {
            SetAggrod("Auth");
            SetFollow(player);
            socket.Emit("updateStateEnemy", new JSONObject(string.Format(@"{{""id"":""{0}""}}", TurtleID)));
        }
        
    }
}

    //void CheckRadius()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (GameObject target in enemies)
    //    {
    //        float distance = Vector3.Distance(target.transform.position, transform.position);
    //        if (distance < 20)
    //        {
    //            aggrod = true;
    //            mNavMeshAgent.destination = target.transform.position;

    //        }
    //        else
    //        {
    //            aggrod = false;
    //        }
    //    }

    //}

    //CheckRadius(giant, player);

    //if (aggrod == false)
    //{
    //timer += Time.deltaTime;

    //if (timer > waitTime)
    //{
    //    int roll = Random.Range(1, 6);
    //    if (roll == 1)
    //    {
    //        Vector3 left = new Vector3(10, 0, 0);
    //        mNavMeshAgent.destination = transform.position + left;
    //    }
    //    if (roll == 2)
    //    {
    //        Vector3 right = new Vector3(-10, 0, 0);
    //        mNavMeshAgent.destination = transform.position + right;
    //    }
    //    if (roll == 3)
    //    {
    //        Vector3 right = new Vector3(0, 0, 10);
    //        mNavMeshAgent.destination = transform.position + right;
    //    }
    //    if (roll == 4)
    //    {
    //        Vector3 right = new Vector3(0, 0, -10);
    //        mNavMeshAgent.destination = transform.position + right;
    //    }
    //    if (roll == 5)
    //    {
    //        // Stand still
    //    }

    //    timer = 0.0f;
    //}

    //}
    //else
    //{

    //}

    // Go left 8 times
    // Stand still for 3 seconds
    // Go right 8 times
    // Stand still for 3 seconds