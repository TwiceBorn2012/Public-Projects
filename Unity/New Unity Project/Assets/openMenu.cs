using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class openMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject textGO;
    public Text title;

    private void Start()
    {
        menu = GameObject.Find("Canvas/RightClickMenu");
        textGO = GameObject.Find("Canvas/RightClickMenu");
        title = textGO.GetComponentInChildren<Text>();
        menu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform != null)
                {
                    PrintName(hit.transform.gameObject);
                    title.text = hit.transform.gameObject.name;
                    menu.SetActive(true);
                }

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            menu.SetActive(false);
            
        }
    }

    private void PrintName (GameObject go)
    {
        //Debug.Log(go.name);
    }
}
