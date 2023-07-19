using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNPC_Boom : MonoBehaviour
{
    public float speed;                     //������Ʈ�� �ӷ�
    public int health;                      //������Ʈ�� ü��
    public int damage;                      //������Ʈ�� ������
    public int BoomTime;                    //Boom �� �� �� �� ���� ���ΰ�
    public int price;                       //������Ʈ�� ����
    bool dieControl;                        //�ǰ� �� ������� Update ����


    float hAxis;
    float vAxis;
    Rigidbody rigid;                        //������Ʈ�� ���� ��ġ

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
        Debug.Log("���ݷ� " + damage);
        Debug.Log("ü�� " + health);
        Debug.Log("���߽ð� " + BoomTime);

        Anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();

        dieControl = true;
        Spawned.Play();
        Invoke("attack", BoomTime);             //BoomTime �� �� ����
    }

    // Update is called once per frame
    void Update()
    {
        //hAxis = Input.GetAxisRaw("Horizontal");
        // vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(-speed, 0, 0).normalized;
        //normalize�� ���� �յ��¿��� speed�� 1�϶� �밢���� ��Ʈ2 �� �Ǵ� ��찡 �����ؼ� normalized�� �ٿ� ���Ⱚ�� 1�� ����

        transform.position += moveVec * speed * Time.deltaTime;
        //Time.deltaTime�� �� �ٿ�����

        Anim.SetBool("walk", moveVec != Vector3.zero);
        //�������� �ִϸ��̼�

        transform.LookAt(transform.position + moveVec);
        //�����̴� �������� �ٶ󺸱�



        if (this.health <= 0 && dieControl == true && isAttack == false)                //�ǰ� �� ���
        {
            speed = 0;
            dieControl = false;
            CancelInvoke();                                        //��� �κ�ũ ��� -> ���� x
            Anim.SetBool("damaged", true);
            Invoke("Delete", 2f);
        }


    }

    //���� �Լ�
    void attack()
    {
        //�������� ���߰�
        //������ �ִϸ��̼�
        //����
        //�ִϸ��̼��� ������
        //Destroy
        isAttack = true;
        speed = 0;
        Anim.SetBool("goattack", true);
        Invoke("BoomSFX", 0.8f);
        Invoke("explore", 1.5f);        //�������� ��ǰ� Ÿ�̹��� �°��ϱ� ����
        Invoke("Delete", 2.5f);
        
    }

    void BoomSFX()
    {
        Boom.Play();
    }

    void explore()
    {
        /*// �ݶ��̴��� ���� �� �ִ� �迭�� �����.
        GameObject[] shooted;

        // OverlapSphere�� ���� �ݰ� ���� �ִ� �ݶ��̴����� �����´�
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);

        // �迭 ũ�⸦ �ݶ��̴� ������ŭ �����Ѵ�
        shooted = new GameObject[colliders.Length];

        // �� �ݶ��̴��� ���� GameObject�� �迭�� ��´�
        for (int i = 0; i < colliders.Length; i++)
        {
            shooted[i] = colliders[i].gameObject;
        }

        // �ݰ� ���ο� �ƹ��͵� �����ٸ� �׳� ��ȯ
        if (shooted.Length == 0)
        {
            //Debug.Log("�ƹ��͵� ����");
            return;
        }*/

        // �ڱ� �ڽ��� �߽����� �ֺ��� �ִ� ��� �ݶ��̴����� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);



        // foreach���� ���ؼ� colls�迭�� �����ϴ� ������ ����ȿ���� �������ش�.
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == 26)
            {
                collider.GetComponent<Poison_Bomb_Moving>().health -= damage;     //�ǰݵ� �� ������ �Ա�
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
        if(collision.gameObject.layer == 26)        //EnemyAttecked �� �浹�� ���
        {
            Anim.SetBool("walk", false);            //�ȴ°� ���߰�
            //Anim.SetBool("attack", true);           //���� �ִϸ��̼�
            speed = 0;
        }
    }
}
