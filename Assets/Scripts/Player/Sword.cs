using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    [SerializeField] GameObject slashAnimPrefab;
    [SerializeField] Transform slashAnimSpawnPoint;
    [SerializeField] Transform weaponCollider;
    [SerializeField] float swordAttackCD = 0.5f;

    PlayerInput playerInput;
    Animator myAnimator;
    ActiveWeapon activeWeapon;
    bool attackButtonDown = false;
    bool isAttacking = false;
    Vector2 lookInput;
    Vector2 mousePos;

    GameObject slashAnim;

    void Awake()
    {
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        LookWithOffset();
        Attack();
    }

    void OnAttack(InputValue inputValue)
    {
        attackButtonDown = inputValue.isPressed;
    }

    IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
    }

    void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;

            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);

            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;

            StartCoroutine(AttackCDRoutine());
        }
    }

    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        slashAnim.GetComponent<SpriteRenderer>().flipX = PlayerController.Instance.FacingLeft;
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.GetComponent<SpriteRenderer>().flipX = PlayerController.Instance.FacingLeft;
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

    void LookWithOffset()
    {
        if (playerInput.currentControlScheme == "KBM")
        {
            lookInput = (mousePos - (Vector2)PlayerController.Instance.transform.position).normalized;
        }

        float angle = Mathf.Atan(lookInput.y / lookInput.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -15, 15);

        if (lookInput.x < -0.1)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else if (lookInput.x > 0.1)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
