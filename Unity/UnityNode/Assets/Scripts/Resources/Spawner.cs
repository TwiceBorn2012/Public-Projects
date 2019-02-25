using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<Player> players = new List<Player>();

    public void SpawnPlayer(string hash, string username, Vector3 position)
    {
        Player player = new Player();
        var playerObj = Instantiate(playerPrefab, new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
        playerObj.GetComponent<NavMeshAgent>().enabled = false;
        playerObj.transform.position = position;
        playerObj.GetComponent<NavMeshAgent>().enabled = true;

        player.hash = hash;
        player.username = username;
        player.player = playerObj;

        players.Add(player);
        
        Debug.Log("Other players in the World: " + players.Count);
    }

    public Player FindPlayer(string hash)
    {
        Player response = players.Find(r => r.hash  == hash);
        return response;
    }

    public void RemovePlayer(string hash)
    {
        GameObject obj = FindPlayer(hash).player;
        Destroy(obj);
        players.Remove(FindPlayer(hash));
        
    }

    public class Player
    {
        public string hash { get; set; }
        public string username { get; set; }
        public GameObject player { get; set; }
    }
    
}
