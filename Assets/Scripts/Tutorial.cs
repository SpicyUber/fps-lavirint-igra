using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Tooltip("The sprite that is displayed on screen as the tutorial.")]
    public Sprite TutorialImage;
    [Tooltip("The image component which will hold the sprite and display it on a canvas.")]
    public Image Image;
    private bool _started; // a flag for if the animation has started
    private float _t = 0; //timer for the fade in and fade out
    private float _otherT = 0; //timer for staying on screen
    private float _lastFrameSinT = -1; // value of the sinus of _t from previous frame
    private float _sinT = 0; //value of the sinus of _t in the current frame

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            _started = true;
          
            _t = 0;
            Image.sprite = TutorialImage;
            GetComponent<Collider>().enabled = false;
        }
    }

     void Update()
    {
        if (!_started ) return;
        _sinT = Mathf.Sin(_t);
        

        if (_sinT < _lastFrameSinT && _otherT<10f)
        {
            Image.color = new Color(1, 1, 1, 1);

            _otherT += Time.deltaTime;
        }
        else {


            Image.color = new Color(1, 1, 1, _sinT);
            _t += Time.deltaTime;
        }

        if (_t > 4f) {  _started = false; Image.sprite = null; Image.color = new Color(1, 1, 1, 0); Destroy(gameObject,20f); }
    }
    void LateUpdate()
    {
        if(_otherT<=0f)
        _lastFrameSinT = _sinT;
    }
}
