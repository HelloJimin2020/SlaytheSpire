﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    private static GameManager m_instance;
    #endregion

    bool isPlayerTurn;

    public int currentCost;
    public int maxCost;

    public List<Card> inventory;

    public GameObject myDeck;
    public GameObject myHand;
    public GameObject myCemetary;
    public GameObject myExhaustZone;

    public Player player;
    public GameObject cardPrefab;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player>();
        maxCost = 3;
    }

    void Start()
    {
        CreateMyInventory();
        startGame();
    }

    void startGame()
    {
        currentCost = maxCost;
        isPlayerTurn = true;
        SettingMyHand();
        TurnProcessing();
        UIManager.instance.SettingUI();
    }

    void SettingMyHand()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].transform.SetParent(myDeck.transform);
        }
        ShuffleDeck(myDeck);
    }

    public static void ShuffleDeck(GameObject deck)
    {
        int random;

        for (int i = 0; i < deck.transform.childCount; ++i)
        {
            random = Random.Range(0, deck.transform.childCount);

            deck.transform.GetChild(i).SetSiblingIndex(random);
        }
    }

    public void Draw()
    {
        myDeck.transform.GetChild(0).gameObject.SetActive(true);
        myDeck.transform.GetChild(0).SetParent(myHand.transform);
    }

    void TurnProcessing()
    {
        if (isPlayerTurn)
        {
            Debug.Log("내턴");
            while (myHand.transform.childCount <5)
            {
                Draw();
            }
        }
        else
        {
            Debug.Log("적턴");
            ChangeTurn();
        }
        UIManager.instance.SettingUI();
    }

    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        TurnProcessing();
    }

    void CreateMyInventory()
    {
        List<string> inven = JsonManager.LoadJsonData<List<string>>(player.gameObject.name, player.gameObject.name+"List");

        for (int i = 0; i < inven.Count; i++)
        {
            GameObject test = Instantiate(cardPrefab , transform.Find("Canvas/Inventory"));
            test.AddComponent(System.Type.GetType(inven[i]));

            Card newCard = test.GetComponent<Card>();
            newCard.transform.SetParent(transform.Find("Canvas/Inventory"));
            newCard.transform.localScale = new Vector3(2, 2, 2);
            newCard.cardInit();
            newCard.gameObject.SetActive(false);

            inventory.Add(newCard);
        }
    }
}