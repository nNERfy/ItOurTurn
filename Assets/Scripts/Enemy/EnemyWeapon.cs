using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]List<Enemy> enemys;

    public List<Enemy> Enemys{get{
        return enemys;
    }}

    private void Start(){
        foreach (var enemy in enemys){
            enemy.Init();
        }
    }

    public Enemy GetFullWeapon(){
        return enemys.Where(x => x.HP > 0).FirstOrDefault();
    }
}
