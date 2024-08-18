using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseEnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] protected GameObject destructionParticles;
    [SerializeField] protected float speed;
    [SerializeField] protected float minPatrolTime, maxPatrolTime;

    protected bool movingRight;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected AudioSource audioSource;

    protected virtual void Start()
    {
        movingRight = Random.value > 0.5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Patrol());

        // Asegurarse de que la orientación inicial esté correcta
        UpdateSpriteDirection();
    }

    protected virtual void Update()
    {
        float moveDirection = movingRight ? 1f : -1f;
        transform.Translate(Vector2.right * speed * Time.deltaTime * moveDirection);
    }

    protected IEnumerator Patrol()
    {
        while (true)
        {
            // Espera un tiempo aleatorio antes de cambiar de dirección
            float patrolTime = Random.Range(minPatrolTime, maxPatrolTime);
            yield return new WaitForSeconds(patrolTime);

            // Cambiar de dirección
            movingRight = !movingRight;
            UpdateSpriteDirection();
        }
    }

    protected virtual void UpdateSpriteDirection()
    {
        // Voltear el sprite del enemigo según la dirección de movimiento
        spriteRenderer.flipX = movingRight;
    }

    public void DestroyEnemy()
    {
        Instantiate(destructionParticles, transform.position, Quaternion.identity);
        audioSource.Play();
        spriteRenderer.enabled = false;
        Destroy(gameObject, 0.3f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (collision.collider.CompareTag("Arrow"))
        {
            DestroyEnemy();
        }
    }
}
