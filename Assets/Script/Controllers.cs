using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controllers : MonoBehaviour
{
    #region static classes
    static List<Key> _keys = new List<Key>();

    static public Key attack = new Key(KeyCode.Mouse0);

    static public Key aim = new Key(KeyCode.Mouse1);

    static public Key active = new Key(KeyCode.E);

    static public Key power = new Key(KeyCode.Q);

    static public Key jump = new Key(KeyCode.Space);

    static public Key dash = new Key(KeyCode.LeftShift);

    static public Key flip = new Key(KeyCode.F);

    static public Key locked = new Key(KeyCode.Tab);

    static Controllers _instance;
    static public Vector2 dir
    {
        get
        {
            if (eneableMove)
                return _instance._dir;
            else
                return Vector2.zero;
        }
        set
        {
            _instance._dir = value;
        }
    }

    static public Vector2 cameraInput;


    #endregion
    public class Key
    {
        #region atributes

        public bool enable;
        public float _timePressed;
        bool _startTimePressed = false;

        public KeyCode principal
        {
            get;
            private set;
        }

        public KeyCode secondary
        {
            get;
            private set;
        }

        public bool pressed
        {
            get
            {
                return CheckKey(principal, secondary, Input.GetKey);
            }
        }

        public bool down
        {
            get
            {
                return CheckKey(principal, secondary, Input.GetKeyDown);
            }
        }

        public bool up
        {
            get
            {
                return CheckKey(principal, secondary, Input.GetKeyUp);
            }
        }

        protected bool CheckKey(KeyCode p, KeyCode s, System.Func<KeyCode, bool> func)
        {
            return func(p) || func(s) && enable && eneable;
        }

        #endregion

        #region general functions
        public void MyUpdate()
        {
            if (_startTimePressed)
            {
                if (pressed)
                    _timePressed += Time.deltaTime;
                else
                    _timePressed = 0;
            }
        }

        /// <summary>
        /// setea si se comenzara a contar el tiempo presionado
        /// </summary>
        /// <param name="startTime">si comienza o termina</param>
        /// <returns>Devuelve el tiempo contado</returns>
        public float TimePressed(bool startTime = true)
        {
            _startTimePressed = startTime;
            if (!startTime)
                _timePressed = 0;
            return _timePressed;
        }

        public override string ToString()
        {
            return principal.ToString();
        }

        public void ChangeKey(KeyCode k, KeyCode k2)
        {
            principal = k;
            secondary = k2;
        }

        public void ChangeKey(KeyCode k)
        {
            principal = k;
        }
        #endregion

        #region constructor

        public Key(KeyCode k)
        {
            enable = true;
            ChangeKey(k);
            _keys.Add(this);
        }

        #endregion
    }

    static public bool eneable
    {
        set
        {
            if (value == false)
            {
                dir = Vector2.zero;
                cameraInput = Vector2.zero;
            }
            _instance.enabled = value;
        }
        get
        {
            return _instance.enabled;
        }
    }

    static public bool eneableMove;

    Vector2 _dir;

    public static void MouseLock()
    {
        Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public static void MouseLock(bool lockState)
    {
        Cursor.lockState = (lockState) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    #region unity functions

    private void Awake()
    {
        _instance = this;
        eneableMove = true;
    }

    void Update()
    {
        _dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        cameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //string txt = "";

        foreach (Key item in _keys)
        {
            item.MyUpdate();
            //txt += item._timePressed + "\n";
        }
    }
    #endregion
}
