using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public Gun Gun;
    public Weapon WeaponObject; 
    public bool ChasesPlayer = true;
    public float MoveSpeed = 3.5f;
    public float VisionRange = 50f;
    //public float AttackRange = 100f;
    public int Damage = 10;
    public AudioClip hurtClip;


    public NavMeshAgent Agent;
    public PlayerScript Player;

    
    public Animator Animator;

    public EnemyState CurrentState = EnemyState.IDLE;

    private HealthComponent _health;
    private AudioSource audioSource;

    void Start()
    {
        if (Gun == null)
            Gun = GetComponentInChildren<Gun>();
        audioSource = GetComponent<AudioSource>();

        if (Agent == null)
            Agent = GetComponent<NavMeshAgent>();
        //
        if (Agent == null)
        {
            Debug.LogError("NavMeshAgent nije pronađen na objektu!");
        }
        else
        {
            Debug.Log("NavMeshAgent je uspešno pronađen i postavljen!");
        }

        if (Player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) Player = found.GetComponent<PlayerScript>();
        }

        _health = GetComponent<HealthComponent>();

        if (_health != null)
        {
            _health.OnDeath.AddListener(Death);
            _health.OnDamage.AddListener(PlayHurtSound);
            _health.OnDamage.AddListener(StunSelf);

        }

        Agent.speed = MoveSpeed;

        TransitionTo(EnemyState.IDLE);
        Animator.SetTrigger("IDLE");
    }

    void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.IDLE:
                
                if (CanSeePlayer())
                {
                    Debug.Log("Vidio igrača! Prelazim u CHASE");
                    TransitionTo(EnemyState.CHASE);
                }
                break;

            case EnemyState.CHASE:
           
                Debug.Log("CHASE stanje aktivno");
                Debug.Log("Agent isStopped: " + Agent.isStopped);  // Proveri da li je agent zaustavljen
                Debug.Log("Agent velocity: " + Agent.velocity);     // Proveri brzinu
                Debug.Log("Agent remaining distance: " + Agent.remainingDistance); // Preostala udaljenost do cilja
                if (PlayerInAttackRange())
                {
                    StartCoroutine(AttackRoutine());
                }
                else
                {
                    // Ako je agent zaustavljen, pokreni ga
                    if (Agent.isStopped)
                    {
                        Agent.isStopped = false;  // Pokreni agenta
                        Debug.Log("Agent is now moving towards the player.");
                    }

                    Vector3 playerPosition = Player.transform.position;
                    Debug.Log("Postavljam destinaciju: " + playerPosition);
                    Agent.SetDestination(playerPosition);
                }
                
                break;
        }
        

    }

    public void TransitionTo(EnemyState state)
    {
        Debug.Log("Prelazim u stanje: " + state);
        if (CurrentState == EnemyState.DEAD) return;
        CurrentState = state;

        switch (state)
        {
            case EnemyState.IDLE:
                Agent.isStopped = true;
                
                Animator.SetTrigger("IDLE");
                Debug.Log("Animacija: IDLE");
                break;

            case EnemyState.CHASE:
                if (ChasesPlayer)
                {
                    Agent.isStopped = false;
                    
                    Animator.SetTrigger("CHASE");
                    Debug.Log("Animacija: CHASE");
                }
                break;

            case EnemyState.DEAD:
                Agent.isStopped = true;
                
                Animator.SetTrigger("DEAD");
                Debug.Log("Animacija: DEAD");
                GetComponent<Collider>().enabled = false;
                break;
        }
    }

    IEnumerator AttackRoutine()
    {
        if (CurrentState == EnemyState.WINDUP || CurrentState == EnemyState.WINDDOWN || CurrentState == EnemyState.STUNNED)
            yield break;

        TransitionTo(EnemyState.WINDUP);
        Agent.isStopped = true;
        Animator.SetTrigger("WINDUP");

        yield return new WaitForSeconds(0.5f); // Windup trajanje

        if (WeaponObject != null)
            WeaponObject.UseAttack();
        else
            TryDealDamage();

        TransitionTo(EnemyState.WINDDOWN);
        Animator.SetTrigger("WINDDOWN");

        yield return new WaitForSeconds(0.4f); // cooldown

        Agent.isStopped = false;
        TransitionTo(EnemyState.CHASE);
    }

    void TryDealDamage()
    {
        if (Player == null) return;
        float dist = Vector3.Distance(transform.position, Player.transform.position);
        if (dist <= Gun.GetAttackRangeForAI())
        {
            var playerHealth = Player.GetComponent<HealthComponent>();
            if (playerHealth != null)
                playerHealth.TakeDamage(Damage);
        }
    }

    public void StunSelf()
    {
        if (CurrentState == EnemyState.DEAD) return;
        StartCoroutine(StunRoutine());
    }

    IEnumerator StunRoutine()
    {
        if (CurrentState == EnemyState.DEAD) yield break;
        
        TransitionTo(EnemyState.STUNNED);
        Agent.isStopped = true;
        Animator.SetTrigger("STUNNED");
        yield return new WaitForSeconds(1f);
        Agent.isStopped = false;
        TransitionTo(EnemyState.CHASE);
    }

    public void Death()
    {
        TransitionTo(EnemyState.DEAD);
    }

    public void PlayHurtSound()
    {
        if (audioSource != null && hurtClip != null)
        {
            audioSource.PlayOneShot(hurtClip);
        }
    }

    bool CanSeePlayer()
    {
        if (Player == null)
        {
            Debug.Log("Nema Player reference");
            return false;
        }
            float dist = Vector3.Distance(transform.position, Player.transform.position);
        if (dist > VisionRange)
        {
            Debug.Log("Igrač je predaleko: " + dist);
            return false;
        }

        Vector3 dir = (Player.transform.position - transform.position).normalized;
        //
        Debug.DrawRay(transform.position + Vector3.up, dir * VisionRange, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up, dir, out RaycastHit hit, VisionRange))
        {
            
            Debug.Log("Raycast pogodio: " + hit.collider.name + " | Tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Raycast vidi igrača!");
                return true;
            }
        }
        Debug.Log("Ne vidi igrača (Raycast blokiran)");
        return false;
    }

    bool PlayerInAttackRange()
    {
        if (Player == null || Gun==null) return false;
        return Vector3.Distance(transform.position, Player.transform.position) <= Gun.GetAttackRangeForAI();
    }
}
