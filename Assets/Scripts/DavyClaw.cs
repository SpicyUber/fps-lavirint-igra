using UnityEngine;

public class DavyClaw : MonoBehaviour, Weapon
{
    public Transform HurtboxCastTransform;
    public float GetAttackRangeForAI()
    {
        //samo za testiranje
        return 1f;
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
