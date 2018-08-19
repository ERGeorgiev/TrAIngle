using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    public GameObject mainMenu;
    public List<GameObject> howItWorks;
    public GameObject info;

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}
