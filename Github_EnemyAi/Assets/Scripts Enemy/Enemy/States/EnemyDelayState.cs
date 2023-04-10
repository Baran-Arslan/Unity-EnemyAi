using UnityEngine;

public class EnemyDelayState : IState
{
    private readonly int _maxEnemyAttackAtOnce;
    private readonly float _waitTime;

    private float _deadline;

    public EnemyDelayState(float waitTime, int maxEnemyAttackAtOnce)
    {
        _waitTime = waitTime;
        _maxEnemyAttackAtOnce = maxEnemyAttackAtOnce;
    }

    public void OnEnter()
    {
        _deadline = Time.time + _waitTime;

        if (EnemyManager.Instance.CurrentAttackingEnemyCount < _maxEnemyAttackAtOnce)//Increase the number of enemies in the playermanager script
            EnemyManager.Instance.CurrentAttackingEnemyCount++;
    }
    public void OnExit()
    {

        if (EnemyManager.Instance.MaxEnemyAttackAtOnce > 0)//Reduce the number of enemies in the playermanager script
            EnemyManager.Instance.CurrentAttackingEnemyCount--;
    }
    public void Tick()
    {
    }
    public bool IsDone()
    {
        return Time.time > _deadline;
    }
}
