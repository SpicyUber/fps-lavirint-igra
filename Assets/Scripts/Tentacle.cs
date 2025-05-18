using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Tentacle : MonoBehaviour
{
    public Transform HearthPos;

    public HealthComponent HealthComponent;
    public ParticleSystem Particles;
    public AudioSource AudioSource;
    public Animator Animator;
    private SkinnedMeshRenderer _renderer;
    private Collider _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer= GetComponentInChildren<SkinnedMeshRenderer>();
        _collider= GetComponent<Collider>();
        AudioSource.volume = 0.4f;
        FakeDespawn(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn() {
        StartCoroutine(SpawnCoroutine(Random.Range(0.1f,0.5f)));

        
    }
    public void Flinch() {
        Animator.SetTrigger("Flinch");
        AudioSource.Play();
        Particles.Play();

    }

    public void FakeDespawn()
    {
        FakeDespawn(false);
    }
    public void FakeDespawn(bool noFX) {
        if (!noFX) { 
        AudioSource.Play();
        Particles.Play();
            FindAnyObjectByType<TheTimer>().CurrentTime += 3;
            Animator.SetTrigger("Despawn");
        }
        _collider.enabled = false;
        if (noFX) { _renderer.enabled = false; } else StartCoroutine(FakeDespawnCoroutine());
        HealthComponent.enabled = false;
        // gameObject.SetActive(false);
    }

    IEnumerator FakeDespawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        _renderer.enabled = false;
      
    }

    IEnumerator SpawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        HealthComponent.enabled = true;
        HealthComponent.CurrentHealth = HealthComponent.MaxHealth;
        /*  int signX = Random.Range(1, 3);
          int signZ = Random.Range(1, 3);
          signX = (signX == 1) ? 1 : -1;
          signZ = (signZ == 1) ? 1 : -1;
          float offsetRange = Random.Range(20, 150); //may need 2 tweak the 2nd num
          transform.position = new Vector3(HearthPos.position.x + (signX * offsetRange), HearthPos.position.y, HearthPos.position.z + (signZ * offsetRange));
         */
        //  gameObject.SetActive(true);
        _renderer.enabled = true;
        _collider.enabled = true;
        Animator.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
           Particles.transform.position = other.transform.position;
        }
    }
}
