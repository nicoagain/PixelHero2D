using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement); // Declaramos un delegado que define un tipo de m�todo que acepta un par�metro float y no devuelve nada.
    public ParallaxCameraDelegate onCameraTranslate; // Declaramos un evento que se dispara cuando la c�mara se mueve
    private float oldPosition;

    void Start()
    {
        oldPosition = transform.position.x; // guarda la posici�n inicial de la c�mara en el eje x.
    }

    void Update()
    {
        if (transform.position.x != oldPosition) // verifica si la posici�n de la c�mara en el eje x ha cambiado.
        {
            if (onCameraTranslate != null) // Si hay un cambio en la posici�n y hay alg�n m�todo suscrito al evento onCameraTranslate, se ejecuta.
            {
                float delta = oldPosition - transform.position.x; // Se calcula la diferencia (delta) entre la posici�n anterior y la nueva posici�n de la c�mara.
                onCameraTranslate(delta); // Se llama al evento onCameraTranslate y se pasa el delta como argumento.
            }

            oldPosition = transform.position.x;  // Se actualiza oldPosition
        }
    }
}
