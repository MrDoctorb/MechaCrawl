using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] Text version;
    private void Start()
    {
        if(version != null)
        {
            version.text = "Version " + Application.version;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Feedback()
    {
        Application.OpenURL("https://forms.gle/XkBkofQynVm3UXo3A");
    }
}
