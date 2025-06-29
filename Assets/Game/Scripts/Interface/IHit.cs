using UnityEngine;
using UnityEngine.Events;

public interface IHit
{
    public void OnHit(float damage, UnityAction<EnemyReward[]> actionDeath = null);
}
