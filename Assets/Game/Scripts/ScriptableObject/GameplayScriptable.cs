using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayScriptable", menuName = "Game/GameplayScriptable")]
public class GameplayScriptable : ScriptableObject
{
    public ItemBaseGameplay CoinPrefab;
    public ItemHPGameplay HPPrefab;
    public Boss BossPrefab;
    public Chest ChestPrefab;

}
