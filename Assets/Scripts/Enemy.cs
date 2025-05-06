using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Weapon WeaponObject;
    public EnemyState CurrentState;
    public NavMeshAgent Agent;
    public bool ChasesPlayer;
    public float MoveSpeed;
    public PlayerScript Player; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //switch(state){case state1 : state1(); break; itd}    
    }

    public void StunSelf() { }

    public void TransitionTo(EnemyState state)
    {

    }

    public void Death() { }

    public void PlayHurtSound() { }




}
