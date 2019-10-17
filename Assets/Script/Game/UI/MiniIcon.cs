using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniIcon : MonoBehaviour
{
    void Update()
    {
        this.transform.eulerAngles = new Vector3(90, 180, 0);
    }
}
