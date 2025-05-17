using UnityEngine;

public class Gun : MonoBehaviour, Weapon
{
    public GameObject BulletPrefab;
    public Transform BulletSpawnTransform;
    public float bulletForce = 1f;
    float attackCooldown = 1f;
    float nextAttackTime = 0f;
    public float AttackRange = 14f;
    public float attackWindUp = 0.5f;
    public float GetAttackRangeForAI()
    {
        //samo za testiranje
        return AttackRange;
    }
    public float GetWindUp() { return attackWindUp; }
    public void SetWindUp(float windUp) { if (windUp <= 0) attackWindUp = 1f; else attackWindUp = windUp; }
    public float GetCooldown() { return attackCooldown; }
    public void SetCooldown(float cooldown) { if (cooldown <= 0) attackCooldown = 1f; else attackCooldown = cooldown; }
    public bool UseAttack()
    {
        
            if (Time.time < nextAttackTime)
            {
                return false; // još uvek traje cooldown
            }

            // izvrši napad
            nextAttackTime = Time.time + attackCooldown;
           
        
        if (BulletPrefab == null || BulletSpawnTransform == null)
        {
            Debug.LogWarning("BulletPrefab ili BulletSpawnTransform nije postavljen!");
            return false;
        }

        GameObject bulletGO = Instantiate(BulletPrefab, BulletSpawnTransform.position, BulletSpawnTransform.rotation);
        
    

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            
            bullet.Fire(BulletSpawnTransform.forward, bulletForce);
        }

        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //UseAttack u update-u nikako :o
        // UseAttack();


    }
}
