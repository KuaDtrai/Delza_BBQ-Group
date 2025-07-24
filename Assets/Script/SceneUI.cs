using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUI : MonoBehaviour
{

    public void Return()
    {
        SceneManager.LoadSceneAsync("Start Menu");
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
            { Time.timeScale = 1; }
        else
        { Time.timeScale = 0; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
