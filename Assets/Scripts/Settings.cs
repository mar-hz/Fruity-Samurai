using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class Settings : MonoBehaviour
{
    public Transform canvasTransform;
    public GameObject settingsMenu;
    private GameObject currentSettingsMenu;
    public Slider music,sfx;
    public AudioMixer audioMixer;
    int[] blacklist ={0,2}; // Array of scenes to blacklist
    public GameObject goBackButton;
    AudioSource sfxSource;
    public AudioClip clickSound;

    void Awake() {
        sfxSource = GameObject.Find("SFX_Samp")?.GetComponent<AudioSource>();
        if (sfxSource == null)
            Debug.LogError("AudioSource not found on SFX_Samp in Awake!");
    }

    public void OpenSettings() {
        Debug.Log("Settings menu opened.");
        if (currentSettingsMenu == null) {
            currentSettingsMenu = Instantiate(settingsMenu, canvasTransform);
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (System.Array.Exists(blacklist, element => element == currentSceneIndex)) {
            Debug.Log("Hiding GoBackButton.");
            goBackButton = currentSettingsMenu.transform.Find("GoBackButton").gameObject;
            if (goBackButton != null) {
                goBackButton.SetActive(false);
            } else {
                Debug.LogWarning("GoBackButton not found in settings menu!");
            }
            }
            // sfxSource = GameObject.Find("SFX_Samp").GetComponent<AudioSource>();
            // if (sfxSource == null)
            //     Debug.LogError("SFX_Samp not found in scene!");
        }
        currentSettingsMenu.SetActive(true); 
    }

    public void CloseSettings() {
        sfxSource.PlayOneShot(clickSound);
        gameObject.SetActive(false);
        Debug.Log("Settings menu closed.");
    }

    public void ChangeMusicVolume() {
        audioMixer.SetFloat("Music", music.value);
        Debug.Log("Music volume set to: " + music.value);
    }
     public void ChangeSFXVolume() {
        audioMixer.SetFloat("SFX", sfx.value);
        Debug.Log("Music volume set to: " + sfx.value);
    }

    public void ReturnMainMenu() {
        StartCoroutine(PlaySoundThenLoad(0));
        // SceneManager.LoadScene(0);
    }

    IEnumerator PlaySoundThenLoad(int sceneToLoad)
    {
        sfxSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(clickSound.length);
        SceneManager.LoadScene(sceneToLoad);
    }
}
