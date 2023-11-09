using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move",menuName = "Enemy/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField]EnemyType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int stamina;



    public string Name{
        get{return name;}
    }

    public string Description{
        get{return description;}
    }
    public EnemyType Type{
        get{return type;}
    }
    public int Power{
        get{return power;}
    }
    public int Accuracy{
        get{return accuracy;}
    }
    public int Stamina{
        get{return stamina;}
    }

    public bool IsMagic{
        get{
            if(type == EnemyType.Wand || type == EnemyType.Tome) {
                return true;
            }else{
                return false;
            }
        }
    }

}
