using TMPro;
using UnityEngine;

public class ItemHPGameplay : ItemBaseGameplay
{
    [SerializeField] private TextMeshProUGUI amountText;

    public override void OnInit(Transform target, int amount)
    {
        base.OnInit(target, amount);
        amountText.text = $"+{amount}%";
    }
}
