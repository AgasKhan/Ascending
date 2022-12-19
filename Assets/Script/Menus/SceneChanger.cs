using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public Animator transition;
    
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

        //Controllers.MouseLock();
        Cursor.lockState = CursorLockMode.Locked;

        var index = scn.IndexOf('_');
        if(index > -1)
        {
            var numberLevel = System.Convert.ToInt32(scn.Substring(index+1));
            CSVReader.SaveInPictionary<int>("CurrentLevel", numberLevel);
            BaseData.currentLevel = numberLevel;
        }
    }



    IEnumerator LoadSc(string sceneName)
    {
        //loadscene = true;

        transition.SetTrigger("Start");

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            Debug.Log("cargando: " + async.progress);
            yield return null;
        }
        //Debug.Log("Carga finalizada");
    }

}
