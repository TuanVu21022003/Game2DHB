using System;
using UnityEngine;


[Serializable]
public class EnemyReward
{
    public EnemyRewardType type;
    public int amount;
    public string name;
}

public enum EnemyRewardType
{
    None,
    Gold,
}
