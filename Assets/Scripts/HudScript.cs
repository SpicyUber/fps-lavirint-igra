using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class HudScript : MonoBehaviour
{
    [Header("Blood")]
    [Range(0, 1)]
    public float BloodPercent;
    public CanvasGroup bloodOverlay;
    public GameObject WaterOverlay;
    public TextMeshProUGUI Timer;
    public TheTimer TheTimer;
    public Image TimerIcon;
    [Header("You Died")]
    public GameObject youDiedPanel;
    public AudioListener audioListener;
    private bool _hint=false;
    public TextMeshProUGUI TauntText;
    public CinemachineImpulseSource explosionImpulseSource;
    public CinemachineImpulseSource recoilImpulseSource;
    public GameObject HintPanel;
    private bool _died = false;
    public TextMeshProUGUI DeadMessage;
    public AudioClip Beep, Select;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        TheTimer = FindAnyObjectByType<TheTimer>();
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
        float timerTime = Mathf.FloorToInt(TheTimer.CurrentTime);
        if (timerTime >= 0)
        Timer.text = " : " + timerTime;
        
        if(timerTime <=5) { Timer.color = Color.red; TimerIcon.color = Color.red; } else { Timer.color = Color.white; TimerIcon.color = Color.white; }

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
        if (_died) return;
        _died = true;
      AudioSource deathAudio=  GetComponent<AudioSource>();
        if (youDiedPanel != null)
            youDiedPanel.SetActive(true);
        AudioSource[] audioArray = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in audioArray)
        { if(deathAudio!=audio)
            audio.enabled = false;
        }

        deathAudio.Play();

        PlayerPrefs.SetInt("Death", PlayerPrefs.GetInt("Death")+1);
        int deathCount = PlayerPrefs.GetInt("Death");
        int hintCount = PlayerPrefs.GetInt("Hint");

        if ((hintCount<1 && deathCount>3) || (hintCount < 2 && deathCount > 7)) { DisplayHint(); } else {
            DeadMessage.text = "You Died";
        StartCoroutine(RestartStage(4f));
        }
    }
    private void DisplayHint() {
        Cursor.visible = true;
        foreach(Enemy enemy in FindObjectsByType<Enemy>(0)) { if(enemy!=null && enemy.gameObject!=null) enemy.gameObject.SetActive(false); }
        DeadMessage.text = ""; HintPanel.SetActive(true); string message = (PlayerPrefs.GetInt("Hint") == 0)
    ? "HAPPENS To The BeSt of US, WHICH YoU CLEARLY ARENT."
    : "You okay dude?"; TauntText.text = $"you've died {PlayerPrefs.GetInt("Death")} timeS.\n{message}\nwanna make the game eaSIER?"; }

    public void Hover()
    {
        GetComponent<AudioSource>().PlayOneShot(Beep);
    }
    public void RestartButton()
    {
        if (_hint) return;
        _hint = true;
        GetComponent<AudioSource>().PlayOneShot(Select);
        StartCoroutine(RestartStage(1f));
    }

    public void RestartWithHintButton()
    {
        if (_hint) return;
        _hint = true;
        PlayerPrefs.SetInt("Hint", PlayerPrefs.GetInt("Hint")  + 1);
        GetComponent<AudioSource>().PlayOneShot(Select);
        StartCoroutine(RestartStage(1f));
    }
    IEnumerator RestartStage(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
}
