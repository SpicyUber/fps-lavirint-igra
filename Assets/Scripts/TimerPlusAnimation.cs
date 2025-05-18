using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class TimerPlusAnimation : MonoBehaviour
{
    private bool _isPlaying;
    public Image Image;
    public Sprite[] Frames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Image.color = new Color(1,1,1,0);
    }
    public void Play(bool red) { if (_isPlaying) return; _isPlaying = true; StartCoroutine(PlayAnimation(0.066f,red)); }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayAnimation(float timeBetweenFrames, bool red)
    {
        Image.sprite = Frames[0];
        float num = 1 / (Frames.Length-1);
        if (red) { Image.color = Color.red; }
        else
        Image.color = new Color(1, 1, 1, 1);
        if (red) foreach (Sprite sprite in Frames)
            {
                Color color = Color.red;
                Image.sprite = sprite;
                if (sprite != Frames[0])
                {
                   
                    color.a = 1 - num;
                     
                    num += num;
                }
                Image.color = color;
                yield return new WaitForSeconds(timeBetweenFrames);
            }
        else
        foreach (Sprite sprite in Frames)
        {
            Image.sprite = sprite;
            if (sprite != Frames[0]) { 
            Image.color = new Color(1, 1, 1, 1-num);
            num += num;
            }
            yield return new WaitForSeconds(timeBetweenFrames);
        }
        Image.color = new Color(1, 1, 1, 0);
        _isPlaying = false;
    }
}
