using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public static PlayerController Instance;

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float dashSpeed = 4f;
    [SerializeField] float dashTime = .2f;
    [SerializeField] float dashCD = .25f;
    [SerializeField] TrailRenderer myTrailRenderer;
    [SerializeField] bool usingGamePad = true;

    PlayerControls playerControls;
    Vector2 moveInput;
    Vector2 lookInput;
    Rigidbody2D rb;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    bool facingLeft = false;
    bool isDashing = false;

    void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
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
        if (usingGamePad)
        {
            lookInput = playerControls.Movement.Look.ReadValue<Vector2>();
        }

        myAnimator.SetFloat("moveX", moveInput.x);
        myAnimator.SetFloat("moveY", moveInput.y);
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }

    void AdjustPlayerFacingDirection()
    {
        if (usingGamePad)
        {
            if (lookInput.x < -0.1)
            {
                facingLeft = true;
            }
            else if (lookInput.x > 0.1)
            {
                facingLeft = false;
            }
        }
        else
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 playerPos = Camera.main.WorldToScreenPoint(rb.position);

            facingLeft = mousePos.x < playerPos.x;
        }
        mySpriteRenderer.flipX = facingLeft;
    }

    void Dash()
    {
        if (!isDashing)
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
        moveSpeed /= dashSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
