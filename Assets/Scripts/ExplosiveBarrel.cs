
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public HealthComponent HealthComponent;
    public MeshRenderer MeshRenderer;
    Explosion ExplosionScript;
    public MeshCollider MeshCollider;
    private bool _didExplode = false;
    private Color neededColor;
    struct ColBoost {
        public float r;
        public float g;
        public float b;
    }
    ColBoost colBoost;
    int sign = -1;
    float timer = 0;
    float interval = 0.1f;
    bool _should_timer_run = true;

    void Awake()
    {
        if (HealthComponent == null || !HealthComponent.enabled)
        {
            HealthComponent.enabled = true;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colBoost.r = 0.1f;
        colBoost.g = 0.02f;
        colBoost.b = 0.01f;
        neededColor = MeshRenderer.material.color;
        MeshRenderer.material.color = new Color( 1, 1, 1, 1 );
        ;
    }

    // Update is called once per frame
    void Update()
    {
        if (_should_timer_run)
        {
            if (timer < interval)
            {
                timer += Time.deltaTime;
            }

            if (timer >= interval)
            {
                timer = 0;
                _should_timer_run = false;
                ModulateColor();
            }
        }
    }

    public void ModulateColor()
    {
        if(MeshRenderer.material.color.g <= neededColor.g && MeshRenderer.material.color.b > neededColor.b)
        {
            sign = 1;
        }

        if(MeshRenderer.material.color.g >= Color.white.g && MeshRenderer.material.color.b >= Color.white.b)
        {
            sign = -1;
        }

        if ((MeshRenderer.material.color.g > neededColor.g && sign < 0) || (MeshRenderer.material.color.g < Color.white.g && sign > 0))
        {
            MeshRenderer.material.color = MeshRenderer.material.color + sign * new Color(0, colBoost.g, 0, 0);
        }

        if((MeshRenderer.material.color.b > neededColor.b && sign < 0) || (MeshRenderer.material.color.b < Color.white.b && sign > 0))
        {
            MeshRenderer.material.color = MeshRenderer.material.color + sign * new Color(0, 0, colBoost.b, 0);
        }

        _should_timer_run = true;
    }

    public void SpawnExplosion()
    {

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
