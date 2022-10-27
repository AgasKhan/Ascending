using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controllers : MonoBehaviour
{


    #region static classes
    static List<Key> _keys = new List<Key>();

    static public Vector2 dir;

    static public Vector2 cameraInput;

    static public Key attack = new Key(KeyCode.Mouse0);

    static public Key aim = new Key(KeyCode.Mouse1);

    static public Key active = new Key(KeyCode.E);

    static public Key power = new Key(KeyCode.Q);

    static public Key jump = new Key("Jump");

    static public Key dash = new Key(KeyCode.LeftShift);

    static public Key flip = new Key(KeyCode.F);

    static public Key locked = new Key(KeyCode.Tab);

    static Controllers _instance;

    #endregion
    public class Key
    {
        #region atributes

        public bool pressed;
        public bool down;
        public bool up;
        KeyCode key;
        string strKey;
        public float _timePressed;
        bool _startTimePressed = false;

        #endregion

        #region general functions
        public void MyUpdate()
        {
            if (strKey != null && strKey != "")
            {
                pressed = Input.GetButton(strKey);
                down = Input.GetButtonDown(strKey);
                up = Input.GetButtonUp(strKey);
            }
            else
            {
                pressed = Input.GetKey(key);
                down = Input.GetKeyDown(key);
                up = Input.GetKeyUp(key);
            }

            if (_startTimePressed)
            {
                if (pressed)
                    _timePressed += Time.deltaTime;
                else
                    _timePressed = 0;
            }
        }

        /// <summary>
        /// setea si se comenzara a conttar el tiempo presionado
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

        public void ChangeKey(string s)
        {
            strKey = s;
        }
        public void ChangeKey(KeyCode k)
        {
            key = k;
            strKey = null;
        }
        #endregion

        //Se podria optimizar para usar una sola funcion constructora que llamase a la changekey correspondiente?
        #region constructor
        public Key(string s)
        {
            ChangeKey(s);
            _keys.Add(this);
        }

        public Key(KeyCode k)
        {
            ChangeKey(k);
            _keys.Add(this);
        }

        #endregion
    }

    static public bool eneable
    {
        set
        {
            _instance.enabled = value;
        }

        get
        {
            return _instance.enabled;
        }
    }

    #region unity functions
    private void Awake()
    {
        _instance = this;
    }
    void Update()
    {
        cameraInput = new Vector2( Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        string txt = "";

        foreach (Key item in _keys)
        {
            item.MyUpdate();
            txt += item._timePressed+"\n";
        }
    }
    #endregion
}
