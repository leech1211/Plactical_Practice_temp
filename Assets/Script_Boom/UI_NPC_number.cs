using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPC_number : MonoBehaviour
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
        number = GameObject.Find("Stage_System").GetComponent<Stage_System>().PrintNum;
        PrintText.text = number.ToString();
    }
}
