using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeElements : MonoBehaviour
{
    public float journeyTime = 1.0f;

    public Player_Character character;

    public KnifeElements Other;

    public Vector3 distance;

    [SerializeField]
    protected static List<Knifes> elements;


    [System.Serializable]
    protected class Knifes
    {
        public Transform reference;

        public Dagger_Proyectile daggerScript;

        /// <summary>
        /// posicion a la que girara estando en orbita
        /// </summary>
        public Vector3 position;

        public MoveRotAndGlueRb movement;

        public Knifes(Transform r, Vector3 p, Character character)
        {
            reference = r;
            position = p;

            reference.localScale = Vector3.one;

            movement = r.GetComponent<MoveRotAndGlueRb>();
            movement.Rotate(Vector3.zero);
            movement.kinematic = true;
            movement.eneableDrag = false;

            daggerScript = r.GetComponent<Dagger_Proyectile>();
            daggerScript.owner = character;

            //daggerScript.transform.GetChild(0).GetComponent<Interactuable_LogicActive>().diseable = false;
        }
    }

    protected void RefreshUI(int n = 0)
    {
        character.MainHUDDaggers(elements.Count+n);
    }

    private void Start()
    {
        elements = new List<Knifes>();
        character = transform.parent.GetComponent<Player_Character>();
    }
}
