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

    void Awake()
    {
        playerControls = new PlayerControls();
        myRigidbody2D = GetComponent<Rigidbody2D>();
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
        Move();
    }

    void PlayerInput()
    {
        moveInput = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    void Move()
    {
        myRigidbody2D.MovePosition(myRigidbody2D.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }

}
