using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickUI : MonoBehaviour
{
    [SerializeField] private Image TL;
    [SerializeField] private Image TR;
    [SerializeField] private Image BL;
    [SerializeField] private Image BR;

    public virtual void MoveIndicator(Vector2 newInput)
    {
        Vector2 input = newInput;
        if (input.magnitude < 0.05f)
        {
            TL.enabled = false;
            TR.enabled = false;
            BL.enabled = false;
            BR.enabled = false;
        }
        else if (input.x >= 0f && input.y >= 0f)
        {
            TL.enabled = false;
            TR.enabled = true;
            BL.enabled = false;
            BR.enabled = false;
        }
        else if (input.x < 0f && input.y >= 0f)
        {
            TL.enabled = true;
            TR.enabled = false;
            BL.enabled = false;
            BR.enabled = false;
        }
        else if (input.x >= 0f && input.y < 0f)
        {
            TL.enabled = false;
            TR.enabled = false;
            BL.enabled = false;
            BR.enabled = true;
        }
        else if (input.x < 0f && input.y < 0f)
        {
            TL.enabled = false;
            TR.enabled = false;
            BL.enabled = true;
            BR.enabled = false;
        }
    }
}
