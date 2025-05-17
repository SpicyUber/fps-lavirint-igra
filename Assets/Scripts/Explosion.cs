using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public ParticleSystem explosionParticles;
    public AudioSource explosionAudio;
    public ExplosiveBarrel ExplosiveBarrel;
    Collider[] hitColliders;
    Collider[] healthColliders;
    KnockbackComponent[] knockbackComps;
    HealthComponent[] healthComps;
    int currentCollider;
    public float knockForce = 50f;
    public bool _explosion_over = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explosionAudio.enabled = true; //temporary (not sure if the explode audio activates wilst being initiated; so I turned it off)
        explosionAudio.Play();
        explosionParticles.Play();
        Explode();

    }

    // Update is called once per frame
    void Update()
    {
        if(_explosion_over && !explosionAudio.isPlaying)
        {
            _explosion_over = false;
            Destroy(gameObject);
        }
    }
   
    public void Explode() {

        currentCollider = -1;
        hitColliders = Physics.OverlapSphere(transform.position, 25);
        healthColliders = Physics.OverlapSphere(transform.position, 12.5f);
        int numberOfColliders = hitColliders.Length;
        knockbackComps = new KnockbackComponent[numberOfColliders];
        healthComps = new HealthComponent[numberOfColliders];

        foreach (var collider in hitColliders)
        {
            currentCollider++;
            if (collider.GetComponent<KnockbackComponent>() != null)
            {
                knockbackComps[currentCollider] = collider.GetComponent<KnockbackComponent>();
               
                knockbackComps[currentCollider].CreateExplosion(transform.position, knockForce, 25);

            }
        }

        currentCollider = -1;

        foreach(var collider in healthColliders)
        {
            currentCollider++;
            if (collider.GetComponent<HealthComponent>() != null)
            {
                healthComps[currentCollider] = collider.GetComponent<HealthComponent>();
                healthComps[currentCollider].TakeDamage(40f);
            }
        }

        _explosion_over = true;
    }
}



