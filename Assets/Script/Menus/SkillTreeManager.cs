using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    static public SkillTreeManager instance;

    public DoubleString presentation;

    public RectTransform gridActives;

    public RectTransform allPowers;

    public static void SwitchPowers(bool b = false)
    {
        AbilitiesParent[] powers = instance.allPowers.GetComponentsInChildren<AbilitiesParent>();

        foreach (var item in powers)
        {
            var myCanvasGroup = item.GetComponent<CanvasGroup>();
            myCanvasGroup.blocksRaycasts = b;
        }
    }

    private void OnEnable()
    {
        DetailsWindow.ChangeAlpha(1, 0.5f);
        DetailsWindow.PreviewImage(false);
        DetailsWindow.ModifyTexts(presentation);
        DetailsWindow.HideMyButton(true);
        DetailsWindow.ActiveButtons(false);
    }

    private void OnDisable()
    {
        DetailsWindow.ChangeAlpha(0, 0.2f);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AbilitiesParent[] allAbilities = GameObject.FindObjectsOfTypeAll(typeof(AbilitiesParent)) as AbilitiesParent[];

        SkillSlot[] activeAbilities = gridActives.GetComponentsInChildren<SkillSlot>();
        int index = 0;

        print(allAbilities.Length);
        
        foreach (var item in allAbilities)
        {
            
            if(item.myAbility!=null && item.myAbility.active)
            {
                print(item.name + " fue cargado");

                item.transform.parent = activeAbilities[index].transform;
                index++;

                if (item.myAbility is Abilities.PowerInit<Toxine_Powers> || item.myAbility is Abilities.PowerInit<Vortex_Powers> || item.myAbility is Abilities.PowerInit<Teleport_Powers> || item.myAbility is Abilities.PowerInit<Stun_Powers>)
                    SwitchPowers();
            }
        }
       
    }
}
