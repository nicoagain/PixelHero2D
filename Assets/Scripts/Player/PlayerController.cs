using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Sprites
    private GameObject standingPlayer;
    private GameObject ballPlayer;

    [Header("Player Movement")]
    private Rigidbody2D playerRB;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private LayerMask selectedLayerMask; // para seleccionar con qué layers queremos interactuar para saltar
    [SerializeField] LayerMask layerGround;
    [SerializeField] LayerMask layerWall;
    private bool isGrounded, isFlippedInX;
    private bool canWallJump = false;
    private bool canMove = true;
    private Animator animatorStandingPlayer;
    private Animator animatorBallPlayer;
    private int IDspeed, IDisGrounded, IDshootArrow, IDcanDoubleJump, IDwallJump;
    private float ballModeCounter;
    [SerializeField] private float waitForBallMode;
    [SerializeField] private float isGroundedRange;
    public float distance;

    [Header("Player Shoot")]
    [SerializeField] private ArrowController arrowController;
    [SerializeField] private GameObject prefabBomb;
    private Transform arrowPointTransform;
    private Transform bombPointTransform;

    [Header("Player Dust")]
    [SerializeField] private GameObject dustJump;
    private Transform dustPointTransform;
    private bool isIdle, canDoubleJump;

    [Header("Player Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float waitForDash;
    private Transform playerControllerTransform;
    private float dashCounter;
    private float afterDashCounter;

    [Header("After Image Player Dash")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer afterImageSR;
    [SerializeField] private float afterImageLifeTime;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float afterImageTimeBetween;
    private float afterImageCounter;


    // Player Extras
    private PlayerExtrasTracker playerExtrasTracker;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerControllerTransform = GetComponent<Transform>();
        playerExtrasTracker = GetComponent<PlayerExtrasTracker>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Encontrar y asignar el GameObject del jugador en posición de pie
        standingPlayer = GameObject.Find("StandingPlayer");
        // Encontrar y asignar el GameObject del jugador en modo bola y lo desactivamos
        ballPlayer = GameObject.Find("BallPlayer");
        ballPlayer.SetActive(false);
        // Obtener el Transform del punto de polvo
        dustPointTransform = GameObject.Find("DustPoint").GetComponent<Transform>();
        // Obtener el Transform del punto de flecha
        arrowPointTransform = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        // Obtener el Transform del punto de bomba
        bombPointTransform = GameObject.Find("BombPoint").GetComponent<Transform>();
        // Obtener el Animator del jugador en posición de pie
        animatorStandingPlayer = standingPlayer.GetComponent<Animator>();
        // Obtener el Animator del jugador en modo bola
        animatorBallPlayer = ballPlayer.GetComponent<Animator>();
        // Asignar los IDs de los parámetros del Animator
        IDspeed = Animator.StringToHash("speed");
        IDisGrounded = Animator.StringToHash("isGrounded");
        IDshootArrow = Animator.StringToHash("shootArrow");
        IDcanDoubleJump = Animator.StringToHash("canDoubleJump");
        IDwallJump = Animator.StringToHash("onWall");
    }

    void Update()
    {
        Dash();
        Jump();
        CheckAndSetDirection();
        Shoot();
        PlayDust();
        BallMode();
        CheckWall();
    }

    private void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        playerRB.velocity = new Vector2(inputX, playerRB.velocity.y);

        if (standingPlayer.activeSelf)
        {
            // Actualizar la velocidad en el Animator del jugador de pie
            animatorStandingPlayer.SetFloat(IDspeed, Mathf.Abs(playerRB.velocity.x));
        }

        if (ballPlayer.activeSelf)
        {
            // Actualizar la velocidad en el Animator del jugador en modo bola
            animatorBallPlayer.SetFloat(IDspeed, Mathf.Abs(playerRB.velocity.x));
        }
    }

    private void Dash()
    {
        // Reducir el contador para la acción después del dash si es mayor a 0
        if (afterDashCounter > 0)
        {
            afterDashCounter -= Time.deltaTime;
        }
        else
        {
            // Si se presiona el botón de dash y el jugador está en StandingPlayer y puede hacer dash
            if ((Input.GetButtonDown("Fire2") && standingPlayer.activeSelf) && playerExtrasTracker.CanDash)
            {
                // Iniciar el contador de dash
                dashCounter = dashTime;
                // Mostrar la imagen posterior del dash
                ShowAfterImage();
            }
        }

        // Si el contador de dash es mayor a 0
        if (dashCounter > 0)
        {
            // Reducir el contador de dash
            dashCounter -= Time.deltaTime;
            // Establecer la velocidad del jugador en el eje X durante el dash
            playerRB.velocity = new Vector2(dashSpeed * playerControllerTransform.localScale.x, playerRB.velocity.y);
            // Reducir el contador para la imagen posterior del dash
            afterImageCounter -= Time.deltaTime;
            // Si el contador para la imagen posterior del dash es menor o igual a 0, mostrar la imagen posterior del dash
            if (afterImageCounter <= 0)
            {
                ShowAfterImage();
            }
            // Establecer el tiempo de espera para el siguiente dash
            afterDashCounter = waitForDash;
        }
        else
        { 
            if(canMove)// Si no está dasheando, llamar al método de movimiento normal
                Move();
        }
    }

    private void Jump()
    {
        // Comprobamos si el jugador está en el suelo utilizando un Raycast hacia abajo desde el punto de chequeo del suelo
        isGrounded = Physics2D.Raycast(checkGroundPoint.position, Vector2.down, isGroundedRange, selectedLayerMask);

        // Si se presiona el botón de salto y el jugador está en el suelo o puede hacer doble salto y tiene permitido hacer doble salto
        if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump && playerExtrasTracker.CanDoubleJump) || canWallJump) && canMove)
        {
            // Si el jugador está en el suelo
            if (isGrounded)
            {
                // Permitir hacer doble salto
                canDoubleJump = true;
                // Instanciar el efecto de polvo al saltar en el dustPoint
                Instantiate(dustJump, dustPointTransform.position, Quaternion.identity);
            }
            else if (canWallJump)
            {
                StartCoroutine(WallJump());
                return;
            }
            else
            {
                // Cancelar la posibilidad de hacer doble salto
                canDoubleJump = false;
                // Activar la animación de doble salto en el Animator del jugador de pie
                animatorStandingPlayer.SetTrigger(IDcanDoubleJump);
            }

            // Aplicamos la fuerza de salto al jugador
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }

        // Actualizar el estado de StandingPlayer en el Animator
        animatorStandingPlayer.SetBool(IDisGrounded, isGrounded);
    }

    IEnumerator WallJump()
    {
        canMove = false;
        // Dirección del salto desde la pared, ajustamos según la orientación del jugador
        Vector2 jumpDirection = isFlippedInX ? new Vector2(1, 1) : new Vector2(-1, 1);
        playerRB.velocity = jumpDirection.normalized * jumpForce;

        // Desactivar la posibilidad de hacer un nuevo salto desde la pared
        canWallJump = false;

        // Instanciar el efecto de polvo al saltar en el punto correspondiente
        Instantiate(dustJump, dustPointTransform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    private void CheckWall()
    {
        Vector2 direction = isFlippedInX ? Vector2.left : Vector2.right;
        Debug.DrawLine(transform.position, (Vector2)transform.position + direction * distance);
        if (Physics2D.Raycast(transform.position, direction, distance, layerWall) && (playerRB.velocity.y < 0 || canWallJump))
        {
            //print("checkwall");
            playerRB.velocity = Vector2.zero;
            animatorStandingPlayer.SetBool(IDwallJump, true);
            canWallJump = true;
        }
        else
        {
            animatorStandingPlayer.SetBool(IDwallJump, false);
            canWallJump = false;
        }
    }

    private void CheckAndSetDirection()
    {
        if (playerRB.velocity.x < 0)
        {
            //playerSR.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
            isFlippedInX = true;
        }
        else if (playerRB.velocity.x > 0)
        {
            //playerSR.flipX = false;
            transform.localScale = Vector3.one;
            isFlippedInX = false;
        }
    }

    private void Shoot()
    {
        // Si se presiona el botón de disparo y el jugador está en posición de pie
        if (Input.GetButtonDown("Fire1") && standingPlayer.activeSelf)
        {
            // Instanciamos la flecha
            ArrowController arrow = Instantiate(arrowController, arrowPointTransform.position, arrowPointTransform.rotation);
            // Si el jugador está volteado en el eje X, ajustar la dirección de la flecha
            if (isFlippedInX)
            {
                arrow.arrowDirection = new Vector2(-1, 0f);
                arrow.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                arrow.arrowDirection = new Vector2(1, 0f);
            }
            // Activar la animación de disparo en el Animator del StandingPlayer
            animatorStandingPlayer.SetTrigger(IDshootArrow);
        }
        // Si se presiona el botón de disparo y el jugador está en modo bola y puede lanzar bombas
        if ((Input.GetButtonDown("Fire1") && ballPlayer.activeSelf) && playerExtrasTracker.CanDropBombs)
        {
            // Instanciamos una bomba en el BombPoint
            Instantiate(prefabBomb, bombPointTransform.position, Quaternion.identity);
        }
    }

    private void PlayDust()
    {
        // Si la velocidad no es 0 y está en Idle
        if (playerRB.velocity.x != 0 && isIdle) 
        {
            // Dejar de estar en Idle
            isIdle = false;
            // Si está en el suelo, instanciar la partícula
            if (isGrounded)
            {
                Instantiate(dustJump, dustPointTransform.position, Quaternion.identity);
            }
        }
        // Si la velocidad es 0, indicar que está en Idle
        if (playerRB.velocity.x == 0)
        {
            isIdle = true;
        }
    }

    private void ShowAfterImage()
    {
        // Instanciar la imagen posterior del dash en la posición y rotación del jugador
        SpriteRenderer afterImage = Instantiate(afterImageSR, playerControllerTransform.position, playerControllerTransform.rotation);
        // Asignar el sprite del jugador a la imagen posterior del dash
        afterImage.sprite = playerSR.sprite;
        // Asignar la escala del jugador a la imagen posterior del dash
        afterImage.transform.localScale = playerControllerTransform.localScale;
        // Asignar el color de la imagen posterior del dash
        afterImage.color = afterImageColor;
        // Destruir la imagen posterior del dash después de un tiempo determinado
        Destroy(afterImage.gameObject, afterImageLifeTime);
        // Establecer el tiempo de espera para la siguiente imagen posterior del dash
        afterImageCounter = afterImageTimeBetween;
    }

    private void BallMode()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        // Con este condicional entramos o salimos del modo bola
        if ((verticalInput <= -1f && !ballPlayer.activeSelf || verticalInput >= 1f && ballPlayer.activeSelf) && playerExtrasTracker.CanEnterBallMode)
        {
            // Reducir el contador para el modo bola
            ballModeCounter -= Time.deltaTime;
            // Si el contador llega a cero o menos...
            if (ballModeCounter < 0)
            {
                // Cambiar entre el jugador en modo bola y en posición de pie y desactivar el DoubleJump si está en modo bola
                ballPlayer.SetActive(!ballPlayer.activeSelf);
                standingPlayer.SetActive(!standingPlayer.activeSelf);
                playerExtrasTracker.CanDoubleJump = !playerExtrasTracker.CanDoubleJump;
            }
        }
        else
        {
            // Reiniciar el contador para el modo bola
            ballModeCounter = waitForBallMode;
        }
    }

    private void OnDrawGizmos()
    {
        // Dibujar una esfera en el checkgroundpoint del suelo para mostrar el rango de detección
        Gizmos.DrawWireSphere(checkGroundPoint.position, isGroundedRange);
    }
}