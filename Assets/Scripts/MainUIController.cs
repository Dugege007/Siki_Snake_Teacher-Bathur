using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public bool hasBorder = true;
    public int score = 0;
    public int length = 1;
    public bool isPause = false;
    public string level = "阶段一";
    public Text msgText;
    public Text scoreText;
    public Text lengthText;
    public Image pauseImage;
    public Sprite[] pauseSprites;//图片组
    public Button homeButton;
    public Image bgImage;
    private Color tempColor;

    //单例模式
    private static MainUIController _instance;
    public static MainUIController Instance
    {
        get { return _instance; }
    }

    void Awake()//程序唤醒时执行
    {
        _instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("border", 1) == 0)
        {
            hasBorder = false;
            foreach(Transform t in bgImage.gameObject.transform)
            {
                t.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }

    void Update()
    {
        switch (score / 100)
        {
            case 0:
            case 1:
            case 2:
                break;
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段二";
                level = msgText.text;

                //bgImage.color = new Color();
                break;
            case 5:
            case 6:
            case 7:
                ColorUtility.TryParseHtmlString("#CCFFBDFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段三";
                level = msgText.text;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段四";
                level = msgText.text;
                break;
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段五";
                level = msgText.text;
                break;
            default:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "无尽阶段";
                level = msgText.text;
                break;
        }
    }

    public void UpdateUI(int s = 10, int l = 1)
    {
        score += s;
        length += l;
        scoreText.text = "得 分\n" + score;
        lengthText.text = "长 度\n" + length;
    }

    public void Pause()//暂停按钮
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;
            pauseImage.sprite = pauseSprites[0];
        }
    }

    public void Home()//主页按钮
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
