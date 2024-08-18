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

        // Debugging: Dibujar las rayas de detección
        Debug.DrawRay(groundDetection.position, Vector2.down * detectionDistance, Color.red);

        // Detección de colisiones para enemigos terrestres
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, detectionDistance);
        if (groundInfo.collider == null)
        {
            //print("volteate");
            // Cambiar de dirección si no hay suelo delante
            movingRight = !movingRight;
            UpdateSpriteDirection();
        }
    }

    protected override void Start()
    {
        base.Start();
        // Asegurarse de que la orientación inicial esté correcta
        UpdateSpriteDirection();
    }

    protected override void UpdateSpriteDirection()
    {
        base.UpdateSpriteDirection();

        // Ajustar la posición de groundDetection
        Vector3 localPosition = groundDetection.localPosition;
        localPosition.x = Mathf.Abs(localPosition.x) * (movingRight ? 1 : -1);
        groundDetection.localPosition = localPosition;
    }

    void OnDrawGizmos()
    {
        // Dibuja una línea en la escena para visualizar la detección del suelo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundDetection.position, groundDetection.position + Vector3.down * detectionDistance);
    }
}
