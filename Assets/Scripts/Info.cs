using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class Info : MonoBehaviour
{
    public Transform canvasTransform;
    public GameObject settingsMenu;
    private GameObject currentSettingsMenu;
    AudioSource sfxSource;
    public AudioClip clickSound;

    void Awake() {
        sfxSource = GameObject.Find("SFX_Samp")?.GetComponent<AudioSource>();
        if (sfxSource == null)
            Debug.LogError("AudioSource not found on SFX_Samp in Awake!");
    }

    public void OpenInfo() {
        Debug.Log("Info menu opened.");
        if (currentSettingsMenu == null) {
            currentSettingsMenu = Instantiate(settingsMenu, canvasTransform);
            // sfxSource = GameObject.Find("SFX_Samp").GetComponent<AudioSource>();
            // if (sfxSource == null)
            //     Debug.LogError("SFX_Samp not found in scene!");
        }
        currentSettingsMenu.SetActive(true); 
    }

    public void CloseInfo() {
        sfxSource.PlayOneShot(clickSound);
        gameObject.SetActive(false);
        Debug.Log("Info menu closed.");
    }
}
