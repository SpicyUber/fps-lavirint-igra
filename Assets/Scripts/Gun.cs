using UnityEngine;

public class Gun : MonoBehaviour, Weapon
{
    public GameObject BulletPrefab;
    public Transform BulletSpawnTransform;
    public float GetAttackRangeForAI()
    {
        //samo za testiranje
        return 14f;
    }

    public bool UseAttack()
    {
        //samo za testiranje
        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
