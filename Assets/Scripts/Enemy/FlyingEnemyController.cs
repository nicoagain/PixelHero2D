using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : BaseEnemyController
{
    protected override void Start()
    {
        base.Start();
        // Asegurarse de que la orientación inicial esté correcta
        UpdateSpriteDirection();
    }

    protected override void UpdateSpriteDirection()
    {
        // Voltear el sprite del enemigo según la dirección de movimiento para los enemigos voladores
        spriteRenderer.flipX = !movingRight;
    }
}
