using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.AI;

public class Network : MonoBehaviour
{

    public static SocketIOComponent socket;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject myPlayer;
    private PlayerCharacter pc;
    public List<Player> players = new List<Player>();
    public List<Enemy> enemies = new List<Enemy>();
    public string sGUID;
    public string playerUN;

    void Start()
    {
        sGUID = myPlayer.GetComponent<NetworkEntity>().GetSGUID();
        socket = GetComponent<SocketIOComponent>();
        pc = myPlayer.GetComponent<PlayerCharacter>();
        socket.On("isConnected", isConnected);
        socket.On("spawn", OnSpawned);
        socket.On("spawnEnemy", OnSpawnedEnemy);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnect);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("requestPositionEnemy", OnRequestPositionEnemy);
        socket.On("requestStateEnemy", OnRequestStateEnemy);
        socket.On("requestTargetEnemy", OnRequestTargetEnemy);
    }

    void OnMove(SocketIOEvent obj)
    {
        var position = new Vector3(GetFloatFromJSON(obj.data, "posX"), GetFloatFromJSON(obj.data, "posY"), GetFloatFromJSON(obj.data, "posZ"));
        var hash = obj.data["hash"].ToString();
        var player = FindPlayer(hash);

        var navPos = player.PlayerOBJ.GetComponent<NavigatePosition>();
        navPos.NavigatTo(position);

    }

    void isConnected(SocketIOEvent e)
    {
        pc.RegisterPlayer();
    }

    void OnDisconnect(SocketIOEvent e)
    {
        var id = e.data["hash"].ToString();
        Debug.Log("Player disconnected hash: " + e.data["hash"]);

        RemovePlayer(id);
    }

    void OnRequestPosition(SocketIOEvent e)
    {
        socket.Emit("updatePosition", new JSONObject(Network.VectorToJSON(myPlayer.transform.position)));
    }

    void OnSpawned (SocketIOEvent e)
    {

        var x = float.Parse(e.data["posX"].ToString().Replace("\"", ""));
        var y = float.Parse(e.data["posY"].ToString().Replace("\"", ""));
        var z = float.Parse(e.data["posZ"].ToString().Replace("\"", ""));
        Vector3 position = new Vector3(x, y, z);
        string hash = e.data["hash"].ToString();
        string userName = e.data["username"].ToString();
        //Debug.Log(hash);
        Debug.Log("Spawned " + userName);

        Player player = new Player();

        GameObject playerObj = Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

        playerObj.GetComponent<NavMeshAgent>().enabled = false;
        playerObj.transform.position = position;
        playerObj.GetComponent<NavMeshAgent>().enabled = true;

        player.Hash = hash;
        player.Username = userName;
        player.PlayerOBJ = playerObj;

        players.Add(player);

        Debug.Log("Other players in the World: " + players.Count);
        //spawner.SpawnPlayer(e.data["hash"].ToString(), e.data["username"].ToString(), new Vector3(float.Parse(e.data["posX"].ToString()), float.Parse(e.data["posY"].ToString()), float.Parse(e.data["posZ"].ToString())));
        
    }

    void OnSpawnedEnemy(SocketIOEvent e)
    {

        var x = float.Parse(e.data["posX"].ToString().Replace("\"", ""));
        var y = float.Parse(e.data["posY"].ToString().Replace("\"", ""));
        var z = float.Parse(e.data["posZ"].ToString().Replace("\"", ""));
        Vector3 position = new Vector3(x, y, z);
        string id = e.data["id"].ToString();
        string name = e.data["name"].ToString();
        string combatState = e.data["combatState"].ToString().Replace("\"", "");
        Enemy enemy = new Enemy();

        GameObject enemyObj = Instantiate(enemyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

        enemyObj.GetComponent<NavMeshAgent>().enabled = false;
        enemyObj.transform.position = position;
        enemyObj.GetComponent<NavMeshAgent>().enabled = true;

        enemy.Id = id;
        enemy.Name = name;
        enemy.EnemyOBJ = enemyObj;
        enemy.CombatState = combatState;

        enemies.Add(enemy);

        enemyObj.GetComponent<TurtleAI>().SetID(id);
        enemyObj.GetComponent<TurtleAI>().SetAggrod(combatState);
    }

    void OnRequestPositionEnemy(SocketIOEvent e)
    {

        var x = float.Parse(e.data["posX"].ToString().Replace("\"", ""));
        var y = float.Parse(e.data["posY"].ToString().Replace("\"", ""));
        var z = float.Parse(e.data["posZ"].ToString().Replace("\"", ""));
        Vector3 position = new Vector3(x, y, z);
        string id = e.data["id"].ToString();
        string name = e.data["name"].ToString();

        Enemy enemy = FindEnemy(id);

        GameObject enemyObj = enemy.EnemyOBJ;

        enemyObj.GetComponent<TurtleAI>().MoveTo(position);

    }

    void OnRequestStateEnemy(SocketIOEvent e)
    {
        string id = e.data["id"].ToString();
        string state = e.data["combatState"].ToString().Replace("\"", "");

        Enemy enemy = FindEnemy(id);

        GameObject enemyObj = enemy.EnemyOBJ;

        enemyObj.GetComponent<TurtleAI>().SetAggrod(state);

    }

    void OnRequestTargetEnemy(SocketIOEvent e)
    {

        var x = float.Parse(e.data["posX"].ToString().Replace("\"", ""));
        var y = float.Parse(e.data["posY"].ToString().Replace("\"", ""));
        var z = float.Parse(e.data["posZ"].ToString().Replace("\"", ""));
        Vector3 position = new Vector3(x, y, z);
        string id = e.data["id"].ToString();
        string name = e.data["name"].ToString();

        Enemy enemy = FindEnemy(id);

        GameObject enemyObj = enemy.EnemyOBJ;

        enemyObj.GetComponent<TurtleAI>().MoveTo(position);

    }

    public static float GetFloatFromJSON(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    public static string VectorToJSON(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }

    public Player FindPlayer(string hash)
    {
        Player response = players.Find(r => r.Hash == hash);

        return response;
    }

    public void RemovePlayer(string hash)
    {
        GameObject obj = FindPlayer(hash).PlayerOBJ;
        Destroy(obj);
        players.Remove(FindPlayer(hash));

    }

    public Enemy FindEnemy(string hash)
    {
        Enemy response = enemies.Find(r => r.Id == hash);

        return response;
    }

    public void RemoveEnemy(string hash)
    {
        GameObject obj = FindEnemy(hash).EnemyOBJ;
        Destroy(obj);
        enemies.Remove(FindEnemy(hash));

    }

    public class Player
    {
        public string Hash { get; set; }
        public string Username { get; set; }
        public GameObject PlayerOBJ { get; set; }
    }

    public class Enemy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GameObject EnemyOBJ { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string MaxHealth { get; set; }
        public string CurrentHealth { get; set; }
        public string CombatState { get; set; }
    }

}
