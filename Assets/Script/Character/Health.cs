using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IGetPercentage
{
    public float maxHp;
    public float hp;
    public float armor;

    public LogicActive damageLogic;

    [Range(0.01f,1f)]
    [SerializeField]
    float cooldownDamage;

    Timer _enf;

    void Awake()
    {
        hp = maxHp;

        _enf = TimersManager.Create(cooldownDamage);

        damageLogic = GetComponent<LogicActive>();

        gameObject.AddTags(Tag.life);
    }

    private void OnDestroy()
    {
        TimersManager.Destroy(_enf);
    }

    /// <summary>
    /// Setea la vida maxima y la vida actual
    /// </summary>
    /// <param name="num">valor de la vida a setear</param>
    public void SetHp(float num)
    {
        maxHp = num;
        hp = num;
    }

    /// <summary>
    /// Devuele el porcentage de vida del objeto
    /// </summary>
    /// <returns>valor entre 0 y 1</returns>
    public float Percentage()
    {
        return hp / maxHp;
    }

    /// <summary>
    /// Resta vida en caso de que el daño sea positivo y la suma en caso que sea negativo
    /// el daño es restado por la armadura, si la armadura es superior al daño, no se recibe daño
    /// No restara daño en caso de que el timer de enfriamiento no haya terminado
    /// Esta funcion ejecutara el activate de LogicActive que sea pasado como referencia en damageLogic cuando se reciba daño o se cure
    /// </summary>
    /// <param name="damage"></param>
    public void Substract(float damage)
    {
        damage = damage > 0 ? ( (damage - armor)>0 ? (damage - armor) : 0) : damage;

        if (_enf.Chck)
        {
            hp -= damage;

            _enf.Reset();

            damageLogic.Activate(damage, hp, Percentage());
        }
    }

    public Vector3 GetAll()
    {
        return new Vector3(maxHp, hp, armor);
    }

    public void SetAll(Vector3 set)
    {
        SetAll((int)set.x, (int)set.y, (int)set.z);
    }

    public void SetAll(int m, int h, int a)
    {
        maxHp = m;
        hp = h;
        armor = a;
    }

    
}