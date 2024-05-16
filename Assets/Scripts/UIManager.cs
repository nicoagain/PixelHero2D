using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ItemsManager itemsManager;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 200), "Heart Items Pending: " + Mathf.Max(0, 5 - itemsManager.HeartItemsCollected));
        GUI.Label(new Rect(10, 30, 200, 200), "Rotating Coin Items Pending: " + Mathf.Max(0, 6 - itemsManager.RotatingCoinItemsCollected));
        GUI.Label(new Rect(10, 50, 200, 200), "Shining Coin Items Pending: " + Mathf.Max(0, 10 - itemsManager.ShiningCoinItemsCollected));
    }
}
