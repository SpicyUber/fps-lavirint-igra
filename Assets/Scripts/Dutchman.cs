using System.Collections;
using UnityEngine;

public class Dutchman : MonoBehaviour
{
    private PlayerScript _p;
    private bool _thrown;
    public Material Ghost;
    
    private Vector3 _startPos;
    private Quaternion _startRot;
    private Rigidbody _rb;
    private MeshRenderer[] _meshRenderers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {_rb = GetComponent<Rigidbody>();
        _startPos = transform.position;
        _startRot = transform.rotation;
        _p= FindAnyObjectByType<PlayerScript>();   
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

  public bool Throw()
    {if (_thrown) return false;
        _thrown= true;
        GetComponent<RockUpAndDown>().enabled = false;
        StartCoroutine(ThrowR());
        return true;
    }

    IEnumerator ThrowR()
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;
        Material[] ghostMats = new Material[1];
        ghostMats[0] = Ghost;
        foreach (MeshRenderer _mr in _meshRenderers) { _mr.materials = ghostMats; }
        float t = 0;
        Quaternion r = transform.rotation;
        while (t < 3f)
        {

            transform.Translate(Vector3.up * Time.deltaTime*40f);
       transform.rotation=  Quaternion.Lerp( r   ,Quaternion.LookRotation((-transform.position + _p.transform.position).normalized),t/3f);


            yield return null;
            t += Time.deltaTime;
        }
        _rb.AddForce((-transform.position + _p.transform.position).normalized*3000f);
        GetComponent<AudioSource>().Play();
        while (t < 9f) {

        foreach(  Collider cols in  Physics.OverlapSphere(transform.position + Vector3.up * 45f, 45f))
            {
                if (cols.gameObject.CompareTag("Player")) { _p.Death(); }
            }
            
            yield return null; t += Time.deltaTime;
        }
        _rb.linearVelocity= Vector3.zero;
        transform.position = _startPos;
        transform.rotation = _startRot;
        _thrown = false;
        GetComponent<RockUpAndDown>().enabled = true;
        foreach (MeshRenderer _mr in _meshRenderers) { _mr.materials = mats; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + Vector3.up*45f, 45f);
    }
}
