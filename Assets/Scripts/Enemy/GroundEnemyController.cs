using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyController : BaseEnemyController
{
    [SerializeField] private Transform groundDetection;
    [SerializeField] private float detectionDistance;

    protected override void Update()
    {
        base.Update();

        // Debugging: Dibujar las rayas de detecci�n
        Debug.DrawRay(groundDetection.position, Vector2.down * detectionDistance, Color.red);

        // Detecci�n de colisiones para enemigos terrestres
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, detectionDistance);
        if (groundInfo.collider == null)
        {
            //print("volteate");
            // Cambiar de direcci�n si no hay suelo delante
            movingRight = !movingRight;
            UpdateSpriteDirection();
        }
    }

    protected override void Start()
    {
        base.Start();
        // Asegurarse de que la orientaci�n inicial est� correcta
        UpdateSpriteDirection();
    }

    protected override void UpdateSpriteDirection()
    {
        base.UpdateSpriteDirection();

        // Ajustar la posici�n de groundDetection
        Vector3 localPosition = groundDetection.localPosition;
        localPosition.x = Mathf.Abs(localPosition.x) * (movingRight ? 1 : -1);
        groundDetection.localPosition = localPosition;
    }

    void OnDrawGizmos()
    {
        // Dibuja una l�nea en la escena para visualizar la detecci�n del suelo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundDetection.position, groundDetection.position + Vector3.down * detectionDistance);
    }
}
