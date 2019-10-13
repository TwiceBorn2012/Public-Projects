using System.Collections;
using System.Collections.Generic;
using System.Web;
using SocketIO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Data;
using System.Globalization;
using System;

public class MenuControls : MonoBehaviour
{
    public GameObject topMenuPanel;

    void Start()
    {
        //topMenuPanel = GameObject.FindGameObjectWithTag("OpenMenuUI").gameObject;
        List<GameObject> buttons = new List<GameObject>();
        var list = GameObject.FindGameObjectsWithTag("MenuButtons");
        foreach(GameObject item in list)
        {
            item.GetComponent<Button>().onClick.AddListener(OpenMenu1);
        }

    }

    public void CloseMenu()
    {
        topMenuPanel.SetActive(false);
    }

    public void OpenMenu1()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu2()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu3()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu4()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu5()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu6()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu7()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu8()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu9()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu10()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu11()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu12()
    {
        topMenuPanel.SetActive(true);
    }

    public void OpenMenu13()
    {
        topMenuPanel.SetActive(true);
    }
}

