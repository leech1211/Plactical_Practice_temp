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
    public int peiz3;         //2페이즈 시작 지점

    public int money;         //유저의 돈


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

    public bool Stage_Fail;                     //스테이지를 실패 했는가 -> 애니메이션의 중복을 방


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

        //Debug.Log("페이즈 1");
        MaxNumNpc = 30;         //해당 스테이지 NPC
        SpawnedNPC = 0;         //초기 값 세팅
        PrintNum = MaxNumNpc;   //초기 값 세팅

        Min_Spawn_Timing = 4;       //초기 최소 스폰 주기는 4초
        Max_Spawn_Timing = 10;      //초기에 최대 스폰 주기는 10초
        

        peiz3 = MaxNumNpc / 3 + MaxNumNpc / 3;
        peiz2 = MaxNumNpc / 3;


        money = 100;

        BoomSpeed = 5;
        BoomHealth = 8;
        BoomDamage = 10;
        BoomTime = 3;
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
        /*페이즈는 총 3단계가 있음
          2페이즈는 총 NPC 개수의 1/3 이 spawn 되었을 때
          3페이즈는 총 NPC 개수의 2/3 이 spawn 되었을 때*/

        //게임 시작을 누르면 StartUI가 지나가도록


        if (Stage_health <= 0)
            Stage_Fail = true;              //남은 체력이 0이면 스테이지 실패

        if (SpawnedNPC >= peiz3 && Stage_peiz3 == false)
        {
            //Debug.Log("페이즈 3");
            //스폰속도 2~4
            //적 damage, health +1
            //Boom 체력 +1
            Stage_peiz3 = true;
            Min_Spawn_Timing = 2;
            Max_Spawn_Timing = 4;
            Enemy_Poison_Bomb_health += 1;
            Enemy_Poison_Bomb_damage += 1;
            BoomHealth += 1;            
        }
        else if (SpawnedNPC >= peiz2 && Stage_peiz2 == false)
        {
            //Debug.Log("페이즈 2");
            //스폰속도 4~8
            //적 speed+1
            //Boom 체력 +2
            Stage_peiz2 = true;
            Min_Spawn_Timing = 4;
            Max_Spawn_Timing = 8;
            Enemy_Poison_Bomb_speed += 1;
            BoomHealth += 2;            
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log(hit.transform.gameObject);
            }
        }
    }

}