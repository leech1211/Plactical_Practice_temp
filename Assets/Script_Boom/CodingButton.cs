using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CodingButton : MonoBehaviour
{
    public GameObject leftEmemy;        //남은 적 UI
    public GameObject userMoney;        //남은 돈 UI
    public GameObject userHealth;       //남은 체력 UI
    public GameObject startBtn;
    public GameObject codingBtn;
    public void CodingButtonClicked()
    {
        leftEmemy.SetActive(false);                                     //UI들 가리기
        userMoney.SetActive(false);
        userHealth.SetActive(false);
        startBtn.SetActive(false);
        codingBtn.SetActive(false);
        Time.timeScale = 0;
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);    //씬이동
        
    }
    
    
}
