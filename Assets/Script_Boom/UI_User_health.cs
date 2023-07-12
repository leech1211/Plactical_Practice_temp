using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_User_health : MonoBehaviour
{
    public int number;
    Text PrintText;

    // Start is called before the first frame update
    void Start()
    {
        PrintText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        number = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health;
        PrintText.text = number.ToString();
    }
}
