using UnityEngine;

public class KnockbackComponent : MonoBehaviour
{
    //NE DIRAJ PLIZ HVALA
    public float KnockbackMultiplier;
    public HudScript Hud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponent<PlayerScript>() == null) { Hud = null; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateExplosion(Vector3 knockbackLocation,float force,float radius) {
        Rigidbody rb = GetComponent<Rigidbody>();


        if (rb==null) return;
        rb.AddExplosionForce(force, knockbackLocation, radius);
        if (Hud != null)
        {
            Hud.ExplosionCameraShake(10);
        }
    }
}
