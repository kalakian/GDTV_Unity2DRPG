using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;

    PlayerControls playerControls;
    Vector2 moveInput;
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        playerControls = new PlayerControls();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    void PlayerInput()
    {
        moveInput = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", moveInput.x);
        myAnimator.SetFloat("moveY", moveInput.y);
    }

    void Move()
    {
        myRigidbody2D.MovePosition(myRigidbody2D.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }

    void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 playerPos = Camera.main.WorldToScreenPoint(myRigidbody2D.position);

        mySpriteRenderer.flipX = mousePos.x < playerPos.x;
    }
}
