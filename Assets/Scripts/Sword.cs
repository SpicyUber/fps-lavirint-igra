using UnityEngine;

public class Sword : MonoBehaviour, Weapon
{
    public Transform HurtboxCastTransform; // početna tačka napada (npr. ruka)
    public float attackRange = 2f;         // koliko daleko "seče"
    public float attackRadius = 1f;      // širina sečiva
    public int damage = 50;                // kolika šteta
    public float attackCooldown = 1f;

    private float nextAttackTime = 0f;
    public void Start()
    {
        Debug.Log("Start je pozvan");
       

    }
    public void Update()
    {
        UseAttack();
    }
    public float GetAttackRangeForAI()
    {
        return attackRange;
    }

    public bool UseAttack()
    {

        if (Time.time < nextAttackTime)
            return false;

        nextAttackTime = Time.time + attackCooldown;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        // SphereCast za melee detekciju
        Ray ray = new Ray(HurtboxCastTransform.position, HurtboxCastTransform.forward);
        if (Physics.SphereCast(ray, attackRadius, out RaycastHit hit, attackRange))
        {
            Debug.Log("Sword pogodio: " + hit.collider.name);

            HealthComponent health = hit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        return true;
    }

    // Samo za vizuelizaciju u sceni
    private void OnDrawGizmosSelected()
    {
        if (HurtboxCastTransform == null) return;
        Debug.Log("Gizmos pozvan");
        Gizmos.color = Color.red;
        Gizmos.DrawRay(HurtboxCastTransform.position, HurtboxCastTransform.forward * attackRange);
        Gizmos.DrawWireSphere(HurtboxCastTransform.position + HurtboxCastTransform.forward * attackRange, attackRadius);
    }
}
