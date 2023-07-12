using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CodingButton : MonoBehaviour
{
    public void CodingButtonClicked()
    {
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
    }
}
