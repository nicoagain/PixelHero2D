using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : BaseEnemyController
{
    protected override void Start()
    {
        base.Start();
        // Asegurarse de que la orientaci�n inicial est� correcta
        UpdateSpriteDirection();
    }

    protected override void UpdateSpriteDirection()
    {
        // Voltear el sprite del enemigo seg�n la direcci�n de movimiento para los enemigos voladores
        spriteRenderer.flipX = !movingRight;
    }
}
