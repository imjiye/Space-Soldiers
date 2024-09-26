using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CoinManager>();
            }
            return m_instance;
        }
    }

    private static CoinManager m_instance;

    public Text txt_money;
    public int money;

    private void Awake()
    {
        //money = PlayerPrefs.GetInt("Mycoin");
        money = GameManager.instance.Get_MyCoin();
        UpdateMoney(true, 0);
    }

    public bool UpdateMoney(bool up, int value)
    {
        if (up)
        {
            money += value;
            SaveMoney();
            UpdateUi();
            return true;
        }
        else
        {
            if (money >= value)
            {
                money -= value;
                SaveMoney();
                UpdateUi();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Mycoin", money);
    }

    public void UpdateUi()
    {
        txt_money.text = " " + money;
    }
}