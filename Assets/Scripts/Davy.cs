using System.Collections;
using UnityEngine;

public class Davy : Enemy
{
    public Shield ShieldComponent;
    public Heart[] Hearts;
    public int HeartIndex;
    public int brojac;
    public Tentacle[] Tentacles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
  
    public override void Func()
    {
        base.Func();
        Hearts = FindObjectsByType<Heart>(0);
        Tentacles = FindObjectsByType<Tentacle>(0);
        for (int i = 0; i < Hearts.Length; i++)
        {
            int index = i; // Bitno zbog lambdi
            Hearts[i].OnHeartDestroyed.AddListener(() => OnHeartDestroyed(index));
        }

        ActivateNextHeart();

    }
    public void OnHeartDestroyed(int index)
    {
        Debug.Log("Davy: Srce " + index + " uništeno.");
        brojac++;

        StunSelfAndTakeDownShield();

        // Ako je ovo treće srce, respawnuj tentakle
        if (index == 2)
        {
            RespawnAllTentacles();
        }

        // Idi na sledeće srce
        HeartIndex = (index + 1) % Hearts.Length;
    }
    // Update is called once per frame

    public override void FuncUpdate()
    {
        base.FuncUpdate();
        
        //if (Hearts[HeartIndex].heartMesh.enabled == false)
        //{
        //    Debug.Log(HeartIndex);
        //    Debug.Log(brojac);
        //    brojac++;
        //    Debug.Log("Heart is not enabled");
        //    StunSelfAndTakeDownShield();
            
        //    if (HeartIndex < Hearts.Length - 1)
        //    {
        //        HeartIndex++;
        //    }
        //    else
        //    {
        //        HeartIndex = 0;
        //    }
        //    if (HeartIndex == 2)
        //    {
        //        RespawnAllTentacles();
        //    }
        //}
        }

    

    public void StunSelfAndTakeDownShield() {
      
        StartCoroutine(ShieldDown());
        


    }
    public void ActivateNextHeart() {
        Hearts[HeartIndex].ActivateHeart();
        
    }
    public void RespawnAllTentacles() { 
        for (int i = 0; i < Tentacles.Length; i++)
        {
            if (Tentacles[i]._collider.enabled == false) {
                Tentacles[i].Spawn();
            }
            
            
        }
    }
    IEnumerator ShieldDown()
    {
        ShieldComponent.ShieldDown();
        yield return new WaitForSeconds(5f);
        ShieldComponent.ShieldUp();
        ActivateNextHeart();

    }

}
