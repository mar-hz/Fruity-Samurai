using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Settings : MonoBehaviour
{
    public Transform canvasTransform;
    public GameObject settingsMenu;
    private GameObject currentSettingsMenu;
    public Slider music,sfx;
    // public AudioMixer audioMixer;
    public int[] blacklist ={0}; // Array of scenes to blacklist
    public GameObject goBackButton;

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
        }
        currentSettingsMenu.SetActive(true); 
    }

    public void CloseSettings() {
       gameObject.SetActive(false);
       Debug.Log("Settings menu closed.");
    }

    // public void ChangeMusicVolume() {
    //     audioMixer.SetFloat("Music", music.value);
    //     Debug.Log("Music volume set to: " + music.value);
    // }
    //  public void ChangeSFXVolume() {
    //     audioMixer.SetFloat("SFX", sfx.value);
    //     Debug.Log("Music volume set to: " + sfx.value);
    // }

    public void ReturnMainMenu() {
        SceneManager.LoadScene(0);
    }


}
