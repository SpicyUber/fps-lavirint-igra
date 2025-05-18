using System.Collections;
using UnityEngine;

public class TheTimer : MonoBehaviour
{
    public float StartTime = 20f;
    public float CurrentTime = 20f;
    public GameObject Water;
    private bool _flooding = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTime = StartTime;
    }

    // Update is called once per frame
    void Update()
    { if (_flooding) return;
        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f) Flood();
    }
    void Flood()
    { _flooding = true;
        StartCoroutine(FloodRoutine());
    }

    IEnumerator FloodRoutine()
    {
        float t = 0;
        while (t<3f) {
            Water.transform.Translate(Vector3.up * 10f * Time.deltaTime);
        yield return null;
            if(t>1f) FindAnyObjectByType<HudScript>().WaterOverlay.SetActive(true);
            t += Time.deltaTime;
        }
        FindAnyObjectByType<HudScript>().YouDied();
    }
}
