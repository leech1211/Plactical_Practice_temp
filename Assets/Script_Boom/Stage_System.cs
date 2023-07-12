using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_System : MonoBehaviour
{
    /*SpawnedNPC�� 0���� ������ �����Ҷ� ���� 1�� ����
      MaxNumNpc���� ���� �� MaxNumNpc�� �������ٸ� ���̻� �������� ����
      PrintNum�� �ʱ� ���� MaxNumNpc �����ϸ� NPC�� 1���� ���� �� ���� 1 ����*/


    public int Stage_health;    //�ش� ���������� ü��


    public int MaxNumNpc;       //�ش� ���������� �� NPC ����
    public int SpawnedNPC;      //���ݱ��� ������ NPC����
    public int PrintNum;        //UI�� ����� NPC ����

    public float Min_Spawn_Timing;    //NPC ���� �ּ��ֱ�
    public float Max_Spawn_Timing;    //NPC ���� �ִ��ֱ�

    public int peiz2;         //2������ ���� ����
    public int peiz3;         //2������ ���� ����

    public int money;         //������ ��


    public float BoomSpeed;                     //Boom������Ʈ�� �ӷ�
    public int BoomHealth;                      //Boom������Ʈ�� ü��
    public int BoomDamage;                      //Boom������Ʈ�� ������
    public int BoomTime;                        //Boom �� �� �� �� ���� ���ΰ�
    public int BoomPrice;                       //Boom������Ʈ�� ����

    public int Enemy_Poison_Bomb_speed;         //Poison Bomb�� �ӷ�
    public int Enemy_Poison_Bomb_health;        //Poison Bomb�� ü��
    public int Enemy_Poison_Bomb_damage;        //Poison Bomb�� ���ݷ�

    public bool Stage_Start;                          //���� ���� �� �׽�Ʈ �� �� ������ ����
    public bool Stage_peiz2, Stage_peiz3;             //�ش� ���������� �� peiz����  

    public bool Stage_Fail;                     //���������� ���� �ߴ°� -> �ִϸ��̼��� �ߺ��� ��


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
        Stage_health = 10;      //�ش� �������� ü��

        //Debug.Log("������ 1");
        MaxNumNpc = 30;         //�ش� �������� NPC
        SpawnedNPC = 0;         //�ʱ� �� ����
        PrintNum = MaxNumNpc;   //�ʱ� �� ����

        Min_Spawn_Timing = 4;       //�ʱ� �ּ� ���� �ֱ�� 4��
        Max_Spawn_Timing = 10;      //�ʱ⿡ �ִ� ���� �ֱ�� 10��
        

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
        /*������� �� 3�ܰ谡 ����
          2������� �� NPC ������ 1/3 �� spawn �Ǿ��� ��
          3������� �� NPC ������ 2/3 �� spawn �Ǿ��� ��*/

        //���� ������ ������ StartUI�� ����������


        if (Stage_health <= 0)
            Stage_Fail = true;              //���� ü���� 0�̸� �������� ����

        if (SpawnedNPC >= peiz3 && Stage_peiz3 == false)
        {
            //Debug.Log("������ 3");
            //�����ӵ� 2~4
            //�� damage, health +1
            //Boom ü�� +1
            Stage_peiz3 = true;
            Min_Spawn_Timing = 2;
            Max_Spawn_Timing = 4;
            Enemy_Poison_Bomb_health += 1;
            Enemy_Poison_Bomb_damage += 1;
            BoomHealth += 1;            
        }
        else if (SpawnedNPC >= peiz2 && Stage_peiz2 == false)
        {
            //Debug.Log("������ 2");
            //�����ӵ� 4~8
            //�� speed+1
            //Boom ü�� +2
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