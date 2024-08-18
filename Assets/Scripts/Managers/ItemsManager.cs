using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance { get; private set; }
    public PlayerExtrasTracker playerExtrasTracker;

    private int _heartItemsCollected;  // Contador de ítems de tipo Heart recogidos
    private int _rotatingCoinItemsCollected;  // Contador de ítems de tipo RotatingCoin recogidos
    private int _shiningCoinItemsCollected;  // Contador de ítems de tipo ShiningCoin recogidos

    public int HeartItemsCollected { get => _heartItemsCollected; set => _heartItemsCollected = value; }
    public int RotatingCoinItemsCollected { get => _rotatingCoinItemsCollected; set => _rotatingCoinItemsCollected = value; }
    public int ShiningCoinItemsCollected { get => _shiningCoinItemsCollected; set => _shiningCoinItemsCollected = value; }

    // Este enum debe ser el mismo que en ItemController para que no haya errores de referencia
    public enum ItemType
    {
        Heart,
        RotatingCoin,
        ShiningCoin
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ItemCollected(ItemType itemType)
    {
        // Manejo de ítems recogidos según el tipo
        if (itemType == ItemType.Heart)
        {
            HeartItemsCollected++;
            if (HeartItemsCollected >= 5 && !playerExtrasTracker.CanDoubleJump)
            {
                playerExtrasTracker.CanDoubleJump = true;
                PlayerPrefs.SetInt("CanDoubleJump", 1); // Guardar estado de la habilidad
            }
        }
        else if (itemType == ItemType.RotatingCoin)
        {
            RotatingCoinItemsCollected++;
            if (RotatingCoinItemsCollected >= 10 && !playerExtrasTracker.CanDash)
            {
                playerExtrasTracker.CanDash = true;
                PlayerPrefs.SetInt("CanDash", 1); // Guardar estado de la habilidad
            }
        }
        else if (itemType == ItemType.ShiningCoin)
        {
            ShiningCoinItemsCollected++;
            if (ShiningCoinItemsCollected >= 6 && (!playerExtrasTracker.CanEnterBallMode || !playerExtrasTracker.CanDropBombs))
            {
                playerExtrasTracker.CanEnterBallMode = true;
                playerExtrasTracker.CanDropBombs = true;
                PlayerPrefs.SetInt("CanEnterBallMode", 1); // Guardar estado de la habilidad
                PlayerPrefs.SetInt("CanDropBombs", 1); // Guardar estado de la habilidad
            }
        }
    }

    private void OnGUI()
    {
        GUIStyle largeFontStyle = new GUIStyle(GUI.skin.label);
        largeFontStyle.fontSize = 24;

        if (!playerExtrasTracker.CanDoubleJump)
        {
            GUI.Label(new Rect(10, 10, 400, 50), "Heart Items Pending: " + Mathf.Max(0, 5 - HeartItemsCollected), largeFontStyle);
        }

        if (!playerExtrasTracker.CanDash)
        {
            GUI.Label(new Rect(10, 40, 400, 50), "Rotating Coin Items Pending: " + Mathf.Max(0, 10 - RotatingCoinItemsCollected), largeFontStyle);
        }

        if (!playerExtrasTracker.CanEnterBallMode || !playerExtrasTracker.CanDropBombs)
        {
            GUI.Label(new Rect(10, 70, 400, 50), "Shining Coin Items Pending: " + Mathf.Max(0, 6 - ShiningCoinItemsCollected), largeFontStyle);
        }
    }
}