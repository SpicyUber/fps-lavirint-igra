using UnityEngine;
using UnityEngine.UIElements;

public class Tentacle : MonoBehaviour
{
    public Transform HearthPos;

    public HealthComponent HealthComponent;
    public ParticleSystem Particles;
    public AudioSource AudioSource;
    public Animator Animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject.SetActive(true); //temporary
        Spawn(); //4 testing
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn() {

        HealthComponent.CurrentHealth = HealthComponent.MaxHealth;
        int signX = Random.Range(1, 3);
        int signZ = Random.Range(1, 3);
        signX = (signX == 1) ? 1 : -1;
        signZ = (signZ == 1) ? 1 : -1;
        float offsetRange = Random.Range(20, 150); //may need 2 tweak the 2nd num
        transform.position = new Vector3(HearthPos.position.x + (signX * offsetRange), HearthPos.position.y, HearthPos.position.z + (signZ * offsetRange));
        gameObject.SetActive(true);
    }
    public void Flinch() {
        AudioSource.Play();
        Particles.Play();

    }
    public void FakeDespawn() {
        gameObject.SetActive(false);
    }
}
