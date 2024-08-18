using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceMove : MonoBehaviour
{
    [SerializeField] float moveDistance; 
    [SerializeField] float moveDuration; 

    void Start()
    {
        StartCoroutine(MoveUpAndDown());
    }

    IEnumerator MoveUpAndDown()
    {
        Vector2 startPosition = transform.position;

        // Posición final después de mover hacia abajo
        Vector2 downPosition = new Vector2(startPosition.x, startPosition.y - moveDistance);
        // Posición final después de mover hacia arriba
        Vector2 upPosition = new Vector2(startPosition.x, startPosition.y + moveDistance);

        while (true)
        {
            yield return StartCoroutine(MoveToPosition(downPosition));
            yield return StartCoroutine(MoveToPosition(upPosition));
        }
    }

    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
