using System.Collections;
 
using UnityEngine;

public class DavyEventTrigger : MonoBehaviour
{
    public AudioSource MusicAudioSource;
        public AudioClip DavyIntro,DavySong;
    public MeshRenderer SkyRenderer;
    private bool _didFire=false;
    private Color _color;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _didFire)
        {
            return;
        }
          
        _didFire = true;
        StartCoroutine(SkyRoutine(3f, SkyRenderer.materials[0]));
        MusicAudioSource.Stop();
        MusicAudioSource.clip= DavySong;
        MusicAudioSource.PlayOneShot(DavyIntro);
        MusicAudioSource.PlayDelayed(DavyIntro.length);
        FindAnyObjectByType<TheTimer>().CurrentTime += 20;
        foreach (Tentacle tentacle in GameObject.FindObjectsByType<Tentacle>(FindObjectsSortMode.None))
        {
            tentacle.Spawn();
        }


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
    
    }
}
