using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using DG.Tweening;
using System;

public class ItemController : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceHeart;
    [SerializeField] AudioSource audioSourceRotatingCoin;
    [SerializeField] AudioSource audioSourceShiningCoin;

    [SerializeField] private float fadeDuration;
    [SerializeField] private float moveDuration;
    [SerializeField] private float moveDistance;
    private bool isFading = false;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    private Animator animator;

    public ItemType itemType;

    // Este enum debe ser el mismo que en ItemsManager
    public enum ItemType
    {
        Heart,
        RotatingCoin,
        ShiningCoin
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        animator = GetComponent<Animator>();

        // Ocultar el ítem si la habilidad ya está desbloqueada
        if (IsAbilityUnlocked())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobación la colisión con el player y que no se esté desvaneciendo
        if (other.CompareTag("Player") && !isFading)
        {
            isFading = true;  // Marcamos que está desvaneciendo
            PlayCollectionSound();
            StartCoroutine(FadeOutAndMove());  // Iniciamos la corrutina de desvanecimiento y movimiento
        }
    }

    private IEnumerator FadeOutAndMove()
    {
        float timer = 0f;  // Temporizador
        Renderer renderer = GetComponent<Renderer>();
        Color initialColor = renderer.material.color;  // Color inicial del material

        animator.enabled = false;  // Desactivamos el animator
        spriteRenderer.sprite = originalSprite;  // Restauramos el sprite original

        while (timer < fadeDuration)
        {
            // Movemos el objeto en el eje Y
            transform.DOMoveY(transform.position.y + moveDistance, moveDuration).SetEase(Ease.Linear);
            // Calculamos la transparencia del objeto
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            renderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            timer += Time.deltaTime;  // Incrementamos el temporizador
            yield return null;
        }

        // Notifica al ItemsManager sobre el tipo de ítem recogido
        ItemsManager.Instance.ItemCollected((ItemsManager.ItemType)itemType);
        // Destruimos el objeto
        Destroy(gameObject);
    }

    private void PlayCollectionSound()
    {
        switch (itemType)
        {
            case ItemType.Heart:
                if (audioSourceHeart != null)
                {
                    audioSourceHeart.Play();
                }
                break;
            case ItemType.RotatingCoin:
                if (audioSourceRotatingCoin != null)
                {
                    audioSourceRotatingCoin.Play();
                }
                break;
            case ItemType.ShiningCoin:
                if (audioSourceShiningCoin != null)
                {
                    audioSourceShiningCoin.Play();
                }
                break;
        }
    }

    private bool IsAbilityUnlocked()
    {
        switch (itemType)
        {
            case ItemType.Heart:
                return PlayerPrefs.GetInt("CanDoubleJump", 0) == 1;
            case ItemType.RotatingCoin:
                return PlayerPrefs.GetInt("CanDash", 0) == 1;
            case ItemType.ShiningCoin:
                return PlayerPrefs.GetInt("CanEnterBallMode", 0) == 1;
            default:
                return false;
        }
    }
}
