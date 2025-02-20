using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Main : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject showPlayer;
    public GameObject StartUI;
    public Button button1;
    public Button button2;
    public Button button3;
    public GameObject WinUI;
    public GameObject Inventory;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;

     void Awake()
    {
        //button1 = GetComponent<Button>();
        //button2 = GetComponent<Button>();
        //button3 = GetComponent<Button>();
    }

    void Start()
    {
        showPlayer.gameObject.SetActive(false);
        One.gameObject.SetActive(false);
        Two.gameObject.SetActive(false);
        Three.gameObject.SetActive(false);
        button1.onClick.AddListener(button1UI);
        button2.onClick.AddListener(button2UI);
        button3.onClick.AddListener(button3UI);
    }

    private void button1UI()
    {
        gameObject.SetActive(false);
        showPlayer.gameObject.SetActive(true);
        One.gameObject.SetActive(true);
    }

    private void button2UI()
    {
        gameObject.SetActive(false);
        showPlayer.gameObject.SetActive(true);
        Two.gameObject.SetActive(true);
        Three.gameObject.SetActive(false);
    }

    private void button3UI()
    {
        gameObject.SetActive(false);
        showPlayer.gameObject.SetActive(true);
        Three.gameObject.SetActive(true);
    }

    void Update()
    {
        
    }
}
