using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Consola : MonoBehaviour
{
    [SerializeField]
    string OpenConsoleCharacter;

    public TextMeshProUGUI salida;
    public GameObject canvas;
    public Scrollbar scrollbar;

    public static Consola instancia;

    List<string> texto;

    int pagina = 0;

    bool actualizar = true;

    // Start is called before the first frame update
    void Start()
    {
        DebugPrint.Error("");
        DebugPrint.Log("");
        DebugPrint.Warning("");


        if (instancia != null)
            Destroy(gameObject);
        else
            instancia = this;

        DontDestroyOnLoad(this);

        texto = new List<string>();

        texto.Add(
            "<color=green>Consola</color>" +
            "\n" +
            "Usa la ruedita del mouse para desplazarte por las paginas" +
            "\n" +
            "Tambien puedes utilizar Re pág y Av Pág, para moverte entre paginas" +
            "\n" +
            "Desarrollado por Lucas Euler"
            );
    }

    // Update is called once per frame
    void Update()
    {


        if (DebugPrint.chk())
        {
            string aux;
            int number = 0;

            List<string> arrAux = new List<string>();

            if (texto[texto.Count - 1].Split('\n').Length < 199)
            {
                arrAux.AddRange(texto[texto.Count - 1].Split('\n'));
                texto.RemoveAt(texto.Count - 1);
            }

            arrAux.AddRange( DebugPrint.PrintSalida().Split('\n'));
            
            for (int i = 0; i < (arrAux.Count/200 + 1); i++)
            {
                aux = "";

                do
                {
                    aux += "\n"+arrAux[number];
                    number++;
                }
                while (number<arrAux.Count && number%200!=0);

                aux += "\n\n<color=grey>--------Cambio de frame--------</color>\n\n";

                texto.Add(aux.Trim());
            }

            pagina = texto.Count - 1;
            actualizar = true;
        }

        if (Input.inputString== OpenConsoleCharacter)
            canvas.SetActive(!canvas.activeSelf);

        if(canvas.activeSelf)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                scrollbar.value = Mathf.Lerp(scrollbar.value, Input.mouseScrollDelta.y < 0 ? 0 : 1, 1080 / salida.rectTransform.sizeDelta.y);

                if ( (Input.mouseScrollDelta.y > 0 && (scrollbar.value > 0.99f || !scrollbar.enabled) ) )
                {
                    Anterior();
                }
                else if ( (Input.mouseScrollDelta.y < 0 && (scrollbar.value < 0.01f || !scrollbar.enabled) ) )
                {
                    Siguiente();
                }
            }

            if(Input.GetKeyDown(KeyCode.PageUp))
            {
                Anterior();
            }

            else if(Input.GetKeyDown(KeyCode.PageDown))
            {
                Siguiente();
            }

            if (actualizar)
            {
                if(pagina<texto.Count)
                    salida.text = "-----------------------------------------------------------------------------" +
                        "\n\n<color=grey>Pagina: " + pagina + " de " + (texto.Count - 1) + "</color>\n\n" + texto[pagina] +
                        "\n\n<color=grey>Pagina: " + pagina + " de " + (texto.Count - 1) + "</color>\n\n" +
                        "-----------------------------------------------------------------------------";
                actualizar = false;
            }

            if (salida.rectTransform.rect.height != salida.renderedHeight)
                salida.rectTransform.sizeDelta = new Vector2(salida.rectTransform.sizeDelta.x, salida.renderedHeight);
        }
    }

    void Siguiente()
    {
        if(pagina < (texto.Count - 1))
        {
            pagina++;
            actualizar = true;
            scrollbar.value = 1;
        }
    }

    void Anterior()
    {
        if(pagina > 0)
        {
            pagina--;
            actualizar = true;
            scrollbar.value = 0;
        }
    }




}


static class DebugPrint
{
    static PrintF debug;
    static PrintF warning;
    static PrintF error;

    public static void Log(string t)
    {
        debug.Add(t);
    }

    public static void Warning(string t)
    {
        warning.Add("<color=yellow>" + t + "</color>");
    }

    public static void Error(string t)
    {
        error.Add("<color=red>" + t + "</color>");
    }

    public static bool chk()
    {
        if (debug.LenghtChk() || error.LenghtChk() || warning.LenghtChk())
            return true;
        return false;
    }

    public static string PrintSalida()
    {
        if (chk())

            return error.Out() +
                    warning.Out() +
                    debug.Out();

        return "";
    }


    public static void PrintConsola()
    {
        error.Print("error");
        warning.Print("warning");
        debug.Print();
    }

}

struct PrintF
{
    string pantalla;

    public void Add(string palabra)
    {
        pantalla += "\n" + palabra;

        /*if (pantalla != null && pantalla != "")
            pantalla += "\n" + palabra;
        else
            pantalla = palabra;*/
    }

    public string Out()
    {
        string aux = pantalla;
        pantalla = "";
        return aux;

    }

    public bool LenghtChk()
    {
        return pantalla.Length > 0 ? true : false;
    }

    public void Print(string debugMode = "debug")
    {

        if (pantalla != null && pantalla != "")
        {
            switch (debugMode)
            {
                case "warning":
                    Debug.LogWarning(pantalla);
                    break;

                case "error":
                    Debug.LogError(pantalla);
                    break;

                default:
                    Debug.Log(pantalla);
                    break;
            }
        }
        pantalla = "";
    }

    public void Clear()
    {
        pantalla = "";
    }
}

