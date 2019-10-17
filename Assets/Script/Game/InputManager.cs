using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager{

    public bool bFront;
    public bool bBack;
    public bool bRight;
    public bool bLeft;
    public bool bSpace;
    public bool bShift;

    public float inputV = 0;
    public float inputH = 0;
    public float inputS = 0;

    public float InputThreshold = 0.01f;

    public void InputKey(bool isPlayer = false)
    {
        //判斷是否為玩家
        bFront = isPlayer ? Input.GetKey(KeyCode.W) : bFront;
        bBack = isPlayer ? Input.GetKey(KeyCode.S) : bBack;

        bRight = isPlayer ? Input.GetKey(KeyCode.D) : bRight;
        bLeft = isPlayer ? Input.GetKey(KeyCode.A) : bLeft;

        bSpace = isPlayer ? Input.GetKey(KeyCode.Space) : bSpace;
        bShift = isPlayer ? Input.GetKeyDown(KeyCode.LeftShift) : bShift;

        //前後 1 to -1
        if (bFront)
        {
            inputV = Mathf.MoveTowards(inputV, 1, 0.05f);
        }
        else if(bBack)
        {
            inputV = Mathf.MoveTowards(inputV, -1, 0.05f);
        }
        else
        {
            inputV = Mathf.MoveTowards(inputV, 0, 0.05f);
        }

        //右左 1 to -1
        if(bRight)
        {
            inputH = Mathf.MoveTowards(inputH, 1, 0.1f);
        }
        else if(bLeft)
        {
            inputH = Mathf.MoveTowards(inputH, -1, 0.1f);
        }
        else
        {
            inputH = Mathf.MoveTowards(inputH, 0, 0.1f);
        }


        inputS = bSpace ? Mathf.MoveTowards(inputS, 1, 0.25f) : Mathf.MoveTowards(inputS, 0, 0.25f);
        

        //限制值域
        inputV = Mathf.Clamp(inputV, -1, 1);
        inputH = Mathf.Clamp(inputH, -1, 1);
        inputS = Mathf.Clamp(inputS, 0, 1);
    }
}
