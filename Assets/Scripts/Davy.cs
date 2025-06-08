using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Davy : Enemy
{
    public Shield ShieldComponent;
    public Heart[] Hearts;
    public int HeartIndex;
    public int brojac;
    public Tentacle[] Tentacles;
    private HealthComponent health;
    public GameObject HpBar;
    private bool _isGhost = false;
    public Material GhostMaterial;
    public Image Victory;
    private Material[] _matArray;
    public EnemySpawner[] Spawners;
    public ParticleSystem Particles;
    public Slider HpBarSlider;
    private float _t=0f;
    public float AbilityInterval = 10f;
    public AudioClip SpawnEnemiesSFX1,SpawnEnemiesSFX2, ThrowBoatSFX, SpellSFX, TeleportSFX1, TeleportSFX2, DavyDeathSFX;
    private bool _kneelQueueUp=false;
    private bool _victory = false;
    private float _winningTime=1f;
    private float _startT;
    public AudioSource Music;
    public TextMeshProUGUI VictoryText;
    private bool _instakill = true;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void TurnGhost()
    {
        if (_isGhost) { return; }
        _isGhost = true;
        Material[] materials = new Material[GetComponentInChildren<SkinnedMeshRenderer>().materials.Length];
        for ( int i =0;i<materials.Length;i++ ) {
            materials[i] = GhostMaterial;
}
        GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;
    }
    public void SpawnEnemies(int index)
    {
       
        if (index == 1) { audioSource.PlayOneShot(SpawnEnemiesSFX2); } else { audioSource.PlayOneShot(SpawnEnemiesSFX1);  }
        audioSource.PlayOneShot(SpellSFX);
        foreach (EnemySpawner spawner in Spawners) { spawner.Spawn(index); }
    }
    public void KillEnemies(int index)
    {
        foreach (EnemySpawner spawner in Spawners) { spawner.Kill(index); }
    }
    public void Teleport()
    {
        if(_isGhost) { return; }
        audioSource.PlayOneShot(TeleportSFX1);
        TurnGhost();
        
        StartCoroutine(TeleportRoutine());
    }

    IEnumerator TeleportRoutine()
    {
        VisionRange = 0;
        TransitionTo(EnemyState.IDLE);
        Agent.enabled = false;
        float targetY = transform.position.y-5f;

        while (transform.position.y > targetY)
        {
            transform.Translate(new(0,-Time.deltaTime*6f,0), Space.World); 
            yield return null;
        }
        Vector3 playerToEnemy = (transform.position - Player.transform.position);
        playerToEnemy.y = 0;
        while (playerToEnemy.magnitude > 4f)
        {
            Vector3 temp = (-transform.position + Player.transform.position);
            temp.y = 0;
            transform.Translate(temp.normalized * Time.deltaTime * 30f,Space.World);
            yield return null;
            playerToEnemy = (transform.position - Player.transform.position);
            playerToEnemy.y = 0;
        }

        targetY = transform.position.y + 5f;
        audioSource.PlayOneShot(TeleportSFX2);
        while (transform.position.y < targetY)
        {
            transform.Translate(new(0, Time.deltaTime * 6f, 0), Space.World);
            yield return null;
        }

        if (CurrentState != EnemyState.KNEEL) { VisionRange = 100; }
        Agent.enabled = true;
        TurnNormal();
        
    }

    public void TurnNormal()
    {
        if(!_isGhost) { return; }

        _isGhost = false;
        GetComponentInChildren<SkinnedMeshRenderer>().materials=_matArray;
       
    }
    public override void Func()
    {
        base.Func();
        //  TurnGhost();
        
        Hearts = FindObjectsByType<Heart>(0);
        Tentacles = FindObjectsByType<Tentacle>(0);
        Spawners = FindObjectsByType<EnemySpawner>(0);
        _matArray = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        // SpawnEnemies();
        //  Teleport();
        NerfDavy();
        HpBar.SetActive(true);
        for (int i = 0; i < Hearts.Length; i++)
        {
            int index = i; // Bitno zbog lambdi
            Hearts[i].OnHeartDestroyed.AddListener(() => OnHeartDestroyed(index));
        }
       health= GetComponent<HealthComponent>();
       // Teleport();
        ActivateNextHeart();

    }

    private void NerfDavy()
    {
        if (PlayerPrefs.GetInt("Hint") > 0) { GetComponent<HealthComponent>().MaxHealth = Mathf.Clamp(GetComponent<HealthComponent>().MaxHealth - 150f * PlayerPrefs.GetInt("Hint"), 25f, int.MaxValue); GetComponent<HealthComponent>().CurrentHealth = GetComponent<HealthComponent>().MaxHealth; }
        AbilityInterval = AbilityInterval + PlayerPrefs.GetInt("Hint") * 5f;
        if (PlayerPrefs.GetInt("Hint") > 1) { _instakill = false; }
    }

    public void OnHeartDestroyed(int index)
    {
        Debug.Log("Davy: Srce " + index + " uništeno.");
        brojac++;
        KillEnemies(0);
        KillEnemies(1);
        StunSelfAndTakeDownShield();

        
        
            RespawnAllTentacles();
        

        // Idi na sledeće srce
        HeartIndex = (index + 1) % Hearts.Length;
    }
    // Update is called once per frame

    public override void FuncUpdate()
    {
        base.FuncUpdate();
        UpdateHPBar();
        UseAbility();
        
        if (_victory)
        {
            VictoryFunc();
        }
        _t += Time.deltaTime;
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
    private void VictoryFunc()
    {
        FindAnyObjectByType<TheTimer>().CurrentTime = _winningTime;
        FindAnyObjectByType<HudScript>().ExplosionCameraShake(0.075f);
        Time.timeScale = 1f;
        if (_t < _startT + 5f)
            Victory.color = new Color(1, 1, 1, (_t-_startT) / (5f));
    /*added a false here so its removed for the itch io build*/    else if(_t<0f && false) { Victory.color = new Color(1, 1, 1, 1);
            VictoryText.text = $"Iskoristili ste pomoĆ.  \r\n\r\n\r\nVreme je za naplatu. \r\novo je ekran koji traći vreme.\r\n Možete koristiti sledećih {Mathf.FloorToInt(Mathf.Abs(_t))} sekundi za razmišljanje. \r\nRazmislite zašto ste toliko loši da vam je trebala pomoć.\r\n\r\n\r\nhahahahahahaha\r\n";


        }
        else
        {
            Victory.color = new Color(1, 1, 1, 1);
            //  VictoryText.text = "Hrabra posado, avantura vam se bliži kraju.  \r\n\r\n\r\nPred vama je novi izazov, pravi piratski. \r\nPRAVI PIRATI NE ZNAJU ZA SRAMotu.\r\nPotrebno je koračati oronulim putanjama kalemegdana, pretražiti svaki kutak, proviriti kroz najzapušteniju kapiju u potrazi za strancem.\r\nstranac mora snimiti glasovnu poruku u kojoj na svom maternjem jeziku govori...\r\n\r\n\r\n<i>Davy Davy eggs and gravy, come to me and the beating will be heavy.</i>\r\n";
            VictoryText.text = "Congrats!\nDavy Jones has been defeated and the curse has been broken.\nReturning to menu in "+Mathf.FloorToInt(45f-_t)+" seconds.\n\nCREDITS:\nLead Programmer And Game Designer - Aleksandar Ilanković\nLevel Design - Aleksandar Ilanković\nPlayer Logic - Aleksandar Ilanković\nUI - Luka Milikić,Aleksandar Ilanković\nExplosion & Explosive Barrel Logic - Teodor Vicelarević\nWeapons Logic - Milan Davidović\nHealth & Knockback Systems - Aleksandar Ilanković \nEnemy AI - Luka Milikić \n Boss Fight Logic - Aleksandar Ilanković, Milan Davidović\nBoss Fight original Model - Danica Živković\nMusic Production - Aleksandar Ilanković\noriginal Compositions - Bobby Prince, Hans Zimmer\nAnimation - Aleksandar Ilanković";
        }

        if (_t > 40f) { VictoryText.text = "\r\n\r\n\r\n\r\n\r\nSpecial THANKS To\r\n\r\n-Andrijana ČuČiloviĆ\r\n\r\n-Veljko DjuriĆ\r\n\r\n-MARKo DapČeviĆ\n\nReturning to menu in "+Mathf.FloorToInt(45f-_t)+" seconds.";  }
        if (_t > 45f) { SceneManager.LoadScene(0); }
    }
    public void UseAbility()
    {
        if (CurrentState == EnemyState.DEAD) return;
        if (_t > AbilityInterval)
        {
            if(CurrentState== EnemyState.KNEEL || CurrentState == EnemyState.STUNNED) { 
            _t = AbilityInterval-1;
            }
            else
            {
                _t = 0;
                if (health.CurrentHealth / health.MaxHealth > 0.99) { 
                    if((Player.transform.position-transform.position).magnitude>8f)
                Teleport();
                    
                }
                else if(health.CurrentHealth / health.MaxHealth > 0.51)
                {
                    int num = Random.Range(1, 3);
                    switch (num)
                    {
                        case 1:
                            if ((Player.transform.position - transform.position).magnitude > 0.66f)
                                Teleport();
                            else { SpawnEnemies(0); _t = -5; }
                            break;
                            case 2:
                            SpawnEnemies(0); _t = -5;
                            break;

                    }

                }
                else if ((health.CurrentHealth / health.MaxHealth > 0.33) || !_instakill)
                {
                    int num = Random.Range(1, 4);
                    switch (num)
                    {
                        case 1:
                            if ((Player.transform.position - transform.position).magnitude > 8f)
                                Teleport();
                            else { SpawnEnemies(Random.Range(0,2)); _t = -5; }
                            break;
                        case 2:
                            SpawnEnemies(1); _t = -5;
                            break;
                        case 3:
                            SpawnEnemies(0); _t = -5;
                            break;

                    }

                }
                else
                {
                    int num = Random.Range(1, 5);
                    switch (num)
                    {
                        case 1:
                            if ((Player.transform.position - transform.position).magnitude > 8f)
                                Teleport();
                            else _t = AbilityInterval - 1;
                            break;
                        case 2:
                            SpawnEnemies(1);
                            break;
                        case 3:
                            ThrowShip();
                            break;
                        case 4:
                            SpawnEnemies(0);
                            break;

                    }
                }

            }
        }
        
    }

    private void ThrowShip()
    {
        if (FindAnyObjectByType<Dutchman>().Throw()) audioSource.PlayOneShot(ThrowBoatSFX);
    }

    public void UpdateHPBar()
    {
        if (HpBarSlider == null) return;
        HpBarSlider.value = health.CurrentHealth/ health.MaxHealth;

    }
    public void StunSelfAndTakeDownShield() {
      
        StartCoroutine(ShieldDown());
        


    }
    public void ActivateNextHeart() {
        if(_victory)return;
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
        while(_isGhost) { yield return null; }
        TransitionTo(EnemyState.KNEEL);
       
        ShieldComponent.ShieldDown();
        yield return new WaitForSeconds(13f);
      if(!_victory)  ShieldComponent.ShieldUp();
        yield return new WaitForSeconds(2f);
        ActivateNextHeart();
       
        VisionRange = 100;
        TransitionTo(EnemyState.CHASE);
    }

    public void Bleed()
    {
        Particles.Play();
    }

    public void DavyDeath()
    {
        _victory = true;
        /*commented part of the line here so its removed for the itch io build*/
        _startT = 0 /*- 60 * PlayerPrefs.GetInt("Hint")*/;
        _t = _startT;
        Music.Stop();
        audioSource.PlayOneShot(DavyDeathSFX);
        _winningTime = FindAnyObjectByType<TheTimer>().CurrentTime;
        KillEnemies(0);
        KillEnemies(1);
    }

    public void KillAllTentacles()
    {
        foreach(Tentacle t in Tentacles)
        {
            if(t._collider.enabled)
            t.FakeDespawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) {Particles.transform.position = other.transform.position;  }
    }

}
