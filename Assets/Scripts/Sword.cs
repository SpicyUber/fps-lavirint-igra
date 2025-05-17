using UnityEngine;

public class Sword : MonoBehaviour, Weapon
{
    public Transform HurtboxCastTransform; // početna tačka napada (npr. ruka)
    public float attackRange = 2f;         // koliko daleko "seče"
    public float attackRadius = 1f;      // širina sečiva
    public int damage = 50;                // kolika šteta
    public float attackCooldown = 1f;
    public float attackWindUp = 0.5f;
    private float nextAttackTime = 0f;


    public float GetCooldown() { return attackCooldown; }
    public void SetCooldown(float cooldown) { if (cooldown <= 0) attackCooldown = 1f; else attackCooldown = cooldown; }
    public void Start()
    {
        Debug.Log("Start je pozvan");
       

    }
    public void Update()
    {
       // UseAttack();
    }
    public float GetAttackRangeForAI()
    {
        return attackRange;
    }
    public float GetWindUp() { return attackWindUp; }
    public void SetWindUp(float windUp) { if (windUp <= 0) attackWindUp = 1f; else attackWindUp = windUp; }
    public bool UseAttack()
    {

        if (Time.time < nextAttackTime)
            return false;

        nextAttackTime = Time.time + attackCooldown;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        // SphereCast za melee detekciju
        Ray ray = new Ray(HurtboxCastTransform.position, HurtboxCastTransform.forward);
        foreach (RaycastHit hit in Physics.SphereCastAll(ray, attackRadius,  attackRange))
        {
            Debug.Log("Sword pogodio: " + hit.collider.name);

            HealthComponent health = hit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                if (hit.collider.gameObject.CompareTag("Player")) { 
                health.TakeDamage(damage);
                break;
                }
            }
        }

        return true;
    }

    // Samo za vizuelizaciju u sceni
    private void OnDrawGizmosSelected()
    {
        if (HurtboxCastTransform == null) return;
      //  Debug.Log("Gizmos pozvan");
        Gizmos.color = Color.red;
        Gizmos.DrawRay(HurtboxCastTransform.position, HurtboxCastTransform.forward * attackRange);
        Gizmos.DrawWireSphere(HurtboxCastTransform.position + HurtboxCastTransform.forward * attackRange, attackRadius);
    }
}
