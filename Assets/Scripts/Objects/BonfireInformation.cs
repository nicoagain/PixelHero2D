using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireInformation : MonoBehaviour
{
    public GameObject buttonToPress; 
    public GameObject infoPanel; 
    private bool playerInRange; 

    void Start()
    {
        buttonToPress.SetActive(false); 
        infoPanel.SetActive(false); 
        playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            infoPanel.SetActive(!infoPanel.activeSelf); // Activa o desactiva el panel de información
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonToPress.SetActive(true); 
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonToPress.SetActive(false); 
            playerInRange = false;
        }
    }
}
