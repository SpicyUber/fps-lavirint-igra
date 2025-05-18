using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public HealthComponent HealthComponent;
    Explosion ExplosionScript;
    public MeshCollider MeshCollider;
    private bool _didExplode = false;

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

        if (_didExplode) return;
        _didExplode = true;

        StartCoroutine(ExplodeRoutine(0.1f));


    }

    IEnumerator ExplodeRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnExplosionSecondHalf();
    }
    private void SpawnExplosionSecondHalf()
    {
        HealthComponent.enabled = false;

        if (ExplosionPrefab != null)
        {
            MeshCollider.enabled = false;
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            ExplosionScript = ExplosionPrefab.GetComponent<Explosion>();

            //btw, explode funkcija se poziva u Startu od explode skripte, a inicijalizuje se na poziciji bureta
        }

        Destroy(this.gameObject);


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 25);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 12.5f);
    }
}
