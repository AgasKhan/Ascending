using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsCall : MonoBehaviour
{

    MenuManager menu;

    public void Event(GameObject g)
    {
        if (g.CompareTags("Configurado"))
            return;

        print("configurando: " + g.name);

        g.AddTags("Configurado");

        if (g.TryGetComponent(out Button b))
        {
            print("configurado boton");
            b.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            b.onClick.AddListener(() => { menu.eventListVoid[g.name](); });
            menu.eventListVoid[g.name]();
            return;
        }
        else if (g.TryGetComponent(out Slider s))
        {
            print("configurado slider");
            //s.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            s.onValueChanged.AddListener((float f) => { menu.eventListFloat[g.name](f); });
            menu.eventListFloat[g.name](s.value);
            return;
        }

        DebugPrint.Warning("no contiene uno de los componentes esperados: " + g.name);
    }
    void Start()
    {
        menu = MenuManager.instance;
    }
}
