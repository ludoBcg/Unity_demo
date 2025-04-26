using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            SceneManager.LoadScene(0);
            Debug.Log("Quit Scene");
        }
    }
}
