using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using DG.Tweening;
using System;

public class ItemController : MonoBehaviour
{
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutAndMove());
        }
    }

    private IEnumerator FadeOutAndMove()
    {
        float timer = 0f;
        Renderer renderer = GetComponent<Renderer>();
        Color initialColor = renderer.material.color;

        animator.enabled = false;
        spriteRenderer.sprite = originalSprite;

        while (timer < fadeDuration)
        {
            transform.DOMoveY(transform.position.y + moveDistance, moveDuration).SetEase(Ease.Linear);
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            renderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // Notifica al ItemsManager sobre el tipo de ítem recogido
        ItemsManager.Instance.ItemCollected((ItemsManager.ItemType)itemType);
        Destroy(gameObject);
    }
}
