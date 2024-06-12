using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] GameObject slashAnimPrefab;
    [SerializeField] Transform slashAnimSpawnPoint;
    [SerializeField] Transform weaponCollider;
    [SerializeField] float swordAttackCD = 0.5f;

    PlayerControls playerControls;
    Animator myAnimator;
    PlayerController playerController;
    ActiveWeapon activeWeapon;
    bool attackButtonDown = false;
    bool isAttacking = false;

    GameObject slashAnim;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    void StartAttacking()
    {
        attackButtonDown = true;
    }

    void StopAttacking()
    {
        attackButtonDown = false;
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

        slashAnim.GetComponent<SpriteRenderer>().flipX = playerController.FacingLeft;
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.GetComponent<SpriteRenderer>().flipX = playerController.FacingLeft;
    }

    void MouseFollowWithOffset()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 playerPos = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if(mousePos.x < playerPos.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
