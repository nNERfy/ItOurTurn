using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState{Start,PlayerAction,PlayerMove,EnemyMove,Busy,SoulScreen}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] SoulScreen soulScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;

    EnemyWeapon playerWeapon;
    Enemy wildEnemy;

    public void StartBattle(EnemyWeapon playerWeapon, Enemy wildEnemy){
        this.playerWeapon = playerWeapon;
        this.wildEnemy = wildEnemy;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle(){
        playerUnit.Setup(playerWeapon.GetFullWeapon());
        playerHud.SetData(playerUnit.Enemy);
        enemyUnit.Setup(wildEnemy);
        enemyHud.SetData(enemyUnit.Enemy);

        soulScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Enemy.Moves);

        yield return dialogBox.TypeDialog($"A Wild {enemyUnit.Enemy.Base.Name} appeared.");

        PlayerAction();
    }

    void PlayerAction(){
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action....");
        dialogBox.EnableSelector(true);
    }

    void OpenSoulScreen(){
        state = BattleState.SoulScreen;
        soulScreen.SetSoulData(playerWeapon.Enemys);
        soulScreen.gameObject.SetActive(true);
    }

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogBox.EnableSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    void Help(){
        dialogBox.SetDialog("Heavy Skl > Standard Skl > Mage Skl > RangerSkl \nGuess what would be the best!");
    }

    void Run(){
        dialogBox.SetDialog("Can't even run away!");
    }


    IEnumerator PerformPlayerMove(){
        state = BattleState.Busy;

        var move = playerUnit.Enemy.Moves[currentMove];
        move.Stamina--;
        yield return dialogBox.TypeDialog($"{playerUnit.Enemy.Base.Name} used {move.Base.Name}.");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();
        var damageDetails = enemyUnit.Enemy.TakeDamage(move,playerUnit.Enemy);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if(damageDetails.Defeated){
            yield return dialogBox.TypeDialog($"{enemyUnit.Enemy.Base.Name} has been defeated.");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }else {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove(){
        state = BattleState.EnemyMove;

        var move = enemyUnit.Enemy.GetRandomMove();
        move.Stamina--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Enemy.Base.Name} used {move.Base.Name}.");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();
        var damageDetails = playerUnit.Enemy.TakeDamage(move,enemyUnit.Enemy);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if(damageDetails.Defeated){
            yield return dialogBox.TypeDialog($"{playerUnit.Enemy.Base.Name} has been defeated.");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);

            var nextEnemy = playerWeapon.GetFullWeapon();
            if(nextEnemy != null){
                OpenSoulScreen();
            }else{
                OnBattleOver(false);
            }
        }else {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails){
        if(damageDetails.Critical > 1f) yield return dialogBox.TypeDialog("A critical hit!");
        if(damageDetails.TypeAdvantage > 1f) yield return dialogBox.TypeDialog("An advantage hit!!!");
        else if(damageDetails.TypeAdvantage < 1f) yield return dialogBox.TypeDialog("An disadvantage move!");
    }

    public void HandleUpdate(){
        if(state == BattleState.PlayerAction){
            HandleSelection();
        }else if (state == BattleState.PlayerMove){
            HandleMoveSelection();
        }else if (state == BattleState.SoulScreen){
            HandleSoulSelection();
        }
    }

    void HandleSelection(){
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            ++currentAction;
        }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            --currentAction;
        }else if(Input.GetKeyDown(KeyCode.DownArrow)){
            currentAction+=2;
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            currentAction-=2;
        }

        currentAction = Mathf.Clamp(currentAction,0,3);

        dialogBox.UpdateSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Return)){
            if(currentAction == 0){
                //Fight
                PlayerMove();
            }else if (currentAction == 1){
                // Help
                Help();
            }else if (currentAction == 2){
                // Soul
                OpenSoulScreen();
            }else if (currentAction == 3){
                // Run
                Run();
            }
        }
    }

    void HandleMoveSelection(){
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            ++currentMove;
        }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            --currentMove;
        }else if(Input.GetKeyDown(KeyCode.DownArrow)){
            currentMove+=2;
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            currentMove-=2;
        }

        currentMove = Mathf.Clamp(currentMove,0,playerUnit.Enemy.Moves.Count -1 );

        dialogBox.UpdateMoveSelection(currentMove,playerUnit.Enemy.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.Return)){
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Escape)){
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }

    void HandleSoulSelection(){
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            currentMember+=2;
        }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            currentMember-=2;
        }else if(Input.GetKeyDown(KeyCode.DownArrow)){
            ++currentMember;
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            --currentMember;
        }

        currentMember = Mathf.Clamp(currentMember,0,playerWeapon.Enemys.Count -1);

        soulScreen.UpdateMemberSelection(currentMember);

        if(Input.GetKeyDown(KeyCode.Return)){
            var selectedMember = playerWeapon.Enemys[currentMember];
            if(selectedMember.HP <= 0){
                soulScreen.SetMeassageText("You can't change to empty soul!");
                return;
            }
            if (selectedMember == playerUnit.Enemy){
                soulScreen.SetMeassageText("You can't repeating your soul!");
                return;
            }

            soulScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchSoul(selectedMember));
        }else if(Input.GetKeyDown(KeyCode.Escape)){
            soulScreen.gameObject.SetActive(false);
            PlayerAction();
        }
    }

    IEnumerator SwitchSoul(Enemy newEnemy){

        if(playerUnit.Enemy.HP>0){
        yield return dialogBox.TypeDialog($"......");
        playerUnit.PlayFaintAnimation();
        yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newEnemy);
        playerHud.SetData(newEnemy);       
        dialogBox.SetMoveNames(newEnemy.Moves); 
        yield return dialogBox.TypeDialog($"Switching to {newEnemy.Base.Name} !");

        StartCoroutine(EnemyMove());
    }

}
