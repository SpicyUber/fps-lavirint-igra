using UnityEngine;

public class Davy : Enemy
{
    public Shield ShieldComponent;
    public Heart[] Hearts;
    public int HeartIndex;
    public Tentacle[] Tentacles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
  
    public override void Func()
    {
        base.Func();
        Hearts = FindObjectsByType<Heart>(0);
        Tentacles = FindObjectsByType<Tentacle>(0);
    }

    // Update is called once per frame

    public override void FuncUpdate()
    {
        base.FuncUpdate();

    }

    public void StunSelfAndTakeDownShield() { }
    public void ActivateNextHeart() { }
    public void RespawnAllTentacles() { }


}
