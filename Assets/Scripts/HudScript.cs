using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HudScript : MonoBehaviour
{
    [Header("Blood")]
    [Range(0, 1)]
    public float BloodPercent;
    public CanvasGroup bloodOverlay;

    [Header("You Died")]
    public GameObject youDiedPanel;
    public AudioListener audioListener;

    public CinemachineImpulseSource explosionImpulseSource;
    public CinemachineImpulseSource recoilImpulseSource;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (explosionImpulseSource == null)
            Debug.LogWarning("Explosion impulse source not assigned!");

        if (recoilImpulseSource == null)
            Debug.LogWarning("Recoil impulse source not assigned!");

        if (explosionImpulseSource != null)
        {
            explosionImpulseSource.ImpulseDefinition = new CinemachineImpulseDefinition
            {
                ImpulseChannel = 1,
                ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion,
                ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform,
                ImpulseDuration = 0.3f,
                DissipationDistance = 100,
                DissipationRate = 0.5f,
                PropagationSpeed = 300
            };
            explosionImpulseSource.DefaultVelocity = Vector3.up;
        }

        if (recoilImpulseSource != null)
        {
            recoilImpulseSource.ImpulseDefinition = new CinemachineImpulseDefinition
            {
                ImpulseChannel = 2,
                ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil,
                ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform,
                ImpulseDuration = 0.2f,
                DissipationDistance = 50,
                DissipationRate = 0.4f,
                PropagationSpeed = 300
            };
            recoilImpulseSource.DefaultVelocity = Vector3.back;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bloodOverlay != null)
        {
            bloodOverlay.alpha = BloodPercent;
        }
    }

    public void ExplosionCameraShake(float intensity) {
        if (explosionImpulseSource != null)
        {
            explosionImpulseSource.GenerateImpulse(Vector3.up * intensity);
        }
    }
    public void RecoilCameraShake(float intensity) {
        if (recoilImpulseSource != null)
        {
            recoilImpulseSource.GenerateImpulse(-Vector3.up * intensity/4);
        }
    }

    public void YouDied() {
      AudioSource deathAudio=  GetComponent<AudioSource>();
        if (youDiedPanel != null)
            youDiedPanel.SetActive(true);
        AudioSource[] audioArray = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in audioArray)
        { if(deathAudio!=audio)
            audio.enabled = false;
        }

        deathAudio.Play();

        
        StartCoroutine(RestartStage());
    }

    IEnumerator RestartStage()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
}
