using System.Collections;
 
using UnityEngine;

public class DavyEventTrigger : MonoBehaviour
{
    public AudioSource MusicAudioSource;
    public AudioSource Wind;
    public ParticleSystem Particle;
    public AudioClip DavyIntro,DavySong;
    public MeshRenderer SkyRenderer;
    private bool _didFire=false;
    private Color _color;
    public GameObject DavyJones;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _didFire)
        {
            return;
        }
        Wind.Play();
        PlayerPrefs.SetInt("Checkpoint", 1);
        _didFire = true;
        StartCoroutine(SkyRoutine(3f, SkyRenderer.materials[0]));
        MusicAudioSource.Stop();
        MusicAudioSource.clip= DavySong;
        MusicAudioSource.PlayOneShot(DavyIntro);
        MusicAudioSource.PlayDelayed(DavyIntro.length);
        StartCoroutine(FirstThunder(DavyIntro.length-4f));
        StartCoroutine(StartDavy(DavyIntro.length));
       
        FindAnyObjectByType<TheTimer>().AddTime(20);
        foreach (Tentacle tentacle in GameObject.FindObjectsByType<Tentacle>(FindObjectsSortMode.None))
        {
            tentacle.Spawn();
        }
        foreach (Enemy enemy in GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if(enemy!= null && enemy.CurrentState!=EnemyState.DEAD) { enemy.TransitionTo(EnemyState.IDLE); } 
        }

    }
    IEnumerator StartDavy(float t)
    {
        yield return new WaitForSeconds(t);
        
        DavyJones.SetActive(true);
        yield return new WaitForSeconds(1f);
        Particle.Stop();
    }
    IEnumerator FirstThunder(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindAnyObjectByType<ThunderManager>().Play();
    }
   public void Start() { _color = SkyRenderer.materials[0].GetColor("_Color");
        SkyRenderer.materials[0].SetColor("_Color", Color.white); 
    }
    IEnumerator SkyRoutine(float duration, Material material) {
        float t = 0f;
       Color color= material.GetColor("_Color");
        while (t< duration)
        {
             
            material.SetColor("_Color",Color.Lerp(Color.white, _color, t / duration));
            yield return null;
        t+=Time.deltaTime;
        }
        Particle.Play();

    }
}
