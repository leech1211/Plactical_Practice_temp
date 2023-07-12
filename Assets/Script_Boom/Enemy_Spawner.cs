using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public Transform spawnerPoints;
    public GameObject enemyPrefabs;
    public bool generate;
    public float spawnTime;

    public int StageNPCMaxnum;              //현재 스테이지의 총 NPC 개수
    public int StageNPCSpawnednum;          //현재 스테이지의 생성한 NPC 개수

    public float Spawn_Min_Cycle;             //스폰주기 최소
    public float Spawn_Max_Cycle;             //스폰주기 최대 

    bool stage_start;
    bool update_control;                    //stage_start가 true가 된 이후 계속해서 spawn을 실행하지 않기위함

    public AudioSource SFX;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("Init", 1);  //Stage_system 이 초기화 된 이후 값을 가져와야 함
        stage_start = false;
        update_control = true;
    }
    void Init()
    {
        //스폰 할지 말지에 대한 Bool
        generate = false;

        StageNPCSpawnednum = GameObject.Find("Stage_System").GetComponent<Stage_System>().SpawnedNPC;

        Spawn_Min_Cycle = GameObject.Find("Stage_System").GetComponent<Stage_System>().Min_Spawn_Timing;        //스폰주기 최소
        Spawn_Max_Cycle = GameObject.Find("Stage_System").GetComponent<Stage_System>().Max_Spawn_Timing;        //스폰주기 최대
        StageNPCMaxnum = GameObject.Find("Stage_System").GetComponent<Stage_System>().MaxNumNpc;

        //스폰주기는 Stage_System 내부의 변수를 따름
        spawnTime = Random.Range(Spawn_Min_Cycle, Spawn_Max_Cycle);
        //스폰 함수
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        StageNPCSpawnednum = GameObject.Find("Stage_System").GetComponent<Stage_System>().SpawnedNPC;
        Spawn_Min_Cycle = GameObject.Find("Stage_System").GetComponent<Stage_System>().Min_Spawn_Timing;
        Spawn_Max_Cycle = GameObject.Find("Stage_System").GetComponent<Stage_System>().Max_Spawn_Timing;
        stage_start = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start;

        if(stage_start == true && update_control == true)             //user가 게임시작을 처음 누르면
        {
            update_control = false;             //해당 조건문에 다시 들어오지 않게 하기 위함
            Init();
        }
    }

    void spawn()
    {
        //이미 전부 생성했다면 그만
        if(StageNPCSpawnednum >= StageNPCMaxnum)
        {
            
        }
        //generate가 true이면 스폰, false면 true로 변경 후 다시 시간
        else if (generate == true)
        {
            Instantiate(enemyPrefabs, spawnerPoints.position, transform.rotation);
            SFX.Play();                                                                      //스폰 시 효과음 
            GameObject.Find("Stage_System").GetComponent<Stage_System>().SpawnedNPC++;       //스폰할 때 마다 스폰한 NPC++
            generate = false;
        }
        else if (generate == false)
        {
            generate = true;
        }

        //매번 랜덤한 시간으로 동작
        //스폰주기는 Stage_System 내부의 변수를 따름
        spawnTime = Random.Range(Spawn_Min_Cycle, Spawn_Max_Cycle);
        Invoke("spawn", spawnTime);
    }
}
