using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controllers : MonoBehaviour
{
    #region static classes
    static List<Button> _keys = new List<Button>();

    static public Button attack = new Button(KeyCode.Mouse0);

    static public Button aim = new Button(KeyCode.Mouse1);

    static public Button active = new Button(KeyCode.E);

    static public Button power = new Button(KeyCode.Q);

    static public Button jump = new Button(KeyCode.Space);

    static public Button dash = new Button(KeyCode.LeftShift);

    static public Button flip = new Button(KeyCode.F);

    static public Button locked = new Button(KeyCode.Tab);

    static public Button pause = new Button(KeyCode.Escape);

    static public Axis horizontalMouse = new Axis("Mouse X");

    static public Axis verticalMouse = new Axis("Mouse Y");

    static public Axis horizontal = new Axis("Horizontal");

    static public Axis vertical = new Axis("Vertical");


    static Controllers _instance;

    /*
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
    */

    #endregion

    #region class

    public class Key<T,B>
    {
        public bool enable;

        public System.Func<T, B> pressFunc;
        public System.Func<T, B> downFunc;
        public System.Func<T, B> upFunc;

        public T principal
        {
            get;
            private set;
        }

        public T secondary
        {
            get;
            private set;
        }

        public B pressed
        {
            get
            {
                return CheckKey(principal, secondary, pressFunc);
            }
        }

        public B down
        {
            get
            {
                return CheckKey(principal, secondary, downFunc);
            }
        }

        public B up
        {
            get
            {
                return CheckKey(principal, secondary, upFunc);
            }
        }

        protected B CheckKey(T p, T s, System.Func<T, B> func)
        {
            if(enable && eneable)
            {
                return func(p);
            }
            else
                return default;
        }

        public void ChangeKey(T k, T k2)
        {
            principal = k;
            secondary = k2;
        }

        public void ChangeKey(T k)
        {
            principal = k;
        }

    }

    public class Button 
    {
        #region atributes

        Key<KeyCode, bool> key;
        
        public float _timePressed;
        bool _startTimePressed = false;

        public bool up => key.up;
        public bool down => key.down;
        public bool pressed => key.pressed;
        public bool enable
        {
            get
            {
                return key.enable;
            }

            set
            {
                key.enable = value;
            }

        }
        public KeyCode principal => key.principal;

        #endregion

        #region general functions
        public void MyUpdate()
        {
            if (_startTimePressed)
            {
                if (key.pressed)
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
            return key.principal.ToString();
        }


        #endregion

        #region constructor

        public Button(KeyCode k)
        {
            key = new Key<KeyCode, bool>();

            key.enable = true;
            key.ChangeKey(k);

            key.pressFunc = Input.GetKey;
            key.downFunc = Input.GetKeyDown;
            key.upFunc = Input.GetKeyUp;

            _keys.Add(this);
        }

        #endregion
    }

    public class Axis
    {
        Key<string, float> key;
        public bool enable
        {
            get
            {
                return key.enable;
            }

            set
            {
                key.enable = value;
            }
        
        } 
        public float pressed => key.pressed;
        public override string ToString()
        {
            return key.principal.ToString();
        }

        public Axis(string keys)
        {
            key = new Key<string, float>();

            key.enable = true;
            key.ChangeKey(keys);

            key.pressFunc = Input.GetAxis;
        }
    }

    #endregion

    static public bool eneable
    {
        set
        {
            /*
            if (value == false)
            {
                dir = Vector2.zero;
                cameraInput = Vector2.zero;
            }
            */
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
        /*
        _dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        cameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        */

        //string txt = "";

        foreach (Button item in _keys)
        {
            item.MyUpdate();
            //txt += item._timePressed + "\n";
        }
    }
    #endregion
}
