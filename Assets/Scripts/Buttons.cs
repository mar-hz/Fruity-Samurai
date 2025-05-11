using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Buttons : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip clickSound;
    public void clickSFX () {
        sfxSource.PlayOneShot(clickSound);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Iniciar()
    {
        StartCoroutine(PlaySoundThenLoad(1));
        // SceneManager.LoadScene(1);
    }

    public void ToMenu() {
        StartCoroutine(PlaySoundThenLoad(0));
        // SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    public void Salir()
    {
        StartCoroutine(PlaySoundThenQuit());
        // #if UNITY_EDITOR
        //     UnityEditor.EditorApplication.isPlaying = false;
        // #else
        //     Application.Quit();
        // #endif
    }

    IEnumerator PlaySoundThenLoad(int sceneToLoad)
    {
        sfxSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(clickSound.length);
        SceneManager.LoadScene(sceneToLoad);
    }
    IEnumerator PlaySoundThenQuit()
    {
        sfxSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(clickSound.length);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
