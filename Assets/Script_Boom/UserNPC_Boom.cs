using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNPC_Boom : MonoBehaviour
{
    public float speed;                     //오브젝트의 속력
    public int health;                      //오브젝트의 체력
    public int damage;                      //오브젝트의 데미지
    public int BoomTime;                    //Boom 이 몇 초 후 터질 것인가
    public int price;                       //오브젝트의 가격
    bool dieControl;                        //피가 다 닳았을때 Update 제어


    float hAxis;
    float vAxis;
    Rigidbody rigid;                        //오브젝트의 현재 위치

    Vector3 moveVec;
    BoxCollider box;
    Animator Anim;

    bool isAttack;

    public AudioSource Spawned;
    public AudioSource Boom;
    void Start()
    {
        speed = GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomSpeed;
        health = GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomHealth;
        damage = GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomDamage;
        BoomTime = GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomTime;
        price = GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomPrice;
        Debug.Log("공격력 " + damage);
        Debug.Log("체력 " + health);
        Debug.Log("폭발시간 " + BoomTime);

        Anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();

        dieControl = true;
        Spawned.Play();
        Invoke("attack", BoomTime);             //BoomTime 이 후 공격
    }

    // Update is called once per frame
    void Update()
    {
        //hAxis = Input.GetAxisRaw("Horizontal");
        // vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(-speed, 0, 0).normalized;
        //normalize의 경우는 앞뒤좌우의 speed가 1일때 대각선은 루트2 가 되는 경우가 존재해서 normalized를 붙여 방향값을 1로 보정

        transform.position += moveVec * speed * Time.deltaTime;
        //Time.deltaTime을 꼭 붙여주자

        Anim.SetBool("walk", moveVec != Vector3.zero);
        //걸을때는 애니메이션

        transform.LookAt(transform.position + moveVec);
        //움직이는 방향으로 바라보기



        if (this.health <= 0 && dieControl == true && isAttack == false)                //피가 다 닳면
        {
            speed = 0;
            dieControl = false;
            CancelInvoke();                                        //모든 인보크 취소 -> 폭발 x
            Anim.SetBool("damaged", true);
            Invoke("Delete", 2f);
        }


    }

    //공격 함수
    void attack()
    {
        //움직임을 멈추고
        //터지는 애니메이션
        //폭발
        //애니메이션이 끝나면
        //Destroy
        isAttack = true;
        speed = 0;
        Anim.SetBool("goattack", true);
        Invoke("BoomSFX", 0.8f);
        Invoke("explore", 1.5f);        //폭발이후 모션과 타이밍을 맞게하기 위함
        Invoke("Delete", 2.5f);
        
    }

    void BoomSFX()
    {
        Boom.Play();
    }

    void explore()
    {
        /*// 콜라이더를 담을 수 있는 배열을 만든다.
        GameObject[] shooted;

        // OverlapSphere를 통해 반경 내에 있는 콜라이더들을 가져온다
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

        // 배열 크기를 콜라이더 개수만큼 설정한다
        shooted = new GameObject[colliders.Length];

        // 각 콜라이더에 대해 GameObject를 배열에 담는다
        for (int i = 0; i < colliders.Length; i++)
        {
            shooted[i] = colliders[i].gameObject;
        }

        // 반경 내부에 아무것도 없었다면 그냥 반환
        if (shooted.Length == 0)
        {
            //Debug.Log("아무것도 없음");
            return;
        }*/

        // 자기 자신을 중심으로 주변에 있는 모든 콜라이더들을 가져옴
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);



        // foreach문을 통해서 colls배열에 존재하는 각각에 폭발효과를 적용해준다.
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == 26)
            {
                collider.GetComponent<Poison_Bomb_Moving>().health -= damage;     //피격된 적 데미지 입기
                //Debug.Log(collider.GetComponent<Poison_Bomb_Moving>().health);
            }
        }
    }

    void Delete()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    { 
        if(collision.gameObject.layer == 26)        //EnemyAttecked 와 충돌할 경우
        {
            Anim.SetBool("walk", false);            //걷는거 멈추고
            //Anim.SetBool("attack", true);           //공격 애니메이션
            speed = 0;
        }
    }
}
