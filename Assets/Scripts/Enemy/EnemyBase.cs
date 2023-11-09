using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy",menuName = "Enemy/Create new enemy")]
public class EnemyBase : ScriptableObject
{
    [SerializeField]string name;
    
    [TextArea]
    [SerializeField]string description;

    [SerializeField]Sprite frontSprite;

    [SerializeField]EnemyType type1;
    [SerializeField]EnemyType type2;

    //Base Stats
    [SerializeField]public int maxHp;
    [SerializeField]public int attack;
    [SerializeField]public int defense;
    [SerializeField]public int agi;
    [SerializeField]public int mana;

    [SerializeField]List<LearnableMove> learnableMoves;
    public string Name{
        get{return name;}
    }

    public string Description{
        get{return description;}
    }

     public Sprite FrontSprite {
        get { return frontSprite; }
    }

    public EnemyType Type1 {
        get { return type1; }
    }

    public EnemyType Type2 {
        get { return type2; }
    }

    public int MaxHp {
        get { return maxHp; }
    }

    public int Attack {
        get { return attack; }
    }

    public int Defense {
        get { return defense; }
    }

    public int Agi{
        get{return agi;}
    }

    public int Mana{
        get{return mana;}
    }

    public List<LearnableMove> LearnableMoves{
        get{return learnableMoves;}
    }
}

[System.Serializable]
public class LearnableMove{
    [SerializeField]MoveBase moveBase;
    [SerializeField]int level;

    public MoveBase Base{
        get{ return moveBase;}
    }

    public int Level{
        get{return level;}
    }
}


public enum EnemyType{
    None,
    Axe,
    Hammer,
    Dagger,
    Sword,
    Bow,
    Lance,
    Wand,
    Tome
}

public class TypeChart
{
    static float[][] chart = {
        //           A  H  D  S  B  L  W  T
        new float[] {1f,1f,2f,2f,0.5f,0.5f,1f,1f}, //Axe
        new float[] {1f,1f,2f,2f,0.5f,0.5f,1f,1f}, //Hammer
        new float[] {0.5f,0.5f,1f,1f,1f,1f,2f,2f}, //Dagger
        new float[] {0.5f,0.5f,1f,1f,1f,1f,2f,2f}, //Sword
        new float[] {2f,2f,1f,1f,1f,1f,0.5f,0.5f}, //Bow
        new float[] {2f,2f,1f,1f,1f,1f,0.5f,0.5f}, //Lance
        new float[] {1f,1f,0.5f,0.5f,2f,2f,1f,1f}, //Wand
        new float[] {1f,1f,0.5f,0.5f,2f,2f,1f,1f}, // Tome
    };

    public static float GetAdvantage(EnemyType attackType, EnemyType defenseType){
        if(attackType == EnemyType.None || defenseType == EnemyType.None){
            return 1;
        }
        int row = (int)attackType -1;
        int col = (int)defenseType -1;

        return chart[row][col];
    }
}
