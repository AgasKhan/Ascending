using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeController
{
    static public List<TimeController> entitys;

    static TimeController instance;

    static float maxLevelTimer;

    static float currentTime;

    static int multiplyReverseCamera;

    static public void Update()
    {
        if (!GameManager.saveTime)
            return;

        if (maxLevelTimer < currentTime)
            GameManager.instance.StartCoroutine(GameManager.ReverseAll(multiplyReverseCamera));

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
                GameManager.CurrentTime(-Time.unscaledDeltaTime);
            }

            foreach (var item in entitys)
            {
                item.ReverseItem();
            }
        }
        else
        {
            GameManager.CurrentTime(Time.unscaledDeltaTime);
            foreach (var item in entitys)
            {
                item.UpdateItem();
            }
        }
    }


    [System.Serializable]
    public struct PosAndRot
    {
        public Vector3 pos;
        public Vector3 scale;
        public Quaternion rot;
        public bool active;

        public PosAndRot(PosAndRot par)
        {
            pos = par.pos;
            rot = par.rot;
            active = par.active;
            scale = par.scale;
        }

        public PosAndRot(Vector3 p, Quaternion r, Vector3 sc,  bool a)
        {
            pos = p;
            rot = r;
            active = a;
            scale = sc;
        }

        public PosAndRot(Transform t)
        {
            pos = t.position;
            rot = t.rotation;
            scale = t.localScale;
            active = t.gameObject.activeSelf;
        }
    }

    public int count=0;

    public Transform t;

    public Animator a;

    MonoBehaviour[] m;

    Collider c;

    Patrol p;

    Rigidbody rb;    

    Health h;

    Enemy_Character e;

    /// <summary>
    /// auxiliar array bools monobehabior
    /// </summary>
    bool[] mono;

    Stack<PosAndRot> posAndRots = new Stack<PosAndRot>();

    Stack<Transform> parents = new Stack<Transform>();

    Stack<Vector3> velocitys = new Stack<Vector3>();

    Stack<Vector3> healths = new Stack<Vector3>();

    Stack<int> patrolIndex = new Stack<int>();

    Stack<bool> colActive = new Stack<bool>();

    Stack<bool[]> monoActive = new Stack<bool[]>();

    public void ReverseItem(int jump = 1)
    {
        if (posAndRots.Count <= 1 || t == null)
            return;

        //desactivo el animator
        if (a != null)
            a.enabled = false;

        PosAndRot aux;
        //List<PosAndRot> par = new List<PosAndRot>();

        for (int i = 0; i < jump && count > 0; i++)
        {
            //copio todos los datos
            aux = new PosAndRot(posAndRots.Pop());

            count--;

            t.position = aux.pos;
            t.rotation = aux.rot;
            t.localScale = aux.scale;
            t.gameObject.SetActive(aux.active);
            t.parent = parents.Pop();

            if(m.Length>0)
            {
                mono = monoActive.Pop();

                for (int ii = 0; ii < m.Length; ii++)
                {
                    m[ii].enabled = mono[ii];
                }
            }

            if (rb != null)
                rb.velocity = velocitys.Pop();

            if (h != null)
                h.SetAll(healths.Pop());

            if (p != null)
                p.iPatrulla = patrolIndex.Pop();

            if (e != null)
                e.deleySearch.Substract(100);

            if (c != null)
                c.enabled = colActive.Pop();

            //par = childs.Pop();
        }
        /*
        foreach (var item in par)
        {
            if (item.transform != null)
            {
                item.transform.position = item.pos;
                item.transform.rotation = item.rot;
            }
        }*/
    }

    public void UpdateItem()
    {
        if (t == null)
            return;

        count++;

        if (rb != null)
            velocitys.Push(rb.velocity);

        //activo el animator
        if (a != null)
            a.enabled = true;

        if (h != null)
            healths.Push(h.GetAll());

        if (p != null)
            patrolIndex.Push(p.iPatrulla);

        if (c != null)
            colActive.Push(c.enabled);
            
        if (m.Length>0)
        {
            mono = new bool[m.Length];
            for (int i = 0; i < m.Length; i++)
            {
                mono[i] = m[i].enabled;
            }

            monoActive.Push(mono);
        }

        posAndRots.Push(new PosAndRot(t.position, t.rotation, t.localScale, t.gameObject.activeSelf));

        parents.Push(t.parent);

        //childs.Push(ChildTranform(t));

    }

    public static List<TimeController> ChildTranform(Transform c)
    {
        List<TimeController> d = new List<TimeController>();

        if (c.childCount > 0)
        {
            for (int i = 0; i < c.childCount; i++)
            {
                d.Add(new TimeController(c.GetChild(i)));
                if (c.GetChild(i).childCount > 0)
                    d.AddRange(ChildTranform(c.GetChild(i)));
            }
        }

        return d;

    }

    public static void Awake()
    {
        entitys = new List<TimeController>();
    }

    public TimeController(Transform t)
    {
        this.t = t;
        rb = t.GetComponent<Rigidbody>();
        a = t.GetComponentInChildren<Animator>();
        h = t.GetComponent<Health>();
        e = t.GetComponent<Enemy_Character>();
        c = t.GetComponent<Collider>();
        m = t.GetComponents<MonoBehaviour>();
            
        var aux = t.GetComponent<IPatrolReturn>();
        if(aux!=null)
            p = aux.PatrolReturn();
    }
}



/*
List<PosAndRot> ChildTranform(Transform c)
{
    List<PosAndRot> d = new List<PosAndRot>();

    if (c.childCount > 0)
    {
        for (int i = 0; i < c.childCount; i++)
        {
            d.Add(new PosAndRot(c.GetChild(i)));
            if (c.GetChild(i).childCount > 0)
                d.AddRange(ChildTranform(c.GetChild(i)));
        }
    }

    return d;

}
*/