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
    private Ray ray;

    private void Start()
    {
        menu = GameObject.Find("Canvas/InvisUI/RightClickMenu");
        textGO = GameObject.Find("Canvas/InvisUI/RightClickMenu");
        title = textGO.GetComponentInChildren<Text>();
        menu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit1;
            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray1, out hit1, 100))
            {
                if (hit1.transform != null)
                {
                    menu.transform.position = hit1.point;
                    menu.SetActive(true);
                }

            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            int i = 0;
            while (i < hits.Length)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.tag != "UI" && hit.collider.gameObject.tag != "Ground")
                {

                    Text target = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem" + i + "/Text").GetComponent<Text>();

                    target.text = hit.transform.gameObject.name;
                        //title.text = hit.transform.gameObject.name;
                    
                }
                if (hit.collider.gameObject.tag == "Ground")
                {

                    Text target = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem" + i + "/Text").GetComponent<Text>();
                    //Debug.Log(hit.collider.gameObject.tag);
                    target.text = "Walk Here";
                    //title.text = hit.transform.gameObject.name;

                }
                //Debug.Log(hit.collider.gameObject.name);
                i++;
            }


        }
        if (Input.GetMouseButtonDown(0))
        {
            menu.SetActive(false);
            Text target1 = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem1/Text").GetComponent<Text>();
            Text target2 = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem2/Text").GetComponent<Text>();
            Text target3 = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem3/Text").GetComponent<Text>();
            Text target4 = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem4/Text").GetComponent<Text>();
            Text target5 = GameObject.Find("Canvas/InvisUI/RightClickMenu/RightClickMenuItem5/Text").GetComponent<Text>();
            target1.text = "";
            target2.text = "";
            target3.text = "";
            target4.text = "";
            target5.text = "";
        }
    }

}
