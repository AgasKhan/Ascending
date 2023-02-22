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

        Utilitys.LerpInTime(volume.weight, 1f, 1, Mathf.Lerp, (weight) => { volume.weight = weight; });
        MainHud.ChangeAlphaWithFade(0.1f, 1, "Effect");
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

        GameManager.player.fsmAiming.SwitchState(GameManager.player.fsmAiming.noAiming);
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

    public int count = 0;

    public Transform t;

    List<MementoBase> mementos = new List<MementoBase>();

    public void FinishReverseItem()
    {
        foreach (var item in mementos)
        {
            item.OnExitState();
        }
    }

    public void StartReverseItem()
    {
        foreach (var item in mementos)
        {
            item.OnEnterState();
        }
    }

    public void ReverseItem(int jump = 1)
    {
        if (((Tr)mementos[0]).count <= 1 || t == null)
            return;

        for (int i = 0; i < jump && count > 0; i++)
        {           
            count--;

            foreach (var item in mementos)
            {
                item.OnStayState();
            }
        }
    }

    public void UpdateItem()
    {
        if (t == null)
            return;

        count++;

        foreach (var item in mementos)
        {
            item.OnUpdate();
        }            
    }

    public static List<TimeController> ChildTranform(Transform c, bool monoActive = true)
    {
        List<TimeController> d = new List<TimeController>();

        if (c.childCount > 0)
        {
            for (int i = 0; i < c.childCount; i++)
            {
                d.Add(new TimeController(c.GetChild(i), monoActive));
                if (c.GetChild(i).childCount > 0)
                    d.AddRange(ChildTranform(c.GetChild(i), monoActive));
            }
        }

        return d;

    }

    public TimeController(Transform t, bool monoActive=true)
    {
        var tr = new Tr();

        tr.Init(t);

        mementos.Add(tr);

        if (MementoBase.SaveComponent(t.GetComponent<Rigidbody>(), out RB component))
            mementos.Add(component);

        if (MementoBase.SaveComponent(t.GetComponent<Animator>(), out Anim anim))
            mementos.Add(anim);

        if (MementoBase.SaveComponent(t.GetComponent<Collider>(), out Coll col))
            mementos.Add(col);

        if (MementoBase.SaveComponent(t.GetComponent<Health>(), out Heal hel))
            mementos.Add(hel);

        if (MementoBase.SaveComponent(t.GetComponent<Interactuable_LogicActive>(), out Log log))
            mementos.Add(log);

        if (MementoBase.SaveComponent(t.GetComponent<Enemy_Character>(), out Ene ene))
            mementos.Add(ene);


        if (t.TryGetComponent(out Dagger_Proyectile dag))
        {
            Dag aux = new Dag();

            dag.Init();

            aux.Init(dag);

            mementos.Add(aux);
        }


        if (t.TryGetComponent(out IPatrolReturn patrol))
        {
            Pat aux = new Pat();

            aux.Init(patrol.PatrolReturn());

            mementos.Add(aux);
        }


        if (monoActive)
            foreach (var item in t.GetComponents<MonoBehaviour>())
            {
                Mono mono = new Mono();
                mono.Init(item);
                mementos.Add(mono);
            }

        this.t = t;
    }

    public abstract class MementoBase : IState
    {
        public static bool SaveComponent<T, R>(T t, out R component) where R : Memento<T>, new() where T : Component
        {
            if (t == null)
            {
                component = null;
                return false;
            }

            component = new R();

            component.Init(t);

            return true;
        }

        #region reverse
        public abstract void OnEnterState();
        public abstract void OnStayState();
        public abstract void OnExitState();
        #endregion
        public abstract void OnUpdate();
    }

    public abstract class Memento<R> : MementoBase
    {
        protected R reference;

        public void Init(R reference)
        {
            this.reference = reference;
        }
    }

    public class RB : Memento<Rigidbody>
    {
        Stack<Vector3> velocitys = new Stack<Vector3>();
        Stack<bool> kinematic = new Stack<bool>();
        Stack<bool> gravity = new Stack<bool>();

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            reference.velocity = velocitys.Pop();
            reference.isKinematic = kinematic.Pop();
            reference.useGravity = gravity.Pop();
        }

        public override void OnUpdate()
        {
            velocitys.Push(reference.velocity);
            kinematic.Push(reference.isKinematic);
            gravity.Push(reference.useGravity);
        }
    }

    public class Anim : Memento<Animator>
    {
        public override void OnEnterState()
        {
            reference.enabled = false;
        }

        public override void OnExitState()
        {
            reference.enabled = true;
            reference.Play("standing idle");
        }

        public override void OnStayState()
        {
        }

        public override void OnUpdate()
        {
        }
    }

    public class Coll : Memento<Collider>
    {
        Stack<bool> colActive = new Stack<bool>();

        public override void OnEnterState()
        {

        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            reference.enabled = colActive.Pop();
        }

        public override void OnUpdate()
        {
            colActive.Push(reference.enabled);
        }
    }

    public class Heal : Memento<Health>
    {
        Stack<Vector3> healths = new Stack<Vector3>();

        public override void OnEnterState()
        {

        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            reference.SetAll(healths.Pop());
        }

        public override void OnUpdate()
        {
            healths.Push(reference.GetAll());
        }
    }

    public class Pat : Memento<Patrol>
    {
        Stack<int> patrolIndex = new Stack<int>();

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            reference.iPatrulla = patrolIndex.Pop();
        }

        public override void OnUpdate()
        {
            patrolIndex.Push(reference.iPatrulla);
        }
    }

    public class Tr : Memento<Transform>
    {
        [System.Serializable]
        struct PosAndRot
        {
            public Vector3 pos;
            public Vector3 scale;
            public Quaternion rot;
            public bool active;
            public Transform parent;

            public PosAndRot(PosAndRot par)
            {
                pos = par.pos;
                rot = par.rot;
                active = par.active;
                scale = par.scale;
                parent = par.parent;
            }

            public PosAndRot(Transform t)
            {
                pos = t.position;
                rot = t.rotation;
                scale = t.localScale;
                active = t.gameObject.activeSelf;
                parent = t.parent;
            }
        }

        public int count => posAndRot.Count;

        Stack<PosAndRot> posAndRot = new Stack<PosAndRot>();

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            PosAndRot aux = new PosAndRot(posAndRot.Pop());

            reference.position = aux.pos;
            reference.rotation = aux.rot;
            reference.localScale = aux.scale;
            reference.gameObject.SetActive(aux.active);
            reference.parent = aux.parent;
        }

        public override void OnUpdate()
        {
            posAndRot.Push(new PosAndRot(reference));
        }
    }

    public class Mono : Memento<MonoBehaviour>
    {
        /// <summary>
        /// auxiliar array bools monobehabior
        /// </summary>
        bool mono;

        Stack<bool> monoActive = new Stack<bool>();

        public override void OnEnterState()
        {
            reference.enabled = false;
        }

        public override void OnExitState()
        {
            reference.enabled = mono;
        }

        public override void OnStayState()
        {
            mono = monoActive.Pop();
        }

        public override void OnUpdate()
        {
            monoActive.Push(reference.enabled);
        }
    }

    public class Ene : Memento<Enemy_Character>
    {
        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            reference.deleySearch.Substract(100);
        }

        public override void OnUpdate()
        {
        }
    }

    public class Log : Memento<Interactuable_LogicActive>
    {
        Stack<bool> logicActiveInteractuable = new Stack<bool>();

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void OnStayState()
        {
            reference.diseable = logicActiveInteractuable.Pop();
        }

        public override void OnUpdate()
        {
            logicActiveInteractuable.Push(reference.diseable);
        }
    }

    public class Dag : Memento<Dagger_Proyectile>
    {
        Stack<IState<FSMDagger>> states = new Stack<IState<FSMDagger>>();

        IState<FSMDagger> currentReverse;

        public override void OnEnterState()
        {
            
        }

        public override void OnExitState()
        {
            reference.SwitchState(currentReverse);
        }

        public override void OnStayState()
        {
            currentReverse = states.Pop();
        }

        public override void OnUpdate()
        {
            states.Push(reference.ReturnState());
        }
    }
}
