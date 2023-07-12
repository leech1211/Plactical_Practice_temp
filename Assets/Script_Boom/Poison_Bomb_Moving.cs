using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Bomb_Moving : MonoBehaviour
{
    public float speed;                     //오브젝트의 움직임 속력
    public int health;                      //오브젝트의 체력
    public int damage;                      //오브젝트의 공격력

    bool dieControl;                        //피가 다 닳았을때 Update 제어 
    bool hitHealtBox;                       //update 함수 제어
    bool rewardControl;             

    Rigidbody rigid;                        //오브젝트의 현재 위치
    BoxCollider box;                        //오브젝트의 Box Colider
    Vector3 moveVec;
    Animator Anim;

    public AudioSource AttackSFX;
    public AudioSource CrushHealthBoxSFX;

    // Start is called before the first frame update
    void Start()
    {
        speed = GameObject.Find("Stage_System").GetComponent<Stage_System>().Enemy_Poison_Bomb_speed;
        health = GameObject.Find("Stage_System").GetComponent<Stage_System>().Enemy_Poison_Bomb_health;
        damage = GameObject.Find("Stage_System").GetComponent<Stage_System>().Enemy_Poison_Bomb_damage;
        Anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        dieControl = true;
        hitHealtBox = false;
        rewardControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitHealtBox == false && health >= 0)            //health Box 에 부딪히지 않았다면
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f) && hit.collider.gameObject.layer == 17)
            {
                // 앞에 감지된 오브젝트의 레이어가 17인 경우
                speed = 0;
                Anim.SetBool("walk", false);            //걷는거 멈추고
                Anim.SetBool("attack", true);           //공격 애니메이션
            }
            else
            {
                if (dieControl == false)      //죽은 경우에는 움직이지 않는다 
                {
                    speed = 0;
                }
                else                        //죽지 않은경우에만 움직인다
                {
                    speed = GameObject.Find("Stage_System").GetComponent<Stage_System>().Enemy_Poison_Bomb_speed;
                }
                // 앞에 오브젝트가 감지되지 않음 -> 계속 앞으로 이동
                Anim.SetBool("attack", false);           //공격 애니메이션 그만하기
                moveVec = new Vector3(speed, 0, 0).normalized;
                //normalize의 경우는 앞뒤좌우의 speed가 1일때 대각선은 루트2 가 되는 경우가 존재해서 normalized를 붙여 방향값을 1로 보정

                transform.position += moveVec * speed * Time.deltaTime;
                //Time.deltaTime을 꼭 붙여주자

                Anim.SetBool("walk", moveVec != Vector3.zero);
                //걸을때는 애니메이션

                transform.LookAt(transform.position + moveVec);
                //움직이는 방향으로 바라보기            
            }
        }
        else if(this.health <= 0)     //피가 다 닳면
        {
            speed = 0;                          //speed = 0
            Anim.SetBool("die", true);          //죽은 애니메이션
            rigid.constraints &= ~RigidbodyConstraints.FreezePosition;      // RigidBody-Constraints-Freeze Position false로 -> 가라앉음
                                                                            //콜라이터의 센터 낮추기
            Vector3 center = box.center;
            center.y -= 0.4f;
            box.center = center;

            if(rewardControl == false)              //돈을 무진장 많이 받는 버그 방지
            {
                //Boom 의 가격의 두배를 보상
                GameObject.Find("Stage_System").GetComponent<Stage_System>().money += (GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomPrice * 2);
                rewardControl = true;
                Invoke("remove", 2);
            }                                                                                                                                                                                  

        }
       

    }
    void remove()
    {
        GameObject.Find("Stage_System").GetComponent<Stage_System>().PrintNum--;        //출력하는 NPC수 감소
        Destroy(gameObject);            //삭제
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position , transform.forward * 2f);
    }

    //Boom을 만났을 때 공격
    //애니메이션 이벤트를 통해 호출
    void Attack()
    {
        float raycastDistance = 2f; // 레이캐스트의 거리

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, raycastDistance);
        // 오브젝트에서 쭉 나오는 직선 레이캐스트 수행

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.layer == 27)                    //Boom을 만난경우
            {
                GameObject hitObject = hit.collider.gameObject;
                hitObject.GetComponent<UserNPC_Boom>().health = hitObject.GetComponent<UserNPC_Boom>().health - damage;
                // 데미지 적용
                AttackSFX.Play(); //효과음
            }
            if (health <= 0)        //죽어야 하는데 앞에 계속 Boom이 있어서 반복문을 계속 반복함?
                break;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 28)        //healthNode 와 충돌할 경우
        {
            hitHealtBox = true;                      //update 멈추기
            Anim.SetBool("walk", false);                //걷는거 멈추고
            Anim.SetBool("Explode", true);              //죽는 애니메이션
            GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health -= damage;        //스테이지의 생명력이 깍임
            //Debug.Log("스테이지 체력" + GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health);
            CrushHealthBoxSFX.Play();
            speed = 0;
            Invoke("remove", 1);
        }
    }


}