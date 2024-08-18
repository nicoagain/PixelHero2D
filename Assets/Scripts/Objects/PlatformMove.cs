using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    private float speed = 2f; // Velocidad de movimiento
    private float waitTime; // Tiempo de espera en cada punto
    public Transform[] moveSpots; // Array de puntos de movimiento
    private float startWaitTime = 2; // Tiempo inicial de espera
    private int i = 0; // Índice del punto de movimiento actual

    private void Start()
    {
        waitTime = startWaitTime; // Inicializa el tiempo de espera
    }

    private void Update()
    {
        // Mueve el objeto hacia el punto de movimiento actual
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[i].transform.position, speed * Time.deltaTime);

        // Verifica si el objeto está cerca del punto de movimiento actual
        if (Vector2.Distance(transform.position, moveSpots[i].transform.position) < 0.1f)
        {
            if (waitTime <= 0)
            {
                // Si no es el último punto de la lista, avanza al siguiente
                if (moveSpots[i] != moveSpots[moveSpots.Length - 1])
                    i++;
                else
                    i = 0; // Si es el último punto, vuelve al primero

                waitTime = startWaitTime; // Reinicia el tiempo de espera
            }
            else
                waitTime -= Time.deltaTime; // Decrementa el tiempo de espera
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el objeto colisionado tiene un componente PlayerController, lo hace hijo del objeto actual
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Si el objeto que deja de colisionar tiene un componente PlayerController, lo desasigna como hijo del objeto actual
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.transform.SetParent(null);
        }
    }
}
