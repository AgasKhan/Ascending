using System;
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

    static public void StartAllItem()
    {
        foreach (var item in entitys)
        {
            item.StartItem();
        }
    }

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
        foreach (var item in entitys)
        {
            item.FinishReverseItem();
        }

        Controllers.eneable = true;
        Time.timeScale = 1;
        Utilitys.LerpInTime(volume.weight, 0f, 1, Mathf.Lerp, (weight) => { volume.weight = weight; });
        MainHud.RestoreOriginalColorWithFade(1, "Effect");
        HealthUI_HealthCh.instance.RefreshHealth(GameManager.player.health.Percentage());

        GameManager.player.fsmAiming.CurrentState = GameManager.player.fsmAiming.noAiming;
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

    public Transform t;

    List<MementoBase> mementos = new List<MementoBase>();

    public int count => ((Tr)mementos[0]).Count;

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
        if (count <= 1 || t == null)
            return;

        for (int i = 0; i < jump && count >= 1; i++)
        {      
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

        foreach (var item in mementos)
        {
            item.OnUpdate();
        }            
    }

    public void StartItem()
    {
        if (t == null)
            return;

        foreach (var item in mementos)
        {
            item.OnInit();
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

        #region
        public abstract void OnInit();
        public abstract void OnUpdate();
        #endregion
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
        StackWithDefault<Vector3> velocitys = new StackWithDefault<Vector3>();
        StackWithDefault<bool> kinematic = new StackWithDefault<bool>();
        StackWithDefault<bool> gravity = new StackWithDefault<bool>();

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
        public override void OnInit()
        {
            velocitys.SetDefault(reference.velocity);
            kinematic.SetDefault(reference.isKinematic);
            gravity.SetDefault(reference.useGravity);
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

        public override void OnInit()
        {
        }
    }

    public class Coll : Memento<Collider>
    {
        StackWithDefault<bool> colActive = new StackWithDefault<bool>();

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

        public override void OnInit()
        {
            colActive.SetDefault(reference.enabled);
        }
    }

    public class Heal : Memento<Health>
    {
        StackWithDefault<Vector3> healths = new StackWithDefault<Vector3>();

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
        public override void OnInit()
        {
            healths.SetDefault(reference.GetAll());
        }
    }

    public class Pat : Memento<Patrol>
    {
        StackWithDefault<int> patrolIndex = new StackWithDefault<int>();

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

        public override void OnInit()
        {
            patrolIndex.SetDefault(reference.iPatrulla);
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

        public int Count => posAndRot.Count;

        StackWithDefault<PosAndRot> posAndRot = new StackWithDefault<PosAndRot>();

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
        public override void OnInit()
        {
            posAndRot.SetDefault(new PosAndRot(reference));
        }
    }

    public class Mono : Memento<MonoBehaviour>
    {
        /// <summary>
        /// auxiliar array bools monobehabior
        /// </summary>
        bool mono;

        StackWithDefault<bool> monoActive = new StackWithDefault<bool>();

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

        public override void OnInit()
        {
            monoActive.SetDefault(reference.enabled);
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

        public override void OnInit()
        {
        }
    }

    public class Log : Memento<Interactuable_LogicActive>
    {
        StackWithDefault<bool> logicActiveInteractuable = new StackWithDefault<bool>();

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

        public override void OnInit()
        {
            logicActiveInteractuable.SetDefault(reference.diseable);
        }
    }

    public class Dag : Memento<Dagger_Proyectile>
    {
        StackWithDefault<IState<FSMDagger>> states = new StackWithDefault<IState<FSMDagger>>();

        IState<FSMDagger> currentReverse;

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
            reference.CurrentState=currentReverse;
        }

        public override void OnStayState()
        {
            currentReverse = states.Pop();
        }

        public override void OnUpdate()
        {
            states.Push(reference.CurrentState);
        }

        public override void OnInit()
        {
            states.SetDefault(reference.CurrentState);
        }
    }
}

/// <summary>
/// Caso especifico donde jamas quiero eliminar el ultimo elemento de la lista del stack
/// </summary>
/// <typeparam name="T"></typeparam>
public class StackWithDefault<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
{
    /// <summary>
    /// valor que devuelve por defecto cuando se queda sin elementos
    /// </summary>
    T def;

    Stack<T> stack = new Stack<T>();

    public int Count => stack.Count+1;

    public bool IsSynchronized => ((ICollection)stack).IsSynchronized;

    public object SyncRoot => ((ICollection)stack).SyncRoot;

    public void Clear() => stack.Clear();
    public bool Contains(T item) => stack.Contains(item);

    public IEnumerator<T> GetEnumerator() => stack.GetEnumerator();
    public T Peek() => stack.Count > 0 ? stack.Peek() : def;
    public T Pop() => stack.Count > 0 ? stack.Pop() : def;
    public void Push(T item) => stack.Push(item);
    public T[] ToArray() => stack.ToArray();
    public void TrimExcess() => stack.TrimExcess();

    public void CopyTo(T[] array, int arrayIndex) => stack.CopyTo(array, arrayIndex);

    public void CopyTo(Array array, int index)
    {
        ((ICollection)stack).CopyTo(array, index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)stack).GetEnumerator();
    }

    public void SetDefault(T def)
    {
        this.def = def;
    }
}