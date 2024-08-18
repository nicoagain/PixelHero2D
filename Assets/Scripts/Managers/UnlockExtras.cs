using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockExtras : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerExtrasTracker playerExtrasTracker;
    [SerializeField] private bool canDoubleJump, canDash, canEnterBallMode, canDropBombs;
    AudioSource audioSource;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerExtrasTracker = player.GetComponent<PlayerExtrasTracker>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            SetTracker();
            Destroy(gameObject, 1);
        }
    }

    private void SetTracker()
    {
        if (canDoubleJump) playerExtrasTracker.CanDoubleJump = true;
        if (canDash) playerExtrasTracker.CanDash = true;
        if (canEnterBallMode) playerExtrasTracker.CanEnterBallMode = true;
        if (canDropBombs) playerExtrasTracker.CanDropBombs = true;
    }
}
