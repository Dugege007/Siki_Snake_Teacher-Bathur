using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour
{
    public List<Transform> bodyList = new List<Transform>();

    public float velocity = 0.40f;//速度
    public int step;//步长
    private Vector3 headPos;//头部位置
    private int x;
    private int y;
    private bool isDie = false;
    private Transform canvas;

    public AudioClip eatClip;
    public AudioClip dieClip;
    public GameObject dieEffect;
    public GameObject bodyPrefab;
    public Sprite[] bodySprites = new Sprite[2];


    void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        //通过Resources.Load(string path)方法加载资源，path的书写不需要加Resources/以及文件扩展名
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
    }

    void Start()
    {
        InvokeRepeating("Move", velocity, velocity);//重复调用移动（函数名、在多久后开始、间隔时间）
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);//初始方向
        x = step;
        y = 0;
        MainUIController.Instance.UpdateUI(0);
    }

    void Update()
    {
        SpeedUp();

        bool condition = MainUIController.Instance.isPause == false && isDie == false;

        if (Input.GetKeyDown(KeyCode.W) && y != -step && condition)//按W向上
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            x = 0; y = step;
            Turn();
            CancelInvoke();
            InvokeRepeating("Move", velocity, velocity * 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.W) && y != -step && condition)
        {
            CancelInvoke();
            InvokeRepeating("Move", velocity * 0.75f, velocity);
        }

        if (Input.GetKeyDown(KeyCode.S) && y != step && condition)//按S向下
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
            x = 0; y = -step;
            Turn();
            CancelInvoke();
            InvokeRepeating("Move", velocity, velocity * 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.S) && y != step && condition)
        {
            CancelInvoke();
            InvokeRepeating("Move", velocity * 0.75f, velocity);
        }

        if (Input.GetKeyDown(KeyCode.A) && x != step && condition)//按A向左
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            x = -step; y = 0;
            Turn();
            CancelInvoke();
            InvokeRepeating("Move", velocity, velocity * 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.A) && x != step && condition)
        {
            CancelInvoke();
            InvokeRepeating("Move", velocity * 0.75f, velocity);
        }

        if (Input.GetKeyDown(KeyCode.D) && x != -step && condition)//按D向右
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
            x = step; y = 0;
            Turn();
            CancelInvoke();
            InvokeRepeating("Move", velocity, velocity * 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.D) && x != -step && condition)
        {
            CancelInvoke();
            InvokeRepeating("Move", velocity * 0.75f, velocity);
        }

        if (MainUIController.Instance.level == "阶段二")
            velocity = 0.35f;
        else if (MainUIController.Instance.level == "阶段三")
            velocity = 0.30f;
        else if (MainUIController.Instance.level == "阶段四")
            velocity = 0.25f;
        else if (MainUIController.Instance.level == "无尽阶段")
            velocity = 0.20f;
    }

    void Move()//以上个位置为参照移动
    {
        headPos = gameObject.transform.localPosition; //保存下来蛇头移动前的位置
        gameObject.transform.localPosition = new Vector3(headPos.x + x, headPos.y + y, headPos.z);//蛇头向希望位置移动
        if (bodyList.Count > 0)
        {
            //由于我们是双色蛇身，此方法弃用
            //bodyList.Last().localPosition = headPos;//将蛇身尾部调到蛇头移动前的位置
            //lodyList.Insert(0, bodyList.Last());//将蛇尾在List中的位置更新到最前
            //bodyList.RemoveAt(bodyList.Count - 1);//移除List最末尾的蛇尾引用

            //由于我们是双色蛇身，使用此方法达到显示目的
            for (int i = bodyList.Count - 2 ; i >= 0 ; i--)//从后往前开始移动蛇身
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;//每一个蛇身都移动到它前面一个节点的位置
            }
            bodyList[0].localPosition = headPos;//第一个蛇身移动到蛇头移动前的位置
        }
    }

    void SpeedUp()//按空格加速
    {
        if (Input.GetKeyDown(KeyCode.Space) && MainUIController.Instance.isPause == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0.1f, velocity * 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && MainUIController.Instance.isPause == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", velocity, velocity);
        }
    }

    void Turn()//不要转弯太快撞到自己
    {
        CancelInvoke();
        Move();
        InvokeRepeating("Move", velocity, velocity);
    }

    void Grow()//变长
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        GameObject body = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprites[index];
        body.transform.SetParent(canvas, false);
        bodyList.Add(body.transform);
        Debug.Log("Grow");
    }

    void Die()//死亡
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        CancelInvoke();
        isDie = true;
        Instantiate(dieEffect);
        PlayerPrefs.SetInt("lastl", MainUIController.Instance.length);
        PlayerPrefs.SetInt("lasts", MainUIController.Instance.score);
        if (PlayerPrefs.GetInt("bests", 0) < MainUIController.Instance.score)
        {
            PlayerPrefs.SetInt("bestl", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bests", MainUIController.Instance.score);
        }
        StartCoroutine(GameOver(1.5f));
    }

    IEnumerator GameOver(float t)//协程
    {
        yield return new WaitForSeconds(t);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);//加载Main场景
    }

    private void OnTriggerEnter2D(Collider2D collision)//碰撞器检测
    {
        if (collision.tag == "Food")//也可写为 if(collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI();
            Debug.Log("Eat");
            Grow();
            FoodMaker.Instance.MakeFood((Random.Range(0, 100) < 20) ? true : false);//相当于
            //if (Random.Range(0, 100) < 20)
            //    FoodMaker.Instance.MakeFood(true);
            //else
            //    FoodMaker.Instance.MakeFood(false);
        }
        else if (collision.tag == "Reward")
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI(Random.Range(2, 10) * 10);
            Debug.Log("Eat");
            Grow();
        }
        else if(collision.tag == "Body")
        {
            Die();
            Debug.Log("Die");
        }
        else
        {
            if (MainUIController.Instance.hasBorder)
            {
                Die();
                Debug.Log("Die");
            }
            else
            {
                switch (collision.gameObject.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 210, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 270, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }
}
