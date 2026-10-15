using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!GameManager.instance.consule_UI.consule_Input_UI.isInputting){
            if (Input.GetKeyDown(/*KeyCode.Mouse0*/ KeyBoardManager.instance.keyboardsDictionary["Attack"]))
            {
                stateMachine.ChangeState(player.primaryAttackState);
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyBoardManager.instance.keyboardsDictionary["Attack"]))
            {
                stateMachine.ChangeState(player.airLaunchAttackState);
            }

            if (Input.GetKeyDown(/*KeyCode.Q*/ KeyBoardManager.instance.keyboardsDictionary["Parry"]) && player.skill.parry.parryUnlocked && player.skill.parry.SkillIsReadyToUse())
            {
                //stateMachine.ChangeState(player.counterAttackState);
                SkillManager.instance.parry.UseSkillIfAvailable();
            }

            if (Input.GetKeyDown(/*KeyCode.R*/ KeyBoardManager.instance.keyboardsDictionary["Blackhole"]) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.SkillIsReadyToUse())
            {
                stateMachine.ChangeState(player.blackholeSkillState);
            }

            //!player.sword   is same as   player.sword == null
            //HasNoSword() will return true if there's no sword in player
            //or return falase and return the sword if player has sword
            if (Input.GetKeyDown(/*KeyCode.Mouse1*/ KeyBoardManager.instance.keyboardsDictionary["Aim"]) && player.HasNoSword() && player.skill.sword.throwSwordSkillUnlocked)
            {
                stateMachine.ChangeState(player.aimSwordState);
            }


            if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.jumpState);
            }
            if (player.IsGroundDetected() && player.isOnPlatform)
            {
                if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
                {
                    player.JumpOffPlatform();
                    return;
                }
            }
        }
        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

    }
}
