using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Bomb_Moving : MonoBehaviour
{
    public float speed;                     //������Ʈ�� ������ �ӷ�
    public int health;                      //������Ʈ�� ü��
    public int damage;                      //������Ʈ�� ���ݷ�

    bool dieControl;                        //�ǰ� �� ������� Update ���� 
    bool hitHealtBox;                       //update �Լ� ����
    bool rewardControl;             

    Rigidbody rigid;                        //������Ʈ�� ���� ��ġ
    BoxCollider box;                        //������Ʈ�� Box Colider
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
        if(hitHealtBox == false && health >= 0)            //health Box �� �ε����� �ʾҴٸ�
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f) && hit.collider.gameObject.layer == 17)
            {
                // �տ� ������ ������Ʈ�� ���̾ 17�� ���
                speed = 0;
                Anim.SetBool("walk", false);            //�ȴ°� ���߰�
                Anim.SetBool("attack", true);           //���� �ִϸ��̼�
            }
            else
            {
                if (dieControl == false)      //���� ��쿡�� �������� �ʴ´� 
                {
                    speed = 0;
                }
                else                        //���� ������쿡�� �����δ�
                {
                    speed = GameObject.Find("Stage_System").GetComponent<Stage_System>().Enemy_Poison_Bomb_speed;
                }
                // �տ� ������Ʈ�� �������� ���� -> ��� ������ �̵�
                Anim.SetBool("attack", false);           //���� �ִϸ��̼� �׸��ϱ�
                moveVec = new Vector3(speed, 0, 0).normalized;
                //normalize�� ���� �յ��¿��� speed�� 1�϶� �밢���� ��Ʈ2 �� �Ǵ� ��찡 �����ؼ� normalized�� �ٿ� ���Ⱚ�� 1�� ����

                transform.position += moveVec * speed * Time.deltaTime;
                //Time.deltaTime�� �� �ٿ�����

                Anim.SetBool("walk", moveVec != Vector3.zero);
                //�������� �ִϸ��̼�

                transform.LookAt(transform.position + moveVec);
                //�����̴� �������� �ٶ󺸱�            
            }
        }
        else if(this.health <= 0)     //�ǰ� �� ���
        {
            speed = 0;                          //speed = 0
            Anim.SetBool("die", true);          //���� �ִϸ��̼�
            rigid.constraints &= ~RigidbodyConstraints.FreezePosition;      // RigidBody-Constraints-Freeze Position false�� -> �������
                                                                            //�ݶ������� ���� ���߱�
            Vector3 center = box.center;
            center.y -= 0.4f;
            box.center = center;

            if(rewardControl == false)              //���� ������ ���� �޴� ���� ����
            {
                //Boom �� ������ �ι踦 ����
                GameObject.Find("Stage_System").GetComponent<Stage_System>().money += (GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomPrice * 2);
                rewardControl = true;
                Invoke("remove", 2);
            }                                                                                                                                                                                  

        }
       

    }
    void remove()
    {
        GameObject.Find("Stage_System").GetComponent<Stage_System>().PrintNum--;        //����ϴ� NPC�� ����
        Destroy(gameObject);            //����
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position , transform.forward * 2f);
    }

    //Boom�� ������ �� ����
    //�ִϸ��̼� �̺�Ʈ�� ���� ȣ��
    void Attack()
    {
        float raycastDistance = 2f; // ����ĳ��Ʈ�� �Ÿ�

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, raycastDistance);
        // ������Ʈ���� �� ������ ���� ����ĳ��Ʈ ����

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.layer == 27)                    //Boom�� �������
            {
                GameObject hitObject = hit.collider.gameObject;
                hitObject.GetComponent<UserNPC_Boom>().health = hitObject.GetComponent<UserNPC_Boom>().health - damage;
                // ������ ����
                AttackSFX.Play(); //ȿ����
            }
            if (health <= 0)        //�׾�� �ϴµ� �տ� ��� Boom�� �־ �ݺ����� ��� �ݺ���?
                break;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 28)        //healthNode �� �浹�� ���
        {
            hitHealtBox = true;                      //update ���߱�
            Anim.SetBool("walk", false);                //�ȴ°� ���߰�
            Anim.SetBool("Explode", true);              //�״� �ִϸ��̼�
            GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health -= damage;        //���������� ������� ����
            //Debug.Log("�������� ü��" + GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health);
            CrushHealthBoxSFX.Play();
            speed = 0;
            Invoke("remove", 1);
        }
    }


}