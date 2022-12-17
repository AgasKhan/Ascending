using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<Character> enemys;

    public static Player_Character player;

    static public GameManager instance;

    public float delayStart;

    public int multiplyReverseCamera=1;

    public float maxLevelTimer;

    public bool saveTimeP= false;

    public float currentTime;

    public AudioManager audioM;

    int minFps=900, maxFps=0;

    List<int> media = new List<int>();

    Timer fixedUpdate;

    public static bool saveTime
    {
        get
        {
            return instance.saveTimeP;
        }
        set
        {
            instance.saveTimeP = value;
        }
    }

    public static float CurrentTime(float num = 0)
    {
        instance.currentTime += num;

        return instance.currentTime;
    }

    public static void AddTimeController(Transform t, bool monoActive=true)
    {
        foreach (var item in TimeController.entitys)
        {
            if (item.t == t)
                return;
        }

        TimeController.entitys.Add(new TimeController(t, monoActive));
        TimeController.entitys.AddRange(TimeController.ChildTranform(t, monoActive));
    }

    public static void AddEnemy(Character enemy)
    {
        if (enemys == null)
            enemys = new List<Character>();
        enemys.Add(enemy);
    }

    public string FPS()
    {
        int fps = (int)(Mathf.Round(1 / Time.unscaledDeltaTime));
        int promedio;
        int suma = 0;

        if (minFps > fps && fps>5)
            minFps = fps;
        else if (maxFps < fps)
            maxFps = fps;

        if (media.Count > 180)
            media.RemoveAt(0);

        media.Add(fps);

        foreach (var item in media)
        {
            suma += item;
        }

        promedio = suma / media.Count;


        string aux =  fps + "\tFps\t\t" +
             "\n" +
            maxFps + "\tmaximo\t" +
            "\n" +
            minFps + "\tminimo\t" +
            "\n" +
            promedio + "\tmedia\t\t";

        return aux;
    }

    private void Update()
    {      
        TextCanvas.SrchMessages("debug").ShowText(false, FPS());

        if (!saveTime)
            return;

        TimeController.Update();

        fixedUpdate.Substract(Time.unscaledDeltaTime);
        if (fixedUpdate.Chck && saveTime)
        {
            TimeController.FixedUpdate();
            fixedUpdate.Reset();
        }

        if (maxLevelTimer < currentTime)
            ReverseAllCoroutine();

    }

    private void Awake()
    {
        instance = this;
        Controllers.eneable = false;

        var listPlayer = gameObject.FindWithTags("Player");
        if (listPlayer.Length > 0)
            player = listPlayer[0].GetComponent<Player_Character>();

        TimeController.Awake();

        fixedUpdate = new Timer(1/60f);

        
    }


    private void Start()
    {
        currentTime -= Time.realtimeSinceStartup;
        audioM = GetComponent<AudioManager>();
        BackgroundMusic();      

        TimersManager.Create(0.1f, 
            () => 
            {
                foreach (var item in Abilities.Abilitieslist)
                {
                    item.value.CheckOnStart();
                }

                DebugPrint.Log(Abilities.Abilitieslist.ToString());
            });
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public static void ReverseAllCoroutine()
    {
        instance.StartCoroutine(ReverseAll());
    }

    static IEnumerator ReverseAll()
    {
        saveTime = false;

        TimeController.StartReverse();

        while (TimeController.entitys[0].count > instance.multiplyReverseCamera)
        {
            CurrentTime(-Time.unscaledDeltaTime);
            foreach (var item in TimeController.entitys)
            {
                item.ReverseItem(instance.multiplyReverseCamera);
            }
            yield return null;
        }

        TimeController.FinishReverse();

        MainHud.ReticulaPlay("Start");

        

        instance.currentTime = 0;
        saveTime = true;
    }


    IEnumerator DestroyRetarded(GameObject destroy)
    {
        yield return null;
        Destroy(destroy);
    }

    static public IEnumerator DeActivateRetarded(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

    public void BackgroundMusic()
    {
        Debug.Log("Funciono Background Music");
        audioM.Play("BackgroundMusic");
    }
}


/*
public static void AddGlue(Transform t, Transform me)
{
    GameObject glue = PoolObjects.SpawnPoolObject(Vector2Int.zero, me.transform.position, t.rotation);

    me.transform.SetParent(null);
    glue.transform.SetParent(null);
    glue.transform.localScale = Vector3.one;
    //glue.transform.SetPositionAndRotation(me.transform.position, t.rotation);
    glue.transform.SetParent(t.transform);
    me.transform.SetParent(glue.transform);
}


    private void Update()
    {
        if (!saveTime)
            return;

        List<TimeController> removes = new List<TimeController>();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Controllers.eneable = false;
            Time.timeScale = 0;
        }

        else if (Input.GetKeyUp(KeyCode.R))
        {
            Controllers.eneable = true;
            Time.timeScale = 1;
        }


        foreach (var item in entitys)
        {
            if (item.t == null)
                removes.Add(item);
        }

        for (int i = 0; i < removes.Count; i++)
        {
            entitys.Remove(removes[i]);
        }


        if (maxLevelTimer < currentTime)
            StartCoroutine(ReverseAll(3));

        if (Input.GetKey(KeyCode.R))
        {
            if (entitys[0].posAndRots.Count > 1)
            {
                CurrentTime(-Time.unscaledDeltaTime);
            }

            foreach (var item in entitys)
            {
                item.Reverse();
            }
        }
        else
        {
            CurrentTime(Time.unscaledDeltaTime);
            foreach (var item in entitys)
            {
                item.Update();
            }
        }
    }
*/