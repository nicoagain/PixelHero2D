using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private float checkpointPositionX, checkpointPositionY;
    private PlayerExtrasTracker extrasTracker;

    void Start()
    {
        // Obtener referencia al componente PlayerExtrasTracker
        extrasTracker = GetComponent<PlayerExtrasTracker>();

        // Cargar posición del checkpoint
        if (PlayerPrefs.GetFloat("checkpointPositionX") != 0)
        {
            Vector2 checkpointPosition = new Vector2(PlayerPrefs.GetFloat("checkpointPositionX"), PlayerPrefs.GetFloat("checkpointPositionY"));
            transform.position = checkpointPosition;
        }

        // Cargar habilidades desbloqueadas
        extrasTracker.LoadPlayerPrefs();
    }

    public void ReachedCheckpoint(float x, float y)
    {
        // Guardar posición del checkpoint
        PlayerPrefs.SetFloat("checkpointPositionX", x);
        PlayerPrefs.SetFloat("checkpointPositionY", y);

        // Guardar habilidades desbloqueadas
        extrasTracker.SavePlayerPrefs();
    }
}
