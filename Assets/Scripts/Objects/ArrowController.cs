using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody2D arrowRB;
    [SerializeField] private float arrowSpeed;
    private Vector2 _arrowDirection;

    public Vector2 arrowDirection { get => _arrowDirection; set => _arrowDirection = value; }

    [SerializeField] private GameObject arrowImpact;
    private Transform arrowTransform;

    private void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        arrowTransform = GetComponent<Transform>();
    }

    void Update()
    {
        arrowRB.velocity = arrowDirection * arrowSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si la colisión no es con el enemigo, instanciar las partículas de impacto de la flecha
        Instantiate(arrowImpact, transform.position, Quaternion.identity);
        // Destruir la flecha
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
