using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float waitForExplode;
    [SerializeField] private float waitForDestroy;
    private Animator animator;
    private bool isActive;
    private int IDisActive;
    [SerializeField] private Transform transformBomb;
    [SerializeField] private float expansiveWaveRange;
    [SerializeField] private LayerMask isDestroyable;
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        IDisActive = Animator.StringToHash("isActive");
        transformBomb = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Reducimos el contador de espera para la explosión
        waitForExplode -= Time.deltaTime;
        // Reducimos el contador de espera para la destrucción
        waitForDestroy -= Time.deltaTime;
        // Si el contador para la explosión llega a cero y la bomba no está activa
        if (waitForExplode <= 0 && !isActive)
        {
            // Activamos la bomba
            ActivatedBomb();
            audioSource.Play();
        }
        // Si el contador para la destrucción llega a cero, destruimos la bomba
        if (waitForDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Método para activar la bomba
    private void ActivatedBomb()
    {
        // Marcamos la bomba como activa
        isActive = true;
        impulse.GenerateImpulse();
        // Actualizamos el estado "isActive" en el Animator
        animator.SetBool(IDisActive, isActive);
        // Obtenemos todos los colliders dentro del rango de la onda expansiva que son destruibles
        Collider2D[] destroyedObjects = Physics2D.OverlapCircleAll(transformBomb.position, expansiveWaveRange, isDestroyable);
        // Si hay objetos destructibles dentro del rango
        if (destroyedObjects.Length > 0)
        {
            // destruimos cada objeto
            foreach (var col in destroyedObjects)
            {
                Destroy(col.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Dibujar una esfera en la posición de la bomba para mostrar el rango de la onda expansiva
        Gizmos.DrawWireSphere(transformBomb.position, expansiveWaveRange);
    }
}
