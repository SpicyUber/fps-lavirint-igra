using UnityEngine;

public class Heart : MonoBehaviour
{
    [Header("Gameplay")]
    public Davy DavyJones;
    private HealthComponent health;

    [Header("Visual & Audio")]
    public GameObject heartMesh;             // Mesh objekat srca
    public Collider heartCollider;           // Fizička kolizija
    public ParticleSystem bloodParticles;    // Efekat eksplozije
    public AudioSource audioSource;
    

    private Vector3 baseScale;
    private float beatTimer = 0f;

    void Start()
    {
        Debug.Log("Da li uopste radi ovo?");    
        health = GetComponent<HealthComponent>();
        if (health != null)
        {
            health.MaxHealth = 1;
            health.CurrentHealth = 1;
            audioSource.Play();
           health.OnDeath.AddListener(HeartExplosion); // Fix: Use AddListener to subscribe to UnityEvent
           health.TakeDamage(1);
            

        }

        if (heartMesh != null)
            baseScale = heartMesh.transform.localScale;
    }

    void Update()
    {
        AnimateHeartbeat();
    }

    private void AnimateHeartbeat()
    {
        if (heartMesh == null) return;

        beatTimer += Time.deltaTime * 4f;
        float scaleFactor = 1f + Mathf.Sin(beatTimer) * 0.05f;
        heartMesh.transform.localScale = baseScale * scaleFactor;
    }

    public void HeartExplosion()
    {

        // Particle efekat
        if (bloodParticles != null)
            Debug.Log("Blood particles are not null");
        bloodParticles.Play();

        // Zvuk eksplozije
        if (audioSource != null)
        {
            Debug.Log("Audio source are not null");
            audioSource.enabled = true;
            audioSource.Play();
        }

        // Sakrij mesh i collider
        if (heartMesh != null)
            heartMesh.SetActive(false);

        if (heartCollider != null)
            heartCollider.enabled = false;
        Debug.Log("Heart exploded!");
    }

    public void ActivateHeart()
    {
        if (health != null)
            health.CurrentHealth = 1;
        if (heartMesh != null)
            heartMesh.SetActive(true);

        if (heartCollider != null)
            heartCollider.enabled = true;
    }
}
