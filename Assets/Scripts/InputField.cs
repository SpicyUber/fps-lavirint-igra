using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    //public string Password;
    //  public TextMeshProUGUI text;
    private int sfxNum = 0;
    private bool sfxFlag = false;
    public Slider SensitivitySlider, VolumeSlider;
    public AudioClip VolumeClip;
    public GameObject[] Panels;
    private float cooldown;
   /* public void Compare()
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

    }*/

    public void StartGame()
    {
        UpdateSliders();
         
       if(SceneManager.GetActiveScene().buildIndex==0) SceneManager.LoadSceneAsync("MainScene"); else { SceneManager.LoadSceneAsync(0); Time.timeScale = 1; }
        ActivateLoad();
    }

    public void UpdateSliders()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", SensitivitySlider.value);
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

    

    private void PanelActivate(int index)
    {
        foreach (GameObject pan in Panels)
        {
            pan.SetActive(false);
        }
        if (index > -1) { Panels[index].SetActive(true); GetComponent<AudioSource>().Play(); }
        
    }


    public void PlayVolumeClip() { if (sfxNum == 0) { sfxNum++;return; } sfxFlag = true; }

    public void ActivateControls() { PanelActivate(1);  }

    public void ActivateLoad() { PanelActivate(0); }
    public void ActivateTips() { PanelActivate(2); }
    public void ActivateSettings() { PanelActivate(3); }

    public void ActivateExit()
    {
        Application.Quit();
    }
    public void Start()
    {
        PanelActivate(-1);
        Cursor.visible = true;
        PlayerPrefs.SetInt("Hint",0);
        PlayerPrefs.SetInt("Death", 0);
        if (PlayerPrefs.GetString("unity.player_session_count") == "1")
        {
            PlayerPrefs.SetFloat("MouseSensitivity", 0.5f);
            
        }
       
        if (PlayerPrefs.GetFloat("Volume") == 0 && SceneManager.GetActiveScene().buildIndex==0) { PlayerPrefs.SetFloat("Volume", 0.33f); }
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        SensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
       if(SceneManager.GetActiveScene().buildIndex == 0) PlayerPrefs.SetInt("Checkpoint", 0);
    }

    private void Update()
    {
        if (cooldown > 0.5f && sfxFlag) { sfxFlag = false; cooldown = 0f; GetComponent<AudioSource>().PlayOneShot(VolumeClip);  }
        if (cooldown <= 0.5f) { cooldown += Time.deltaTime; } 
       
    }
}
