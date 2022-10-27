using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<Character> enemys;

    public static Player_Character player;

    public List<TimeController> entitys = new List<TimeController>();

    static GameManager instance;

    public float delayStart;

    public int multiplyReverseCamera=1;

    public float maxLevelTimer;

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

    public bool saveTimeP= false;

    public float currentTime;

    public static float CurrentTime(float num = 0)
    {
        instance.currentTime += num;

        return instance.currentTime;
    }

    public static void AddTimeController(Transform t)
    {
        foreach (var item in instance.entitys)
        {
            if (item.t == t)
                return;
        }
        instance.entitys.Add(new TimeController(t));
        instance.entitys.AddRange(TimeController.ChildTranform(t));
        
    }

    public static void AddEnemy(Character enemy)
    {
        if (enemys == null)
            enemys = new List<Character>();
        enemys.Add(enemy);
    }

    private void Update()
    {
        if (!saveTime)
            return;
         
        if (maxLevelTimer < currentTime)
            StartCoroutine(ReverseAll(multiplyReverseCamera));

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

        if (Input.GetKey(KeyCode.R))
        {
            if (entitys[0].count > 1)
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

    private void Awake()
    {
        instance = this;
        Controllers.eneable = false;
    }


    private void Start()
    {
        currentTime -= Time.realtimeSinceStartup;
    }

    public static IEnumerator ReverseAll(int velocity = 1)
    {
        saveTime = false;

        Controllers.eneable = false;
        Time.timeScale = 0;

        while (instance.entitys[0].count > velocity)
        {
            foreach (var item in instance.entitys)
            {
                item.Reverse(velocity);
            }
            yield return null;
        }

        foreach (var item in instance.entitys)
        {
            if (item.a != null)
                item.a.enabled = true;
        }

        Controllers.eneable = true;
        Time.timeScale = 1;
        instance.currentTime = 0;
        saveTime = true;
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStart);
        Controllers.eneable = true;
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