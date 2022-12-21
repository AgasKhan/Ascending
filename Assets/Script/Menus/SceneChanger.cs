using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public Animator transition;

    bool isCharging = false;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadSc()
    {
        StartCoroutine(LoadSc(SceneManager.GetActiveScene().name));
    }

    public void Load(string scn)
    {
        if (isCharging)
            return;

        //Controllers.MouseLock();
        Cursor.lockState = CursorLockMode.Locked;

        int index = scn.IndexOf('_');
        if(index > -1)
        {
            int numberLevel = System.Convert.ToInt32(scn.Substring(index+1));
            CSVReader.SaveInPictionary<int>("CurrentLevel", numberLevel);
            BaseData.currentLevel = numberLevel;

            foreach (var item in Quests.SrchIncomplete(numberLevel))
            {
                item.active = true;
            } 
        }

        print("mandaste a cargar");

        isCharging = true;
        StartCoroutine(LoadSc(scn));
    }



    IEnumerator LoadSc(string sceneName)
    {
        //loadscene = true;
        Time.timeScale = 1;

        //var myNameScene = SceneManager.GetActiveScene().name;

        //transition.SetTrigger("Start");
        
        yield return null;

        Time.timeScale = 1;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        
        while (!async.isDone)
        {
            Debug.Log("Load: " + async.progress);

            yield return null;
        }

        /*

        yield return null;

        async = SceneManager.UnloadSceneAsync(myNameScene);

        while (!async.isDone)
        {
            Debug.Log("Unload: " + async.progress);

            yield return null;
        }
        */
        //Debug.Log("Carga finalizada");
    }

}
