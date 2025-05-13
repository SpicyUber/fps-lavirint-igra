using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 10f;
    private Rigidbody rb;
    public GameObject hitEffectPrefab;
    AudioSource[] sources;

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
        

        if (rb != null)
        {
            Debug.Log("uslo je u petlju");
            rb.linearVelocity = dir.normalized * 5f;
        }
        Debug.Log(rb);
        sources = GetComponents<AudioSource>();
        sources[1].Play();
        // Uništi metak nakon određenog vremena
        // Destroy(gameObject, lifetime);

    }
    void OnTriggerEnter(Collider other)
    {
        // 1. Detekcija HealthComponent-a
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            float damage = 10f;

            // Detekcija "headshot" pogođene visine
            float hitY = transform.position.y;
            float targetCenterY = other.bounds.center.y;
            float topY = targetCenterY + other.bounds.extents.y;

            float headshotThreshold = 0.2f; // koliko blizu vrha mora biti pogodak

            if (topY - hitY <= headshotThreshold)
            {
                damage *= 2; // headshot!
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

        // 4. Instanciraj particle efekat na poziciji metka (jer nema contact point)
        if (hitEffectPrefab != null)
        {
            Debug.Log("Trigger pogodak sa: " + other.gameObject.name);
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // 5. Uništi metak nakon kraćeg delay-a (da se završi zvuk/efekat)
        Destroy(gameObject, 0.5f);
    }
}
