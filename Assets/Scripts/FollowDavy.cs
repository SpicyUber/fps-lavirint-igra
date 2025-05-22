using UnityEngine;

public class FollowDavy : MonoBehaviour
{
    private Davy _davy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _davy = FindAnyObjectByType<Davy>();
    }

    // Update is called once per frame
    void Update()
    {if (_davy == null) return;
        transform.position = new Vector3(_davy.transform.position.x, _davy.transform.position.y,_davy.transform.position.z) - Vector3.one;
    }
}
