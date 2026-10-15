public class DeathBringerArrowState : DeathBringerState
{
    public DeathBringerArrowState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.1f;
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
