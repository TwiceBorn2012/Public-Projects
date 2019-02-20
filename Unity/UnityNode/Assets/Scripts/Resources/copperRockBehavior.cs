using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copperRockBehavior : MonoBehaviour
{
    private GameObject rock;
    private string type = "copper";
    private int charges;
    private int maxCharges = 4;
    private float timer = 0.0f;
    private float waitTime = 4.0f;
    public Material[] material;
    Renderer rend;
    private GameObject player;
    public PlayerCharacter pc;

    void Start()
    {
        rock = this.gameObject;
        charges = 0;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerCharacter>();
    }

    public void MineRock(GameObject player)
    {
        // Full test - get player equipment, see if there is a pickaxe, add int prof
        // Get player skill mining - add to int prof
        // Random roll based on range from int prof

        
        // ON success
        if(charges > 0)
        {
            charges--;
            CheckCharges();
        }
        if (charges == 0)
        {
            CheckCharges();
        }
        CheckCharges();

        pc.AddPlayerItemToInv("L8Va0fd122Sl");
        pc.UpdateInventory();

        // Decrease charge
        // Add item to player inventory on server
        // Run Update inventory
    }

    void CheckCharges()
    {
        if (charges >= 1)
        {
            rend.sharedMaterial = material[1];

        }
        if (charges == 0)
        {
            rend.sharedMaterial = material[0];
            // Choose different shader based on rock type
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            if (charges < maxCharges)
            {
                charges = charges + 1;
            }
            CheckCharges();

            timer = 0.0f;
        }
    }
}
