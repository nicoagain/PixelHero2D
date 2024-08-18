using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public ParallaxCamera parallaxCamera; // Referencia a la c�mara que manejar� el efecto de paralaje
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>(); // Lista que contendr� las capas de paralaje

    void Start()
    {
        // Si no se ha asignado una c�mara de paralaje, se intenta obtener la c�mara principal
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // Si se encontr� una c�mara de paralaje, se suscribe al evento de traducci�n de la c�mara
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // Inicializa las capas de paralaje
        SetLayers();
    }

    void SetLayers()
    {
        // Limpia la lista de capas de paralaje
        parallaxLayers.Clear();

        // Recorre todos los hijos del transform actual
        for (int i = 0; i < transform.childCount; i++)
        {
            // Obtiene el componente ParallaxLayer del hijo
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // Si el hijo tiene el componente ParallaxLayer, lo a�ade a la lista y le asigna un nombre
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    void Move(float delta)
    {
        // Mueve cada capa de paralaje seg�n el desplazamiento de la c�mara
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
