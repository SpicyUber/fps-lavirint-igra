using DigitalRuby.LightningBolt;
using System.Collections;
using UnityEngine;

public class ThunderManager : MonoBehaviour
{
    private LightningBoltScript[] _lightningBolts;
    public MeshRenderer SkyRenderer;
    public Color Color;
    private Color _originalColor;
    private float _t = 0;
    public float Duration;
    private bool _isShooting = false;
    private bool _isPlaying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lightningBolts = GetComponentsInChildren<LightningBoltScript>();
    }

    public void Play()
    {
        if (_isPlaying || SkyRenderer == null) return;
        _isPlaying = true;
        _originalColor = SkyRenderer.materials[0].GetColor("_Color"); SkyRenderer.materials[0].SetColor("_Color", Color);

        StartCoroutine(DarkenSky());
    }

    private void ShootThunder()
    {
        _isShooting = true;
        foreach (LightningBoltScript lightning in _lightningBolts)
        {
            lightning.Trigger();
        }

        GetComponent<AudioSource>().Play();

        StartCoroutine(UndarkenSky());
    }

    IEnumerator UndarkenSky()
    {
        yield return new WaitForSeconds(Duration);
        float t = 0;
        while (t < 0.75f)
        {
            SkyRenderer.materials[0].SetColor("_Color", Color.Lerp(Color,_originalColor , t / 0.75f));
            yield return null;
            t += Time.deltaTime;
        }
        SkyRenderer.materials[0].SetColor("_Color", _originalColor);
       _isPlaying = false;
    }
    IEnumerator DarkenSky()
    {
        float t = 0;
        while (t < 2f)
        {
            SkyRenderer.materials[0].SetColor("_Color", Color.Lerp(_originalColor,Color,t/2f));
            yield return null;
            t += Time.deltaTime;
        }
        SkyRenderer.materials[0].SetColor("_Color", Color);
        ShootThunder();
    }

    private void Update()
    {
        if (!_isShooting) return;
        _t += Time.deltaTime;
        if (_t > Duration) { _isShooting = false;  _t = 0; /*SkyRenderer.materials[0].SetColor("_Color", _originalColor);*/ }

    }
}
