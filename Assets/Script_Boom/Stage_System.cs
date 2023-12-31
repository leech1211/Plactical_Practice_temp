using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_System : MonoBehaviour
{
    
    /*SpawnedNPC가 0부터 시작해 스폰할때 마다 1씩 증가
      MaxNumNpc까지 증가 후 MaxNumNpc와 같아진다면 더이상 스폰되지 않음
      PrintNum는 초기 값이 MaxNumNpc 동일하며 NPC가 1개씩 죽을 때 마다 1 감소*/


    public int Stage_health;    //해당 스테이지의 체력


    public int MaxNumNpc;       //해당 스테이지의 총 NPC 개수
    public int SpawnedNPC;      //지금까지 스폰한 NPC개수
    public int PrintNum;        //UI에 출력할 NPC 숫자

    public float Min_Spawn_Timing;    //NPC 스폰 최소주기
    public float Max_Spawn_Timing;    //NPC 스폰 최대주기

    public int peiz2;         //2페이즈 시작 지점
    public int peiz3;         //3페이즈 시작 지점

    public int money;         //유저의 돈
    public float stage_time;  //스테이지 시작까지 시간


    public float BoomSpeed;                     //Boom오브젝트의 속력
    public int BoomHealth;                      //Boom오브젝트의 체력
    public int BoomDamage;                      //Boom오브젝트의 데미지
    public int BoomTime;                        //Boom 이 몇 초 후 터질 것인가
    public int BoomPrice;                       //Boom오브젝트의 가격

    public int Enemy_Poison_Bomb_speed;         //Poison Bomb의 속력
    public int Enemy_Poison_Bomb_health;        //Poison Bomb의 체력
    public int Enemy_Poison_Bomb_damage;        //Poison Bomb의 공격력

    public bool Stage_Start;                          //게임 시작 전 테스트 이 후 게임을 시작
    public bool Stage_peiz2, Stage_peiz3;             //해당 스테이지의 각 peiz여부  

    public bool Stage_Fail;                     //스테이지를 실패 했는가 -> 애니메이션의 중복을 방지

    public GameObject PeizBlink;

    //Build Manager


    public static Stage_System instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Stage_System in Scene!");
            return;
        }
        instance = this;
        stage_time = 120;       //stage_time 2분
    }

    public GameObject standardUserNPC;

    private GameObject UserNPCtoBuild;
    public GameObject GetUserNPCtoBuild()
    {
        return UserNPCtoBuild;
    }


    // Start is called before the first frame update
    void Start()
    {
        Stage_health = 10;      //해당 스테이지 체력

        //("페이즈 1");
        /*
         * BoomSpeed = 5
         * Boom체력 8
         * Boom 공격력 10
         * Boom 폭발시간 3
         *
         * Poison Bomb speed = 10
         * Poison Bomb 체력 5
         * Poison Bomb 공격력 1
         */
        MaxNumNpc = 30;         //해당 스테이지 NPC
        SpawnedNPC = 0;         //초기 값 세팅
        PrintNum = MaxNumNpc;   //초기 값 세팅

        Min_Spawn_Timing = 4;       //초기 최소 스폰 주기는 4초
        Max_Spawn_Timing = 10;      //초기에 최대 스폰 주기는 10초
        

        peiz2 = MaxNumNpc / 3 + MaxNumNpc / 3;              //20
        peiz3 = MaxNumNpc / 3;                              //10


        money = 100;
        
        
        // 1,2,3 각각 2개씩, 더하기 2개
        StartCoroutine(StartItemToInventory());
       
        

        BoomSpeed = 5;
        BoomHealth = 0;
        BoomDamage = 0;
        BoomTime = 0;
        BoomPrice = 5;

        Enemy_Poison_Bomb_speed = 10;
        Enemy_Poison_Bomb_health = 5;
        Enemy_Poison_Bomb_damage = 1;


        Stage_Start = false;
        Stage_peiz2 = false;
        Stage_peiz3 = false;

        Stage_Fail = false;

        UserNPCtoBuild = standardUserNPC;
    }

    // Update is called once per frame
    void Update()
    {
        stage_time -= Time.deltaTime;       //시간은 1초씩 줄어든다
        
        /*페이즈는 총 3단계가 있음
          2페이즈는 총 NPC 개수의 1/3 이 spawn 되었을 때
          3페이즈는 총 NPC 개수의 2/3 이 spawn 되었을 때*/

        //게임 시작을 누르면 StartUI가 지나가도록


        if (Stage_health <= 0)
            Stage_Fail = true;              //남은 체력이 0이면 스테이지 실패

        if (PrintNum <= peiz3 && Stage_peiz3 == false && Stage_Start == true)
        {
            //Debug.Log("페이즈 3");
            /*
         *
         * Poison Bomb speed = 11
         * Poison Bomb 체력 8
         * Poison Bomb 공격력 2
         */
            //스폰속도 2~4

            Stage_peiz3 = true;
            Min_Spawn_Timing = 2;
            Max_Spawn_Timing = 4;
            Enemy_Poison_Bomb_health += 1;
            Enemy_Poison_Bomb_damage += 1;

            //스포너의 스폰을 잠시 멈추고 다시 2분 대기 
            Stage_Start = false;
            stage_time = 120;
            PeizBlink.SetActive(true);
            
        }
        else if (PrintNum <= peiz2 && Stage_peiz2 == false && Stage_Start == true)
        {
            //Debug.Log("페이즈 2");
            /*
         *
         * Poison Bomb speed = 11
         * Poison Bomb 체력 7
         * Poison Bomb 공격력 1
         */
            //스폰속도 4~8
            Stage_peiz2 = true;
            Min_Spawn_Timing = 4;
            Max_Spawn_Timing = 8;
            Enemy_Poison_Bomb_speed += 1;

            //스포너의 스폰을 잠시 멈추고 다시 2분 대기 
            Stage_Start = false;
            stage_time = 120;
            PeizBlink.SetActive(true);
                
            ItemManager.instance.AddItem(NodeType.INT_2);
            
        }

        /*if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log(hit.transform.gameObject);
            }
        }*/
    }

    private IEnumerator StartItemToInventory()
    {
        //ItemManager.instance의 start()를 통해 items가 생성될때 까지 대기 후 아이템 소매넣기
        while (ItemManager.instance.getItems() == null)
        {
            yield return null;
        }
        ItemManager.instance.AddItem(NodeType.FLOAT_1);
        ItemManager.instance.AddItem(NodeType.FLOAT_1);
        ItemManager.instance.AddItem(NodeType.FLOAT_2);
        ItemManager.instance.AddItem(NodeType.FLOAT_2);
        ItemManager.instance.AddItem(NodeType.FLOAT_3);
        ItemManager.instance.AddItem(NodeType.FLOAT_3);
        ItemManager.instance.AddItem(NodeType.OPERATOR_PLUS);
        ItemManager.instance.AddItem(NodeType.OPERATOR_PLUS);
    }

}