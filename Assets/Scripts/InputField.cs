using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    public string Password;
    public TextMeshProUGUI text;
  
    public Slider SensitivitySlider;
    public void Compare()
    {
        string s1 = text.text.ToUpper();
        string s2 = Password.ToUpper();
        Debug.Log(s1+s2+ s1.Equals(s2));
        if (s1.Equals(s2))
        {

            PlayerPrefs.SetFloat("MouseSensitivity", SensitivitySlider.value);
            SceneManager.LoadScene("MainScene");

        }
        else
        {
            GetComponent<AudioSource>().Play();
            text.text = "";
        }

    }

    public void Start()
    {
        Cursor.visible = true;
        PlayerPrefs.SetInt("Hint",0);
        PlayerPrefs.SetInt("Death", 0);
        if (PlayerPrefs.GetString("unity.player_session_count") == "1")
        {
            PlayerPrefs.SetFloat("MouseSensitivity", 0.5f);
        }
        SensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        PlayerPrefs.SetInt("Checkpoint", 0);
    }
}
