using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public ParallaxCamera parallaxCamera; // Referencia a la cámara que manejará el efecto de paralaje
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>(); // Lista que contendrá las capas de paralaje

    void Start()
    {
        // Si no se ha asignado una cámara de paralaje, se intenta obtener la cámara principal
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // Si se encontró una cámara de paralaje, se suscribe al evento de traducción de la cámara
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

            // Si el hijo tiene el componente ParallaxLayer, lo añade a la lista y le asigna un nombre
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    void Move(float delta)
    {
        // Mueve cada capa de paralaje según el desplazamiento de la cámara
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
