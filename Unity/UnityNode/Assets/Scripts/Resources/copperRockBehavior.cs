using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copperRockBehavior : MonoBehaviour
{
    private GameObject copperRock;
    private int charges;
    private int maxCharges = 4;
    private float timer = 0.0f;
    private float waitTime = 4.0f;
    private Color empty = new Color(0.161f, 0.161f, 0.161f);
    private Color charged = new Color(0.6603f, 0.1077f, 0.0280f);
    public Material[] material;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        copperRock = GameObject.Find("Copper_Rock");
        var rockMat = copperRock.GetComponent<Renderer>().material;
        rockMat.shader = Shader.Find("_Color");
        rockMat.SetColor("_Color", empty);
        charges = 0;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            //Debug.Log(charges);
            //var rockMat = copperRock.GetComponent<Renderer>().material;
            // Debug.Log(rockColor);
            if (charges < maxCharges)
            {
                charges = charges + 1;
            }
            if (charges > 1)
            {
                rend.sharedMaterial = material[1];
                //rockMat.shader = Shader.Find("Gray.2");
                //rockMat.SetColor("Gray.2", charged);
            }
            if(charges == 0)
            {
                rend.sharedMaterial = material[0];
                //rockMat.shader = Shader.Find("Gray.2");
                //rockMat.SetColor("Gray.2", empty);
            }

            timer = 0.0f;
        }
    }
}
