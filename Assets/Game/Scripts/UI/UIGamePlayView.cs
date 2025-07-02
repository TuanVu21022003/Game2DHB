using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGamePlayView : UIBaseView
{
    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private Transform heartParent;
    [SerializeField] private HeartItemUI heartItemUIPrefab;

    private List<HeartItemUI> listHeart = new();

    public void OnInit(int heartCount, int coin)
    {
        for(int i = 0; i < heartCount; i++)
        {
            HeartItemUI heartItemUI = Instantiate(heartItemUIPrefab, heartParent);
            listHeart.Add(heartItemUI);

        }
        UpdateCoin(coin);
    }

    public override void Show()
    {
        base.Show();
        
    }

    public void UpdateCoin(int coin)
    {
        textCoin.text = coin.ToString();
    }

    public void UpdateHeart(int countHeart)
    {
        Debug.Log($"UpdateHeart: {countHeart}");
        for (int i = 0; i < listHeart.Count; i++)
        {
            if (i >= countHeart && listHeart[i].IsActive())
            {
                listHeart[i].EffectDestroy();
            }
            else
            {
                listHeart[i].gameObject.SetActive(true);
            }
        }
    }
}
