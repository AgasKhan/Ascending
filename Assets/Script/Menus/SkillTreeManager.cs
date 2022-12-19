using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    static public SkillTreeManager instance;

    public RectTransform gridActives;

    public RectTransform allPowers;

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
                item.transform.parent = activeAbilities[index].transform;
                index++;
            }
        }
        
        
    }

    private void Awake()
    {
        instance = this;
    }

    public static void SwitchPowers(bool b = false)
    {
        AbilitiesParent[] powers = instance.allPowers.GetComponentsInChildren<AbilitiesParent>();

        foreach (var item in powers)
        {
            var myCanvasGroup = item.GetComponent<CanvasGroup>();
            myCanvasGroup.blocksRaycasts = b;
        }
    }
}
