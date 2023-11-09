using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{FreeRoam,Battle}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start(){
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle(){
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerWeapon = playerController.GetComponent<EnemyWeapon>();
        var wildEnemy = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildEnemy();

        battleSystem.StartBattle(playerWeapon,wildEnemy);
    }

    void EndBattle(bool won){
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Backspace)){
            Exit();
        }else if(state == GameState.FreeRoam){
            playerController.HandleUpdate();
        }else if (state == GameState.Battle){
            battleSystem.HandleUpdate();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
