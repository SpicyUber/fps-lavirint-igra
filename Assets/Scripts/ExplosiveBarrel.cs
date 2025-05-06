using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public HealthComponent HealthComponent;

    void Awake()
    {
        if(HealthComponent == null || !HealthComponent.enabled)
        {
            HealthComponent.enabled = true;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnExplosion() {

        HealthComponent.enabled = false;

        if (ExplosionPrefab != null) 
        { 
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            //btw, explode funkcija se poziva u Startu od explode skripte
        }

        Destroy(gameObject);


    }
}
