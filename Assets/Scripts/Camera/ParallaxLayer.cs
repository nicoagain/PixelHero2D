using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor; // cu�nto se mover� el GO en relaci�n con el movimiento de la c�mara.
    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition; // Se obtiene la posici�n local actual del objeto.
        newPos.x -= delta * parallaxFactor; // Se ajusta la posici�n en el eje x restando el delta multiplicado por el parallaxFactor

        transform.localPosition = newPos; // Se actualiza la posici�n local
    }
}
