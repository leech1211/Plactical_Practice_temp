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

        // Value 값에 변화가 있을 때마다 이벤트 리스너를 호출합니다.
        scrollbar.onValueChanged.AddListener((float value) => OnScrollbarValueChanged(value));
    }

    void OnScrollbarValueChanged(float value)
    {

        //스크롤 방향 반대로
        value = 1f - value;
        //Debug.Log("Scrollbar Value: " + value);

        //float x = Mathf.Clamp(value, -2.5f,19);
        //float y = Mathf.Clamp(value, 9.5f, 12);
        //float z = Mathf.Clamp(value, 0, 0);

        // x의 범위를 -2.5에서 29로 조정
        float x = Mathf.Lerp(-2.5f, 33f, value);

        // y의 범위를 9.5에서 14로 조정
        float y = Mathf.Lerp(9.5f, 15f, value);

        // 조정된 x와 y 값을 사용하여 카메라의 위치 조정
        Vector3 newPosition = new Vector3(x, y, cam.transform.position.z);
        cam.transform.position = newPosition;

    }
}
