using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    //NE DIRAJ PLIZ HVALA
    public float MaxHealth,CurrentHealth ;
    public HudScript Hud;
    public UnityEvent OnDeath, OnDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponent<PlayerScript>() == null) { Hud = null; }
    }

    // Update is called once per frame
    void Update()
    {
        if(Hud != null && MaxHealth>0)
        {
            Hud.BloodPercent = (CurrentHealth/MaxHealth)*100;
        }
    }

    public void TakeDamage(float dmg) {
    
        CurrentHealth = Mathf.Clamp(CurrentHealth-dmg, 0, MaxHealth);

        if(dmg > 0) { OnDamage.Invoke(); }else
        if (CurrentHealth == 0) { OnDeath.Invoke(); }
    
    }

    private void OnDestroy()
    {
        OnDamage.RemoveAllListeners();
        OnDeath.RemoveAllListeners();
    }
}
