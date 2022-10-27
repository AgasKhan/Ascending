using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadSc()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void Load(string scn)
    {
        StartCoroutine(LoadSc(scn));
        Time.timeScale = 1;
    }



    IEnumerator LoadSc(string sceneName)
    {
        //loadscene = true;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            Debug.Log("cargando: " + async.progress);
            yield return null;
        }
        //Debug.Log("Carga finalizada");
    }

}
