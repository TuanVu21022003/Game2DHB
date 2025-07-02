using System;
using UnityEngine;


[Serializable]
public class EnemyReward
{
    public ItemType type;
    public int amount;
}

public enum ItemType
{
    None,
    Gold,
    HP,
}
