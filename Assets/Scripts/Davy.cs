using UnityEngine;

public class Davy : Enemy
{
    public Shield ShieldComponent;
    public Heart[] Hearts;
    public int HeartIndex;
    public Tentacle[] Tentacles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StunSelfAndTakeDownShield() { }
    public void ActivateNextHeart() { }
    public void RespawnAllTentacles() { }


}
