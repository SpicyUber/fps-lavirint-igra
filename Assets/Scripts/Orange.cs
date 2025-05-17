using UnityEngine;

public class Orange : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float floatHeight = 0.5f;
    public float rotationSpeed = 50f;
    public AudioClip pickupSound;

    private Vector3 startPos;
    private AudioSource audioSource;
    private bool isCollected = false;

    void Start()
    {
        startPos = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        FloatAndRotate();
    }

    private void FloatAndRotate()
    {
        // Vertical floating effect
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Constant rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(-50); // Healing effect
            PlayPickupSound();
            GetComponent<MeshRenderer>().enabled = false;
           
            isCollected = true;
            Destroy(gameObject, pickupSound != null ? pickupSound.length : 0f); // Delay destroy for sound
        }
    }
    private void PlayPickupSound()
    {
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }
    }
}
