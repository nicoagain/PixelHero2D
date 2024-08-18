using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor; // cuánto se moverá el GO en relación con el movimiento de la cámara.
    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition; // Se obtiene la posición local actual del objeto.
        newPos.x -= delta * parallaxFactor; // Se ajusta la posición en el eje x restando el delta multiplicado por el parallaxFactor

        transform.localPosition = newPos; // Se actualiza la posición local
    }
}
