using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 10f;
    private Rigidbody rb;
    public GameObject hitEffectPrefab,headshotEffect;
    AudioSource source;
   public AudioClip HeadshotSFX, GunshotSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // OVO se poziva odmah prilikom instanciranja
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Debug.Log("Start pozvan");
        
        Debug.Log(rb);
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void Fire(Vector3 dir, float force) {
        GetComponent<AudioSource>().pitch = Random.Range(0.96f, 1.15f);

        if (rb != null)
        {
            Debug.Log("uslo je u petlju");
            rb.linearVelocity = dir.normalized * force;
        }
        Debug.Log(rb);
        source = GetComponent<AudioSource>();
        source.PlayOneShot(GunshotSFX,0.4f);
        // Uništi metak nakon određenog vremena
        // Destroy(gameObject, lifetime);

    }
    void OnTriggerEnter(Collider other)
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.96f, 1.15f);
        // 1. Detekcija HealthComponent-a
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null )
        {
            float damage = 25f;

            // Detekcija "headshot" pogođene visine
            float hitY = transform.position.y;
            float targetCenterY = other.bounds.center.y;
            float topY = targetCenterY + other.bounds.extents.y;

            float headshotThreshold = 0.33f; // koliko blizu vrha mora biti pogodak

            if (topY - hitY <= headshotThreshold && other.CompareTag("Enemy"))
            {
                damage *= 2; // headshot!
                headshotEffect.GetComponent<ParticleSystem>().Play();
                source.PlayOneShot(HeadshotSFX);
               
                Debug.Log("HEADSHOT!");
            }

            health.TakeDamage((int)damage);
        }

        // 2. Pusti zvuk
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        // 3. Sakrij mesh
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.enabled = false;

        // 4. Iskljuci kolajder
        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        // 5. Instanciraj particle efekat na poziciji metka (jer nema contact point)
        if (hitEffectPrefab != null)
        {
          //  Debug.Log("Trigger pogodak sa: " + other.gameObject.name);
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // 6. Uništi metak nakon kraćeg delay-a (da se završi zvuk/efekat)
        Destroy(gameObject, 2f);
    }
}
