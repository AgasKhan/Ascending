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

    Sliders.Slid timeImage;

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

    public void BackgroundMusic()
    {
        //Debug.Log("Funciono Background Music");
        audioM.Play("BackgroundMusic");
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

    private void Awake()
    {
        instance = this;
        Controllers.eneable = false;
        
        var listPlayer = gameObject.FindWithTags("Player");
        if (listPlayer.Length > 0)
            player = listPlayer[0].GetComponent<Player_Character>();

        TimeController.Awake();

        fixedUpdate = new Timer(1/60f);

        Quests.ChargeQuests(BaseData.currentLevel);
    }


    private void Start()
    {
        currentTime -= Time.realtimeSinceStartup;
        audioM = GetComponent<AudioManager>();
        BackgroundMusic();
        timeImage = Sliders.SrchSlider("Time");

        TimersManager.Create(0.5f, 
            () => 
            {
                foreach (var item in Abilities.Abilitieslist)
                {
                    item.value.CheckOnStart();
                }

                DebugPrint.Log(Abilities.Abilitieslist.ToString());
            });

        
        MisionHUD.UpdateMisions();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        if(Quests.Update())
            MisionHUD.UpdateMisions();

        TextCanvas.SrchMessages("debug").ShowText(false, FPS());

        timeImage.CurrValue(1 - currentTime/maxLevelTimer);
        timeImage.color = Color.Lerp(Color.white, Color.red, currentTime / maxLevelTimer);


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

    static public IEnumerator DeActivateRetarded(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

   
}

