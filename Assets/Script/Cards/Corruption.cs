﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : Card
{
    private void Start()
    {
        card.name = "타락";
        card.cost = 3;
        card.color = CardColor.Red;
        card.type = CardType.Power;
        card.grade = CardGrade.Legend;
        card.cardImagePath = "Sprite/CardImage/corruption";

        SpriteSetting();
        JsonManager.SaveJsonData(card, "Card", GetType().Name);
    }
    public override void Use(GameObject target)
    {
        Debug.Log("타락카드 사용");
    }
}