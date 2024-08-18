using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement); // Declaramos un delegado que define un tipo de método que acepta un parámetro float y no devuelve nada.
    public ParallaxCameraDelegate onCameraTranslate; // Declaramos un evento que se dispara cuando la cámara se mueve
    private float oldPosition;

    void Start()
    {
        oldPosition = transform.position.x; // guarda la posición inicial de la cámara en el eje x.
    }

    void Update()
    {
        if (transform.position.x != oldPosition) // verifica si la posición de la cámara en el eje x ha cambiado.
        {
            if (onCameraTranslate != null) // Si hay un cambio en la posición y hay algún método suscrito al evento onCameraTranslate, se ejecuta.
            {
                float delta = oldPosition - transform.position.x; // Se calcula la diferencia (delta) entre la posición anterior y la nueva posición de la cámara.
                onCameraTranslate(delta); // Se llama al evento onCameraTranslate y se pasa el delta como argumento.
            }

            oldPosition = transform.position.x;  // Se actualiza oldPosition
        }
    }
}
