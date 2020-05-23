﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Shop : MonoBehaviour
{
    public GameObject charCard;
    public GameObject artifact;
    public GameObject shopHand;
    public HorizontalLayoutGroup[] groups;

    private void OnEnable()
    {
        Vector3 pos = new Vector3(0, 10, 0);
        gameObject.transform.DOLocalMove(pos, 0.1f);
       
        UIManager.instance.proceedButton.SetActive(false);  
    }

    private void OnDisable()
    {
        Vector3 pos = new Vector3(0, 1000, 0);
        gameObject.transform.localPosition = pos;
        UIManager.instance.proceedButton.SetActive(true);
    }

    void CardReset(GameObject gameObject)
    {
        while (gameObject.transform.childCount != 0)
        {
            ObjectPoolManager.instance.ReturnCard(gameObject.transform.GetChild(0).GetComponent<Card>());
        }
    }

    public void OpenShop()
    {
        UIManager.instance.choice = ChoiceMode.Shop;
        gameObject.SetActive(true);
        StartCoroutine(test());

        // groups[0].enabled = false;  
    }

    public void GetCards()
    {
        RandmCardRewardNomal(charCard, CardColor.Red, CardGrade.Nomal);
        RandmCardRewardNomal(charCard, CardColor.Red, CardGrade.Rare);
        RandmCardRewardNomal(charCard, CardColor.Red, CardGrade.Nomal);
        RandmCardRewardNomal(charCard, CardColor.Red, CardGrade.Nomal);
        RandmCardRewardNomal(charCard, CardColor.Red, CardGrade.Nomal);
        groups = GetComponentsInChildren<HorizontalLayoutGroup>();

    }

    IEnumerator test()
    {
        groups[0].enabled = true;
        yield return null;
        groups[0].enabled = false;
    }

    public void RandmCardRewardNomal(GameObject parent, CardColor cardColor, CardGrade cardGrade)
    {
        List<string> list = ObjectPoolManager.instance.cardList;
        bool ok = true;

        while (ok)
        {
            int random = Random.Range(0, list.Count);
            Card shopCard = ObjectPoolManager.instance.GetCard(list[random]);

            if (shopCard.name == "타격" || shopCard.name == "수비" || shopCard.name == "타격+" || shopCard.name == "수비+")
            {
                ObjectPoolManager.instance.ReturnCard(shopCard);
                continue;
            }

            if (shopCard.card.type == CardType.CC || shopCard.card.type == CardType.Curse)
            {
                ObjectPoolManager.instance.ReturnCard(shopCard);
                continue;
            }

            if (shopCard.card.color != cardColor)
            {
                ObjectPoolManager.instance.ReturnCard(shopCard);
                continue;
            }
            bool check = false;

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (shopCard.name == parent.transform.GetChild(i).name)
                {
                    ObjectPoolManager.instance.ReturnCard(shopCard);
                    check = true;
                    break;
                }
            }

            if (check)
            {
                continue;
            }

            if (shopCard.card.grade == cardGrade)
            {
                switch (cardGrade)
                {
                    case CardGrade.Nomal:
                        shopCard.Price = Random.Range(10, 50);
                        break;
                    case CardGrade.Rare:
                        shopCard.Price = Random.Range(50, 100);
                        break;
                    case CardGrade.Legend:
                        shopCard.Price = Random.Range(100, 150);
                        break;
                }
                shopCard.gameObject.transform.SetParent(parent.transform);
                shopCard.gameObject.SetActive(true);
                ok = false;
            }
        }
    }
    public void rerereset()
    {
        CardReset(charCard);
    }

    public void CloseShopPanel()
    {

        gameObject.SetActive(false);
    }
}