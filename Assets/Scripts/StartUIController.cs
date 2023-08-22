using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour
{
    public Text lastText;
    public Text bestText;
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle noBorder;

    void Awake()
    {
        lastText.text = "上次：长度" + PlayerPrefs.GetInt("lastl", 1) + "；分数" + PlayerPrefs.GetInt("lasts",0);//读取注册表中的记录
        bestText.text = "最佳：长度" + PlayerPrefs.GetInt("bestl", 1) + "；分数" + PlayerPrefs.GetInt("bests",0);//读取注册表中的记录
    }

    void Start()
    {
        if (PlayerPrefs.GetString("sh", "sh01") == "sh01")//皮肤判断
        {
            blue.isOn = true;
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
        else
        {
            yellow.isOn = true;
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0202");
        }

        if (PlayerPrefs.GetInt("border", 1) == 1)//模式判断
        {
            border.isOn = true;
            PlayerPrefs.SetInt("border", 1);
        }
        else
        {
            noBorder.isOn = true;
            PlayerPrefs.SetInt("border", 0);
        }
    }

    public void BlueSelected(bool isOn)//选择蓝色皮肤
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
    }

    public void YellowSelected(bool isOn)//选择黄色皮肤
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
    }

    public void BorderSelected(bool isOn)//选择边界模式
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }
    }

    public void NoborderSelected(bool isOn)//选择无尽模式
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }

    public void StartGame()//按下开始游戏
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);//加载Main场景
    }
}
