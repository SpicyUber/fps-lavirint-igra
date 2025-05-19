using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
   // public Gun Gun;
    public Weapon WeaponObject; 
    public bool ChasesPlayer = true;
    public float MoveSpeed = 3.5f;
    public float VisionRange = 50f;
    public int level = 1;
    //public float AttackRange = 100f;
    public int Damage = 10;
    public AudioClip hurtClip;


    public NavMeshAgent Agent;
    public PlayerScript Player;

    
    public Animator Animator;

    public EnemyState CurrentState = EnemyState.IDLE;

    private HealthComponent _health;
    private AudioSource audioSource;

    public void Start()
    {
        Func();
    }
    public virtual void Func()
    {

        if (WeaponObject == null)
            WeaponObject = GetComponentInChildren<Weapon>();
        if (this.GetType() != typeof(Davy))
        {
            SetLevel();
        }
        else
        {
            MoveSpeed = 3.5f;
        }


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
        if (Agent.isActiveAndEnabled)
            Agent.speed = MoveSpeed;
        if (CanSeePlayer())
        {
            // Debug.Log("Vidio igrača! Prelazim u CHASE");
            TransitionTo(EnemyState.CHASE);
        }
        else
            TransitionTo(EnemyState.IDLE);
        // Animator.SetTrigger("IDLE");
    }
    public void SetLevel()
    {
        if (level < 1 || level > 3) level = 1;
        //hard coding values. bad unity practice but its quicker right now then to manually go into editor.
        MoveSpeed = 3.5f * level;
        WeaponObject.SetCooldown(1.5f / level);
        WeaponObject.SetWindUp(0.5f / level);
        GetComponent<HealthComponent>().MaxHealth = 50f * level;
        GetComponent<HealthComponent>().CurrentHealth = 50f * level;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(level).gameObject.SetActive(true);

    }

    public void Update()
    {

        FuncUpdate();

    }
    public virtual void FuncUpdate()
    {
        switch (CurrentState)
        {
            case EnemyState.IDLE:

                Idle();
                break;

            case EnemyState.CHASE:
                Chase();

                //   Debug.Log("CHASE stanje aktivno");
                //     Debug.Log("Agent isStopped: " + Agent.isStopped);  Proveri da li je agent zaustavljen
                //  Debug.Log("Agent velocity: " + Agent.velocity);      Proveri brzinu
                //  Debug.Log("Agent remaining distance: " + Agent.remainingDistance);  Preostala udaljenost do cilja


                break;
        }
    }
    public void Chase()
    {
        if (PlayerInAttackRange())
        {
            StartCoroutine(AttackRoutine());
        }
        else
        {
            // Ako je agent zaustavljen, pokreni ga
            if (Agent.isActiveAndEnabled && Agent.isStopped)
            {
                Agent.isStopped = false;  // Pokreni agenta
                                          // Debug.Log("Agent is now moving towards the player.");
            }

            Vector3 playerPosition = Player.transform.position;
            // Debug.Log("Postavljam destinaciju: " + playerPosition);
            if (Agent.isActiveAndEnabled) Agent.SetDestination(playerPosition);
        }
    }
    public void Idle()
    {
        if (CanSeePlayer())
        {
            // Debug.Log("Vidio igrača! Prelazim u CHASE");
            if (ChasesPlayer) TransitionTo(EnemyState.CHASE); else StartCoroutine(AttackRoutine());
        }
    }
    public void EnemeyState()
    {
        switch (CurrentState)
        {
            case EnemyState.IDLE:
                
                if (CanSeePlayer())
                {
                   // Debug.Log("Vidio igrača! Prelazim u CHASE");
                    if (ChasesPlayer) TransitionTo(EnemyState.CHASE); else StartCoroutine(AttackRoutine());
                }
                break;

            case EnemyState.CHASE:

                //   Debug.Log("CHASE stanje aktivno");
                //     Debug.Log("Agent isStopped: " + Agent.isStopped);  Proveri da li je agent zaustavljen
                //  Debug.Log("Agent velocity: " + Agent.velocity);      Proveri brzinu
                //  Debug.Log("Agent remaining distance: " + Agent.remainingDistance);  Preostala udaljenost do cilja
                if (PlayerInAttackRange())
                {
                    StartCoroutine(AttackRoutine());
                }
                else
                {
                    // Ako je agent zaustavljen, pokreni ga
                    if (Agent.isActiveAndEnabled && Agent.isStopped)
                    {
                        Agent.isStopped = false;  // Pokreni agenta
                       // Debug.Log("Agent is now moving towards the player.");
                    }

                    Vector3 playerPosition = Player.transform.position;
                   // Debug.Log("Postavljam destinaciju: " + playerPosition);
                   if(Agent.isActiveAndEnabled) Agent.SetDestination(playerPosition);
                }
                
                break;
        }
    }
    public void TransitionTo(EnemyState state)
    {
      //  Debug.Log("Prelazim u stanje: " + state);
        if (CurrentState == EnemyState.DEAD) return;
        else if (state == EnemyState.DEAD) audioSource.PlayOneShot(audioSource.clip);
        if ( (CurrentState == EnemyState.WINDUP  || CurrentState == EnemyState.STUNNED || CurrentState == EnemyState.DEAD) && (state == EnemyState.IDLE)) return;
      
        CurrentState = state;

        switch (state)
        {
            case EnemyState.IDLE:
              if(Agent.isActiveAndEnabled)Agent.isStopped = true;
                
                Animator.SetTrigger("IDLE");
               // Debug.Log("Animacija: IDLE");
                break;

            case EnemyState.CHASE:
                if (ChasesPlayer)
                {
                    if (Agent.isActiveAndEnabled)
                        Agent.isStopped = false;
                    
                    Animator.SetTrigger("CHASE");
                  //  Debug.Log("Animacija: CHASE");
                }
                else
                {
                 //  TransitionTo(EnemyState.IDLE);
                }
                break;

            case EnemyState.DEAD:
                if (Agent.isActiveAndEnabled)Agent.isStopped = true;
                 StartCoroutine(WaitOneFrameThenDeathAnimation());
               
               // Debug.Log("Animacija: DEAD");
                GetComponent<Collider>().enabled = false;
                break;
            case EnemyState.WINDUP:

                transform.LookAt(Player.transform,transform.up);
               
                break;
        }
    }
    IEnumerator WaitOneFrameThenDeathAnimation()
    {
        yield return null;
        Animator.ResetTrigger("STUNNED");
        Animator.SetTrigger("DEAD");
    }
    IEnumerator AttackRoutine()
    {
        //ovde je falio uslov za DEAD. Takodje svi ovi uslovi se moraju ponavljati posle yield return-a
        if (CurrentState == EnemyState.WINDUP || CurrentState == EnemyState.WINDDOWN || CurrentState == EnemyState.STUNNED || CurrentState == EnemyState.DEAD)
            yield break;

        TransitionTo(EnemyState.WINDUP);
       if(Agent.isActiveAndEnabled) Agent.isStopped = true;
        Animator.SetTrigger("WINDUP");

        yield return new WaitForSeconds(WeaponObject.GetWindUp()); // Windup trajanje
        if (CurrentState == EnemyState.WINDDOWN || CurrentState == EnemyState.STUNNED || CurrentState == EnemyState.DEAD)
            yield break;
        if (WeaponObject != null)
        {
            Gun gun = WeaponObject as Gun;
            if (gun != null) { gun.BulletSpawnTransform.LookAt(Player.transform.position+new Vector3(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f))); } else { Debug.Log("GUN NULL"); }
            WeaponObject.UseAttack();
        }
          
        else
            TryDealDamage();

        TransitionTo(EnemyState.WINDDOWN);
        Animator.SetTrigger("WINDDOWN");

        yield return new WaitForSeconds(WeaponObject.GetCooldown()); // cooldown
        if (CurrentState == EnemyState.WINDUP || CurrentState == EnemyState.STUNNED || CurrentState == EnemyState.DEAD)
            yield break;
        if (Agent.isActiveAndEnabled) Agent.isStopped = false;
        TransitionTo(EnemyState.CHASE);
    }

    void TryDealDamage()
    {
        if (Player == null) return;
        float dist = Vector3.Distance(transform.position, Player.transform.position);
        if (dist <= WeaponObject.GetAttackRangeForAI())
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
        if (Agent.isActiveAndEnabled) Agent.isStopped = true;
        Animator.SetTrigger("STUNNED");
        yield return new WaitForSeconds(1f);
        if (Agent.isActiveAndEnabled) Agent.isStopped = false;
        TransitionTo(EnemyState.CHASE);
    }

    public void Death()
    {
        TransitionTo(EnemyState.DEAD);
        FindAnyObjectByType<TheTimer>().AddTime(4 *level);
        transform.LookAt(Player.transform, transform.up);
        Destroy(gameObject,2f);
    }

    public void PlayHurtSound()
    {
        if (audioSource != null && hurtClip != null)
        {
            audioSource.PlayOneShot(hurtClip,0.6f);
        }
    }

    bool CanSeePlayer()
    {
        if (Player == null)
        {
           // Debug.Log("Nema Player reference");
            return false;
        }
            float dist = Vector3.Distance(transform.position, Player.transform.position);
        if (dist > VisionRange)
        {
           // Debug.Log("Igrač je predaleko: " + dist);
            return false;
        }

        Vector3 dir = (Player.transform.position - transform.position).normalized;
        //
        Debug.DrawRay(transform.position + Vector3.up* 1.621f, dir * VisionRange, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 1.621f, dir, out RaycastHit hit, VisionRange))
        {
            
          //  Debug.Log("Raycast pogodio: " + hit.collider.name + " | Tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("Player"))
            {
                //Debug.Log("Raycast vidi igrača!");
                return true;
            }
        }
        //Debug.Log("Ne vidi igrača (Raycast blokiran)");
        return false;
    }

    bool PlayerInAttackRange()
    {
        if (Player == null || WeaponObject ==null) return false;
        return Vector3.Distance(transform.position, Player.transform.position) <= WeaponObject.GetAttackRangeForAI();
    }
}
