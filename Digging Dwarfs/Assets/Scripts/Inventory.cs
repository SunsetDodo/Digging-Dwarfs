using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private List<TextMeshProUGUI> stoneTexts;
    
    public int coinCount;
    public int[] stones = new int[5];

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    public void AddStone(int amount, int index)
    {
        stones[index] += amount;
        
        Debug.Log("Adding Stone");
        
        UpdateUI();
    }

    public void SellStone(int index, bool all)
    {
        if (stones[index] == 0) return;
        
        var price = index switch
        {
            0 => 1,
            1 => 2,
            2 => 5,
            3 => 15,
            4 => 50,
            _ => 0
        };

        if (all)
        {
            coinCount += price * stones[index];
            stones[index] = 0;
        }
        else
        {
            coinCount += price;
            stones[index]--;
        }

        UpdateUI();
    }

    public void SellWhite(bool all)
    {
        SellStone(0, all);
    }
    public void SellBlack(bool all)
    {
        SellStone(1, all);
    }
    public void SellOrange(bool all)
    {
        SellStone(2, all);
    }

    public void SellGreen(bool all)
    {
        SellStone(3, all);
    }

    public void SellPurple(bool all)
    {
        SellStone(4, all);
    }
    
    
    private void UpdateUI()
    {
        coinText.text = " X " + coinCount;
        
        for (var i = 0; i < stoneTexts.Count; i++)
        {
            stoneTexts[i].text = " X " + stones[i];
        }
    }
    
    private void Start()
    {
        UpdateUI();
    }
}
