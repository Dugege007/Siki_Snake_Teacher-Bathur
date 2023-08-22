using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour
{
    public int xlimit = 20;
    public int ylimit = 11;
    public int xoffset = 7;

    public GameObject foodPrefab;
    public GameObject rewardPrefab;
    public Sprite[] foodSprites;
    private Transform foodsHolder;

    //单例模式
    private static FoodMaker _instance;
    public static FoodMaker Instance
    {
        get { return _instance; }
    }

    void Awake()//程序唤醒时执行
    {
        _instance = this;
    }

    void Start()
    {
        foodsHolder = GameObject.Find("FoodsHolder").transform;
        MakeFood(false);
    }

    public void MakeFood(bool isReward)
    {
        int index = Random.Range(0, foodSprites.Length);
        GameObject food = Instantiate(foodPrefab);//生成食物
        food.GetComponent<Image>().sprite = foodSprites[index];//给食物附上图片
        food.transform.SetParent(foodsHolder, false);
        bool isOK = false;
        for (int i = 0; i < xlimit * 2 - xoffset; i++)//如何使新生成的食物不落在蛇身上？
        {
            for(int j = 0; j < ylimit * 2; )
            {
                int x = Random.Range(-13, xlimit);
                int y = Random.Range(-ylimit, ylimit);

                if (true)
                {
                    food.transform.localPosition = new Vector3(x * 30, y * 30, 0);
                    isOK = true;
                    break;
                }
            }
            if (isOK == true)
                break;
        }

        if (isReward == true)
        {
            isOK = false;
            GameObject reward = Instantiate(rewardPrefab);//生成奖励
            reward.transform.SetParent(foodsHolder, false);
            for (int i = 0; i < xlimit * 2 - xoffset; i++)//如何使新生成的食物不落在蛇身上？
            {
                for (int j = 0; j < ylimit * 2;)
                {
                    int x = Random.Range(-13, xlimit);
                    int y = Random.Range(-ylimit, ylimit);

                    if (true)
                    {
                        reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
                        isOK = true;
                        break;
                    }
                }
                if (isOK == true)
                    break;
            }

        }
    }
}
