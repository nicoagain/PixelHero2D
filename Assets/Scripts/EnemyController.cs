using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject destructionParticles;

    public void DestroyEnemy()
    {
        // Instanciar las part�culas de destrucci�n del enemigo en la posici�n del enemigo
        Instantiate(destructionParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (collision.collider.CompareTag("Arrow"))
        {
            // Llamar a la funci�n DestroyEnemy cuando la flecha colisiona con el enemigo
            DestroyEnemy();
        }
    }
}
