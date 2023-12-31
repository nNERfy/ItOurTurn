using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Enemy
{
    [SerializeField] EnemyBase _base;
    [SerializeField] int level;

    public EnemyBase Base{get{return _base;}}
    public int Level{get{return level;}}

    public int HP{get; set;}

    public List<Move> Moves{get; set;}

    public void Init(){
        HP = MaxHp;

        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves){
            if(move.Level <= Level){
                Moves.Add(new Move(move.Base));
            }
            if(Moves.Count >= 4) break;
        }
    }

    public int MaxHp{
        get{return Mathf.FloorToInt((Base.maxHp*Level)/40f)+10;}
    }

    public int Attack{
        get{return Mathf.FloorToInt((Base.attack*Level)/40f)+5;}
    }

    public int Defense{
        get{return Mathf.FloorToInt((Base.defense*Level)/40f)+5;}
    }

    public int Agi{
        get{return Mathf.FloorToInt((Base.agi*Level)/40f)+5;}
    }

    public int Mana{
        get{return Mathf.FloorToInt((Base.mana*Level)/40f)+5;}
    }

    public DamageDetails TakeDamage(Move move,Enemy attacker){
        float critical = 1f;
        if(Random.value*100f<=6.25f) critical =2f;

        float type = TypeChart.GetAdvantage(move.Base.Type,this.Base.Type1)*TypeChart.GetAdvantage(move.Base.Type,this.Base.Type2);

        var damageDetails= new DamageDetails(){
            TypeAdvantage =type,
            Critical=critical,
            Defeated = false
        };

        float attack = (move.Base.IsMagic)? attacker.Mana : attacker.Attack;
        float defense = (move.Base.IsMagic)? 1 : Defense;

        float modifiers = Random.Range(0.85f,1f)*type*critical;
        float a =(2*attacker.Level+10)/250f;
        float d = a*move.Base.Power*((float)attack/defense)+2;
        int damage = Mathf.FloorToInt(d*modifiers);

        HP-=damage;
        if(HP<=0){
            HP = 0;
            damageDetails.Defeated=true;
        }
        return damageDetails;
    }

    public Move GetRandomMove(){
        int r = Random.Range(0,Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails{
    public bool Defeated{get; set;}
    public float Critical{get; set;}
    public float TypeAdvantage {get; set;}
}
