using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
