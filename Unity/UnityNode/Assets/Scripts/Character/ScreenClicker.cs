using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenClicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Clicked();
        }
    }
    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit))
        {
            var clickMove = hit.collider.gameObject.GetComponent<ClickMove>();
            clickMove.OnClick(hit.point);
        }
    }
}
