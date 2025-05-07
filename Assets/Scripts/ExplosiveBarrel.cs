using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public HealthComponent HealthComponent;
    Explosion ExplosionScript;
    public MeshCollider MeshCollider;

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

        //HealthComponent.enabled = false;

        if (ExplosionPrefab != null)
        {
            MeshCollider.enabled = false;
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            ExplosionScript = ExplosionPrefab.GetComponent<Explosion>();

            //btw, explode funkcija se poziva u Startu od explode skripte, a inicijalizuje se na poziciji bureta
        }

        Destroy(this.gameObject);




    }
}
