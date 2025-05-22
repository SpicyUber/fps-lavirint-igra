using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Heart : MonoBehaviour
{
    [Header("Gameplay")]
    public Davy DavyJones;
    private HealthComponent health;

    [Header("Visual & Audio")]
    public MeshRenderer heartMesh;             // Mesh objekat srca
    public Collider heartCollider;           // Fizička kolizija
    public ParticleSystem bloodParticles;    // Efekat eksplozije
    public AudioSource audioSource;
    public UnityEvent OnHeartDestroyed;



    private Vector3 baseScale;
    private float beatTimer = 0f;

    void Start()
    {
        Debug.Log("Da li uopste radi ovo?");    
        health = GetComponent<HealthComponent>();
        if (health != null)
        {
           // health.MaxHealth = 1;
            //health.CurrentHealth = 1;
            
            health.OnDeath.AddListener(HeartExplosion); // Fix: Use AddListener to subscribe to UnityEvent
            


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
            
            audioSource.Play();
        }

        FindAnyObjectByType<TheTimer>().AddTime(15f);

        // Sakrij mesh i collider
        if (heartMesh != null)
           heartMesh.enabled = false;

        if (heartCollider != null)
            heartCollider.enabled = false;
        Debug.Log("Heart exploded!");
        
        StartCoroutine(Earthquake());
          OnHeartDestroyed.Invoke(); // Invoke the event if there are any listeners 

    }
    IEnumerator Earthquake()
    {
        float t = 2f;
        FindAnyObjectByType<ThunderManager>().Play();
        while (t > 0f) { FindAnyObjectByType<HudScript>().ExplosionCameraShake(t/24f); yield return new WaitForSeconds(0.2f); t -= 0.2f; }
        
        
    }
    public void ActivateHeart()
    {
        if (health != null)
            health.CurrentHealth = 1;
        if (heartMesh != null)
            heartMesh.enabled= true;
        if (heartCollider != null)
            heartCollider.enabled = true;
    }
}
