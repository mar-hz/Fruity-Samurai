using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Iniciar()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMenu() {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
