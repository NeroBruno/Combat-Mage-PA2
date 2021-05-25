using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        /*
         * The normal LoadScene method basically pauses the entire game in order to spend all of it's resources trying to Load a new Scene
         * LoadSceneAsync loads the scene asynchronously in the background, which is what we want for a bigger and open world game
         * It means that it keeps our current scene and all of the behaviours in it running, while it's loading our new scene into memory
         * What we can then do is get some information back from the SceneManager about the progress of this operation, to build the loading bar!
         */
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);
        
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100.0f + "%";

            yield return null; // this waits until the next frame before continuing
        }
    }

}
