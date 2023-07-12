using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Moving : MonoBehaviour
{
    public Scrollbar scrollbar;
    Camera cam;
    Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        // Value ���� ��ȭ�� ���� ������ �̺�Ʈ �����ʸ� ȣ���մϴ�.
        scrollbar.onValueChanged.AddListener((float value) => OnScrollbarValueChanged(value));
    }

    void OnScrollbarValueChanged(float value)
    {

        //��ũ�� ���� �ݴ��
        value = 1f - value;
        //Debug.Log("Scrollbar Value: " + value);

        //float x = Mathf.Clamp(value, -2.5f,19);
        //float y = Mathf.Clamp(value, 9.5f, 12);
        //float z = Mathf.Clamp(value, 0, 0);

        // x�� ������ -2.5���� 29�� ����
        float x = Mathf.Lerp(-2.5f, 33f, value);

        // y�� ������ 9.5���� 14�� ����
        float y = Mathf.Lerp(9.5f, 15f, value);

        // ������ x�� y ���� ����Ͽ� ī�޶��� ��ġ ����
        Vector3 newPosition = new Vector3(x, y, cam.transform.position.z);
        cam.transform.position = newPosition;

    }
}
