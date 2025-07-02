using UnityEngine;
using UnityEngine.Events;

public interface IHit
{
    public void OnHit(float damage, Character attacker, UnityAction<EnemyReward[]> actionDeath = null);
}
