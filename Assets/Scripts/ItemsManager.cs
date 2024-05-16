using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance { get; private set; }
    public PlayerExtrasTracker playerExtrasTracker;

    private int _heartItemsCollected;
    private int _rotatingCoinItemsCollected;
    private int _shiningCoinItemsCollected;

    

    // Este enum debe ser el mismo que en ItemController
    public enum ItemType
    {
        Heart,
        RotatingCoin,
        ShiningCoin
    }

    public int HeartItemsCollected { get => _heartItemsCollected; set => _heartItemsCollected = value; }
    public int RotatingCoinItemsCollected { get => _rotatingCoinItemsCollected; set => _rotatingCoinItemsCollected = value; }
    public int ShiningCoinItemsCollected { get => _shiningCoinItemsCollected; set => _shiningCoinItemsCollected = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ItemCollected(ItemType itemType)
    {
        if (itemType == ItemType.Heart)
        {
            Debug.Log("Heart Collected");
            HeartItemsCollected++;
            if (HeartItemsCollected >= 5 && !playerExtrasTracker.CanDoubleJump)
            {
                playerExtrasTracker.CanDoubleJump = true;
                Debug.Log("Double Jump Unlocked");
            }
        }
        else if (itemType == ItemType.RotatingCoin)
        {
            Debug.Log("Rotating Coin Collected");
            RotatingCoinItemsCollected++;
            if (RotatingCoinItemsCollected >= 6 && !playerExtrasTracker.CanDash)
            {
                playerExtrasTracker.CanDash = true;
                Debug.Log("Dash Unlocked");
            }
        }
        else if (itemType == ItemType.ShiningCoin)
        {
            Debug.Log("Shining Coin Collected");
            ShiningCoinItemsCollected++;
            if (ShiningCoinItemsCollected >= 10 && (!playerExtrasTracker.CanEnterBallMode || !playerExtrasTracker.CanDropBombs))
            {
                playerExtrasTracker.CanEnterBallMode = true;
                playerExtrasTracker.CanDropBombs = true;
                Debug.Log("Ball Mode and Drop Bombs Unlocked");
            }
        }
    }
}