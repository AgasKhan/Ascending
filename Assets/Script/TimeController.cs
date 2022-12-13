using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class TimeController
{
    static public List<TimeController> entitys;


    static MotionBlur motionBlur;
    static ChromaticAberration chromaticAberration;
    static ColorGrading colorGrading;
    static PostProcessVolume volume;

    static public void StartReverse()
    {
        Controllers.eneable = false;
        Time.timeScale = 0;
        foreach (var item in entitys)
        {
            item.StartReverseItem();
        }

        Utilitys.LerpInTime(volume.weight, 1f, 1, Mathf.Lerp, (weight)=> { volume.weight = weight; });
        MainHud.ChangeAlphaWithFade(0.1f,1, "Effect");
    }

    static public void FinishReverse()
    {
        Controllers.eneable = true;
        Time.timeScale = 1;
        foreach (var item in entitys)
        {
            item.FinishReverseItem();
        }

        Utilitys.LerpInTime(volume.weight, 0f, 1, Mathf.Lerp, (weight) => { volume.weight = weight; });
        MainHud.RestoreOriginalColorWithFade(1, "Effect");
        HealthUI_HealthCh.instance.RefreshHealth(GameManager.player.health.Percentage());
    }

    public static void Awake()
    {
        entitys = new List<TimeController>();

        motionBlur = ScriptableObject.CreateInstance<MotionBlur>();
        chromaticAberration = ScriptableObject.CreateInstance<ChromaticAberration>();
        colorGrading = ScriptableObject.CreateInstance<ColorGrading>();


        motionBlur.enabled.Override(true);
        motionBlur.shutterAngle.Override(270);
        motionBlur.sampleCount.Override(10);

        chromaticAberration.enabled.Override(true);
        chromaticAberration.fastMode.Override(false);
        chromaticAberration.intensity.Override(0.99f);

        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(-100);


        volume = PostProcessManager.instance.QuickVolume(12, 2, chromaticAberration, colorGrading, motionBlur);
        volume.weight = 0;

    }

    static public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            foreach (var item in entitys)
            {
                item.ReverseItem(2);
            }
        }
        else
        {
            foreach (var item in entitys)
            {
                item.UpdateItem();
            }
        }
    }

    static public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReverse();
        }

        else if (Input.GetKeyUp(KeyCode.R))
        {
            FinishReverse();
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (entitys.Count > 1)
            {
                GameManager.CurrentTime(-Time.unscaledDeltaTime);
            }
        }
        else
        {
            GameManager.CurrentTime(Time.unscaledDeltaTime);
           
        }

    }






    [System.Serializable]
    struct PosAndRot
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

    Interactuable_LogicActive l;

    /// <summary>
    /// auxiliar array bools monobehabior
    /// </summary>
    bool[] mono;

    Stack<PosAndRot> posAndRots = new Stack<PosAndRot>();

    Stack<Transform> parents = new Stack<Transform>();

    Stack<Vector3> velocitys = new Stack<Vector3>();

    Stack<Vector3> healths = new Stack<Vector3>();

    Stack<bool> logicActiveInteractuable = new Stack<bool>();

    Stack<int> patrolIndex = new Stack<int>();

    Stack<bool> colActive = new Stack<bool>();

    Stack<bool[]> monoActive = new Stack<bool[]>();

    Stack<bool> kinematic = new Stack<bool>();

    Stack<bool> gravity = new Stack<bool>();

    public void FinishReverseItem()
    {
        if (m.Length > 0)
        { 
            for (int ii = 0; ii < m.Length; ii++)
            {
                m[ii].enabled = mono[ii];
            }
        }

        //activo el animator
        if (a != null)
        {
            a.enabled = true;
            a.Play("standing idle");
        }
            


    }

    public void StartReverseItem()
    {
        for (int ii = 0; ii < m.Length; ii++)
        {
            m[ii].enabled = false;
        }

        //desactivo el animator
        if (a != null)
            a.enabled = false;

        if (t.TryGetComponent(out Dagger_Proyectile dagger))
            dagger.CancelLerps();
    }

    public void ReverseItem(int jump = 1)
    {
        if (posAndRots.Count <= 1 || t == null)
            return;

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
            }

            if (rb != null)
            {
                rb.velocity = velocitys.Pop();
                rb.isKinematic = kinematic.Pop();
                rb.useGravity = gravity.Pop();
            }
                

            if (h != null)
                h.SetAll(healths.Pop());

            if (p != null)
                p.iPatrulla = patrolIndex.Pop();

            if (e != null)
                e.deleySearch.Substract(100);

            if (c != null)
                c.enabled = colActive.Pop();

            if (l != null)
                l.diseable = logicActiveInteractuable.Pop();
            
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
        {
            velocitys.Push(rb.velocity);
            kinematic.Push(rb.isKinematic);
            gravity.Push(rb.useGravity);
        }
            

        if (h != null)
            healths.Push(h.GetAll());

        if (p != null)
            patrolIndex.Push(p.iPatrulla);

        if (c != null)
            colActive.Push(c.enabled);

        if (l != null)
            logicActiveInteractuable.Push(l.diseable);
            
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

    public static List<TimeController> ChildTranform(Transform c, bool monoActive=true)
    {
        List<TimeController> d = new List<TimeController>();

        if (c.childCount > 0)
        {
            for (int i = 0; i < c.childCount; i++)
            {
                d.Add(new TimeController(c.GetChild(i), monoActive));
                if (c.GetChild(i).childCount > 0)
                    d.AddRange(ChildTranform(c.GetChild(i),monoActive));
            }
        }

        return d;

    }



    public TimeController(Transform t, bool monoActive=true)
    {
        this.t = t;
        rb = t.GetComponent<Rigidbody>();
        a = t.GetComponentInChildren<Animator>();
        h = t.GetComponent<Health>();
        e = t.GetComponent<Enemy_Character>();
        c = t.GetComponent<Collider>();
        l = t.GetComponent<Interactuable_LogicActive>();
            
        if(t.TryGetComponent(out IPatrolReturn aux))
            p = aux.PatrolReturn();

        if (monoActive)
            m = t.GetComponents<MonoBehaviour>();
        else
            m = new MonoBehaviour[0];
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