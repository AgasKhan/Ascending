using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsCall : MonoBehaviour
{

    MenuManager menu;

    public void Event(GameObject g)
    {
        if (g.TryGetComponent(out Button b))
        {
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => { menu.eventListVoid[g.name](); });
            menu.eventListVoid[g.name]();
            return;
        }
        else if (g.TryGetComponent(out Slider s))
        {
            s.onValueChanged.RemoveAllListeners();
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
