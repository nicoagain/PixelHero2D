using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector; // necesitamos girar en 180 el offset de la plataforma

    [SerializeField] float startWaitTime; // tiempo de espera para bajar de la plataforma

    private float waitedTime; // tiempo transcurrido

    private void Awake()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.S)) 
            waitedTime = startWaitTime;

        if (Input.GetKey(KeyCode.S))
        {
            if (waitedTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitedTime = startWaitTime;
            }
            else
                waitedTime -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            effector.rotationalOffset = 0;
        }
    }
}
