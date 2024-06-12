using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    public static PlayerController Instance;

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float dashSpeed = 4f;
    [SerializeField] float dashTime = .2f;
    [SerializeField] float dashCD = .25f;
    [SerializeField] TrailRenderer myTrailRenderer;

    PlayerInput playerInput;
    Vector2 moveInput;
    Vector2 lookInput;
    Vector2 mousePos;
    Rigidbody2D rb;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    float startingMoveSpeed;

    bool facingLeft = false;
    bool isDashing = false;

    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startingMoveSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();

        myAnimator.SetFloat("moveX", moveInput.x);
        myAnimator.SetFloat("moveY", moveInput.y);
    }

    void OnLook(InputValue inputValue)
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            lookInput = inputValue.Get<Vector2>();
        }
        else
        {
            mousePos = Camera.main.ScreenToWorldPoint(inputValue.Get<Vector2>());
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }

    void AdjustPlayerFacingDirection()
    {
        if(playerInput.currentControlScheme == "KBM")
        {
            lookInput = mousePos - rb.position;
        }

        if (lookInput.x < -0.1)
        {
            facingLeft = true;
        }
        else if (lookInput.x > 0.1)
        {
            facingLeft = false;
        }

        mySpriteRenderer.flipX = facingLeft;
    }

    void OnDash(InputValue inputValue)
    {
        if (!isDashing && inputValue.isPressed)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
