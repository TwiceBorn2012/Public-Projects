using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public GameObject SpawnPlayer(string id, string position)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        players.Add(id, player);

        return player;
    }

    public GameObject FindPlayer(string id)
    {
        return players[id];
    }

    public void RemovePlayer(string id)
    {
        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }
}
