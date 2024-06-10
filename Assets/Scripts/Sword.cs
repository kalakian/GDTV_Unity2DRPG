using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] GameObject slashAnimPrefab;
    [SerializeField] Transform slashAnimSpawnPoint;

    PlayerControls playerControls;
    Animator myAnimator;
    PlayerController playerController;
    ActiveWeapon activeWeapon;

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
        playerControls.Combat.Attack.started += _ => Attack();
    }

    void Update()
    {
        MouseFollowWithOffset();
    }

    void Attack()
    {
        myAnimator.SetTrigger("Attack");

        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    public void SwingUpFlipAnim()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        slashAnim.GetComponent<SpriteRenderer>().flipX = playerController.FacingLeft;
    }

    public void SwingDownFlipAnim()
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
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
