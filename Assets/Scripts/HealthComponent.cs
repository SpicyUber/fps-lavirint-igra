using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    //NE DIRAJ PLIZ HVALA
    public float MaxHealth, CurrentHealth;
    public HudScript Hud;
    public UnityEvent OnDeath, OnDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponent<PlayerScript>() == null) { Hud = null; }
       // TakeDamage(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Hud != null && MaxHealth > 0)
        {
            Hud.BloodPercent = 1f- (CurrentHealth / MaxHealth);
        }
    }

    public void TakeDamage(float dmg)
    {

        CurrentHealth = Mathf.Clamp(CurrentHealth - dmg, 0, MaxHealth);
        if (CurrentHealth == 0) { OnDeath.Invoke(); }else
        if (dmg > 0) { OnDamage.Invoke(); }
       

    }

    private void OnDestroy()
    {
        OnDamage.RemoveAllListeners();
        OnDeath.RemoveAllListeners();
    }
}