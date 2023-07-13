using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CodingButton : MonoBehaviour
{
    public static CodingButton instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject leftEmemy;        //남은 적 UI
    public GameObject userMoney;        //남은 돈 UI
    public GameObject userHealth;       //남은 체력 UI
    public GameObject startBtn;         //게임시작 UI
    public GameObject codingBtn;        //코딩 UI
    public GameObject MainCamera;       //메인 카메라
    public GameObject Canvas;           //캔버스
    public void CodingButtonClicked()
    {
        readyToLoadBluePrint();
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);    //씬이동
        
    }

    private void readyToLoadBluePrint()     //청사진 넘어갈때 UI들 가리기
    {
        leftEmemy.SetActive(false);                                     //UI들 가리기
        userMoney.SetActive(false);
        userHealth.SetActive(false);
        startBtn.SetActive(false);
        codingBtn.SetActive(false);
        MainCamera.SetActive(false);                                    //메인 카메라
        Canvas.SetActive(false);                                        //캔버스
        //Time.timeScale = 0;
    }

    public void readyToReturnScene()        //청사진에서 돌아올 때 UI들 보이기
    {
        leftEmemy.SetActive(true);                                     //UI들 보이기
        userMoney.SetActive(true);
        userHealth.SetActive(true);
        startBtn.SetActive(true);
        codingBtn.SetActive(true);
        MainCamera.SetActive(true);                                    //메인 카메라
        Canvas.SetActive(true);                                        //캔버스
        //Time.timeScale = 1;
    }
    
    
}
