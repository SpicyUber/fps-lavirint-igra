using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class TheTimer : MonoBehaviour
{
    public float StartTime = 40f;
    public float CurrentTime = 40f;
    public GameObject Water;
    private bool _flooding = false;
    private AudioSource _source;
    public TimerPlusAnimation SpriteAnimation;
    private float _stretchFactor = 1.4f;
    private float _duration = 0.15f;
    private Vector3 originalScale;
    public RectTransform TimerPanel;
    public AudioSource DrownSound;
    private float _t=0;
    public float WaterRockWidth;
    private float _startZ,_startX,_startY;
    private Coroutine _coroutine;
    public void AddTime(float time)
    {
        time = time + PlayerPrefs.GetInt("Hint")*0.5f*time;
        if (_flooding) return;

        if (_coroutine != null) StopCoroutine(_coroutine);
       _coroutine = StartCoroutine(TimeAnimation());
       
        CurrentTime += time;
        SpriteAnimation.Play(CurrentTime <= 5f);
    }
    private IEnumerator TimeAnimation()
    {
        
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
        _startZ = transform.position.z;
        _startX = transform.position.x;
        _startY = transform.position.y;
        _t = 0;
        originalScale = TimerPanel.localScale;
        _source = GetComponent<AudioSource>();
        
    }

    private void Awake()
    {
        CurrentTime = StartTime + PlayerPrefs.GetInt("Hint")* 0.5f * StartTime;

    }

    // Update is called once per frame
    void Update()
    {
       
        if ((CurrentTime > 5 || CurrentTime < 0) && _source.isPlaying) _source.Stop();
        if (_flooding) return;
        RockWater();
    if(CurrentTime <= 5 && CurrentTime >=0 && !_source.isPlaying) { _source.Play();  }
        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f) Flood();
        _t += Time.deltaTime;
    }

    public void RockWater()
    {
        Water.transform.position = new Vector3(_startX, Water.transform.position.y, _startZ + Mathf.Sin(_t/4)* WaterRockWidth);

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
