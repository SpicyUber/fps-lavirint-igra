using UnityEngine;

public class DavyClaw : MonoBehaviour, Weapon
{
    public Transform HurtboxCastTransform; // početna tačka napada (npr. ruka)
    public float attackRange = 1f;         // koliko daleko "seče"
    public float attackRadius = 1f;      // širina sečiva
    public int damage = 100;                // kolika šteta
    public float attackCooldown = 1f;

    private float nextAttackTime = 0f;
    public float GetAttackRangeForAI()
    {
        //samo za testiranje
        return 1f;
    }

    public bool UseAttack()
    {
        if (Time.time < nextAttackTime)
            return false;

        nextAttackTime = Time.time + attackCooldown;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        // SphereCast za melee detekciju
        Debug.Log("Ulazi u UseAttack"); 
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UseAttack();
    }
    private void OnDrawGizmosSelected()
    {
        if (HurtboxCastTransform == null) return;
        Debug.Log("Gizmos pozvan");
        Gizmos.color = Color.red;
        Gizmos.DrawRay(HurtboxCastTransform.position, HurtboxCastTransform.forward * attackRange);
        Gizmos.DrawWireSphere(HurtboxCastTransform.position + HurtboxCastTransform.forward * attackRange, attackRadius);
    }
}
