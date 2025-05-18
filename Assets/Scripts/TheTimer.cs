using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class TheTimer : MonoBehaviour
{
    public float StartTime = 10f;
    public float CurrentTime = 10f;
    public GameObject Water;
    private bool _flooding = false;
    private AudioSource _source;
    public TimerPlusAnimation SpriteAnimation;
    private float _stretchFactor = 1.4f;
    private float _duration = 0.15f;
    private Vector3 originalScale;
    public RectTransform TimerPanel;
    public AudioSource DrownSound;
    public void AddTime(float time)
    {
        if (_flooding) return;
        StartCoroutine(TimeAnimation());
       
        CurrentTime += time;
        SpriteAnimation.Play(CurrentTime <= 5f);
    }
    private IEnumerator TimeAnimation()
    {
        Vector3 originalScale = TimerPanel.localScale;
        Vector3 targetScale = originalScale * _stretchFactor;

        
        Vector3 worldPos = TimerPanel.position;

        float time = 0f;
        while (time < _duration)
        {
            TimerPanel.localScale = Vector3.Lerp(originalScale, targetScale, time / _duration);
            TimerPanel.position = worldPos;  
            time += Time.deltaTime;
            yield return null;
        }
        TimerPanel.localScale = targetScale;
        TimerPanel.position = worldPos;

        time = 0f;
        while (time < _duration)
        {
            TimerPanel.localScale = Vector3.Lerp(targetScale, originalScale, time / _duration);
            TimerPanel.position = worldPos;
            time += Time.deltaTime;
            yield return null;
        }
        TimerPanel.localScale = originalScale;
        TimerPanel.position = worldPos;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
        originalScale = TimerPanel.localScale;
        _source = GetComponent<AudioSource>();
        
    }

    private void Awake()
    {
        CurrentTime = StartTime;
    }

    // Update is called once per frame
    void Update()
    {
        if ((CurrentTime > 5 || CurrentTime < 0) && _source.isPlaying) _source.Stop();
        if (_flooding) return;

    if(CurrentTime <= 5 && CurrentTime >=0 && !_source.isPlaying) { _source.Play();  }
        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f) Flood();
    }
    void Flood()
    { _flooding = true;
       DrownSound.Play();
        StartCoroutine(FloodRoutine());
    }

    IEnumerator FloodRoutine()
    {
        float t = 0;
        while (t<6f) {
            Water.transform.Translate(Vector3.up * 5f * Time.deltaTime);
        yield return null;
            if(t>2f) FindAnyObjectByType<HudScript>().WaterOverlay.SetActive(true);
            t += Time.deltaTime;
        }
        FindAnyObjectByType<HudScript>().YouDied();
    }
}
