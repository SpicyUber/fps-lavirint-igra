using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputField : MonoBehaviour
{
    public string Password;
    public TextMeshProUGUI text;
    public void Compare()
    {
        string s1 = text.text.ToUpper();
        string s2 = Password.ToUpper();
        Debug.Log(s1+s2+ s1.Equals(s2));
        if (s1.Equals(s2))
        {
            SceneManager.LoadScene("MainScene");

        }
        else
        {
            GetComponent<AudioSource>().Play();
            text.text = "";
        }

    }
}
