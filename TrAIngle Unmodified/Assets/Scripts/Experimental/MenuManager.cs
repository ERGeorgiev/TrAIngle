using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EdsLibrary.Audio;

public class MenuManager : MonoBehaviour {
    public static MenuManager instance = null;
    [SerializeField]
    Object sceneToLoad;
    public string highlightSoundName;
    public string pressSoundName;
    private Sound highlightSound;
    private Sound pressSound;

    void Awake()
    {
        if (!AssignInstance())
            return;
    }

    void Start()
    {
        highlightSound = AudioManager.FindSound(highlightSoundName);
        pressSound = AudioManager.FindSound(pressSoundName);
    }

    bool AssignInstance()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
                return false;
            }
            return true;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            return true;
        }
    }
    // Use this for initialization
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad.name);
	}
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnHighlight()
    {
        highlightSound.Play();
    }

    public void OnPress()
    {
        pressSound.Play();
    }
}
