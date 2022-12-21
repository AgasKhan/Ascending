using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardPopUp : MonoBehaviour
{
    public GameObject[] particle;
    public int cantParticles;

    List<DoubleString> messages = new List<DoubleString>();

    TextMeshProUGUI text;
    Button boton;

    public void SetReward(List<DoubleString> messages)
    {
        this.messages = messages;
    }

    void SetText()
    {
        string aux ="";

        aux += "Mission Complete".RichText("color", "green").RichText("size","21") + "\n" + "\n";

        aux += messages[0].superior.RichText("size", "18") + "\n";

        aux += messages[0].inferior.RichText("size", "14");

        text.text = aux;

        for (int i = 0; i < cantParticles; i++)
        {
            float x = Random.Range(20, 81) * (Random.Range(0, 2) == 0 ? 1 : -1);
            float y = Random.Range(10, 41) * (Random.Range(0, 2) == 0 ? 1 : -1);


            Instantiate(particle[Random.Range(0,particle.Length)], new Vector3(x, y, 98), Quaternion.identity);
        }

        
    }


    void OnClickButton()
    {
        messages.RemoveAt(0);
        if (messages.Count > 0)
            SetText();
        else
            gameObject.SetActive(false);
    }

    // Start is called before the first frame update

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        boton = GetComponentInChildren<Button>();
        boton.onClick.AddListener(OnClickButton);
    }

    void Start()
    {
        gameObject.SetActive(false);

        if (messages.Count != 0)
            TimersManager.Create(3, () => {

                gameObject.SetActive(true);
                SetText();

            });
    }

}
