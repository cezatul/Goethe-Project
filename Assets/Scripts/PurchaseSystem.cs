﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseSystem : MonoBehaviour
{
    [Serializable]
    public class CharacterInfo
    {
        public string name;
        public int cost;
        public bool isPurchased
        {
            get
            {
                return PlayerPrefs.GetInt(name + "isPurchased", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(name+"isPurchased", (value)?1:0);
            }
        }

        public Sprite sprite;
    }
    
    public CharacterInfo[] characterInfos;

    public TextMeshProUGUI TotalScrapText;
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI ActivateText;
    public Image previewImage;

    private int currentCharacterIndex = 0;

    private void Awake()
    {
        TotalScrapText.text = GameManager.Instance.TotalScrap.ToString();

        for(int i = 0; i < 9; ++i)
            PlayerPrefs.DeleteKey(characterInfos[i].name + "isPurchased");
        GameManager.Instance.TotalScrap = 20;
    }

    public void OnActivateCharacterButton()
    {
        ActivateCharacter(currentCharacterIndex);
    }

    public void OnIndexButton(int index)
    {
        currentCharacterIndex = index;
        UpdateInfo(index);
    }
    
    public void ActivateCharacter(int index)
    {
        if (characterInfos[index].isPurchased) // Activate
        {
            GameManager.Instance.CurrentCharacterModelIndex = index;
            
            UpdateInfo(index);
        }
        else if(characterInfos[index].cost <= GameManager.Instance.TotalScrap) // Purchase
        {
            GameManager.Instance.TotalScrap -= characterInfos[index].cost; // Transaction
            characterInfos[index].isPurchased = true;

            GameManager.Instance.CurrentCharacterModelIndex = index; // Activation By Default
            
            UpdateInfo(index);
        }
        else
        {
            ActivateText.text = "Not enough Scrap _";
        }
        
    }

    public void UpdateInfo(int index)
    {
        // Infos
        NameText.text = characterInfos[index].name;
        previewImage.sprite = characterInfos[index].sprite;

        // Activation Button
        if (index == GameManager.Instance.CurrentCharacterModelIndex)
            ActivateText.text = "Activated";
        else
        {
            if (characterInfos[index].isPurchased)
            {
                ActivateText.text = "[Activate]";
            }
            else
            {
                CostText.text = "Cost: " + characterInfos[index].cost; // Print Cost
                ActivateText.text = "[Purchase]";
            }
        }

        TotalScrapText.text = GameManager.Instance.TotalScrap.ToString();
    }
}
