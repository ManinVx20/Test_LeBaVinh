using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : IEnemyState
{
    private float _changeStateTimer;
    private float _changeStateTime;

    public void Enter(Enemy enemy)
    {
        enemy.SetMoveDirection(Vector3.zero);

        _changeStateTimer = 0.0f;
        _changeStateTime = Random.Range(1.0f, 1.0f);
    }

    public void Execute(Enemy enemy)
    {
        _changeStateTimer += Time.deltaTime;
        if (_changeStateTimer > _changeStateTime)
        {
            _changeStateTimer = 0.0f;

            enemy.ChangeState(((Boss)enemy).PatrolState);
        }
    }

    public void Exit(Enemy enemy)
    {

    }
}
