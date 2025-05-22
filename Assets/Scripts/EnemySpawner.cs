using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemies;
    
    private Enemy[] _enemies; //_enemy 0 is sword enemy, enemy 1 is gun enemy
   
    private bool[] _markForDeaths;
    private bool[] _isSpawnings;


    public void Start()
    {
        _enemies = new Enemy[2];
        _markForDeaths = new bool[2];
        _isSpawnings = new bool[2];
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = hit.point;
            transform.up = hit.normal;

        }

        

    }
    public void Spawn(int index)
    {
        if (index < 0 || index > 1) return;

        if (_enemies[index] == null)
        {
            _enemies[index] = Instantiate(Enemies[index],transform.parent).GetComponent<Enemy>();
            _enemies[index].transform.position = transform.position;
           
            StartCoroutine(SpawnRoutine(index));
         
        }

        GetComponentInChildren<ParticleSystem>().Play();


    }

    IEnumerator SpawnRoutine(int index)
    {
        _isSpawnings[index] = true;   
        yield return new WaitForSeconds(1f);
        _enemies[index].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _enemies[index].Animator.ResetTrigger("IDLE");
        _enemies[index].TransitionTo(EnemyState.CHASE);
        _isSpawnings[index] = false;
        if (_markForDeaths[index]) Kill(index);
        
    }

    public void Kill(int index)
    {
        if (index < 0 || index > 1) return;
        if (_isSpawnings[index]) { _markForDeaths[index] = true; return; }
        if (_enemies[index] != null) { _enemies[index].Death(); _enemies[index] = null; _markForDeaths[index] = false; }
    }
}
