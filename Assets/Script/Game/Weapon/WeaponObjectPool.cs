using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectPool : MonoBehaviour {

    public GameObject[] WeaponPrefeb;
    int WeaponKind;
    public int[] initailSize;
    Dictionary<int, string> ObjectDict = new Dictionary<int, string>();

    Queue<GameObject> _pool = new Queue<GameObject>();
    //Queue<GameObject> _pool_Rocket = new Queue<GameObject>();
    //Queue<GameObject> _pool_Oil = new Queue<GameObject>();
    //Queue<GameObject> _pool_Shield = new Queue<GameObject>();
    //Queue<GameObject> _pool_TNT = new Queue<GameObject>();
    //Queue<GameObject> _pool_Bomb = new Queue<GameObject>();


    private void Awake()
    {
        //WeaponKind = 5;
        //for (int cnt = 0; cnt < initailSize[WeaponKind]; cnt++)
        //{
        //    GameObject go = Instantiate(WeaponPrefeb[WeaponKind]) as GameObject;
        //    _pool_TNT.Enqueue(go);
        //    go.SetActive(false);
        //}

        //for (int item = 0; item < WeaponPrefeb.Length; item++)
        //{
        //    ObjectDict.Add(item, WeaponPrefeb[item].name);
        //}

        //for (int num = 0; num <= 9; num++)
        //{
        //    WeaponKind = num;
        //    for (int cnt = 0; cnt < initailSize[WeaponKind]; cnt++)
        //    {
        //        GameObject go = Instantiate(WeaponPrefeb[WeaponKind]) as GameObject;
        //        _pool.Enqueue(go);
        //        go.SetActive(false);
        //    }

        //}


    }

    //void QueueSwitch()
    //{
    //    switch (WeaponKind)
    //    {
    //        case 1:
    //            break;
    //        case 2:
    //            _pool_Rocket = _pool;
    //            _pool.Clear();
    //            break;
    //        case 3:
    //            _pool_Oil = _pool;
    //            _pool.Clear();
    //            break;
    //        case 4:
    //            _pool_Shield = _pool;
    //            _pool.Clear();
    //            break;
    //        case 5:
    //            _pool_TNT = _pool;
    //            _pool.Clear();
    //            break;
    //        case 6:
    //            _pool_Bomb = _pool;
    //            _pool.Clear();
    //            break;
    //        case 7:
    //            break;
    //        case 8:
    //            break;
    //    }
    //}

    //取出
    public void ReUse(int ItemKind, Vector3 position,Quaternion rotation)
    {
        if (_pool.Count > 0)
        {
            GameObject reuse = _pool.Dequeue();
            reuse.transform.position = position;
            reuse.transform.rotation = rotation;
            reuse.SetActive(true);

        }
        else
        {
            GameObject go = Instantiate(WeaponPrefeb[WeaponKind]) as GameObject;
            go.transform.position = position;
            go.transform.rotation = rotation;

        }
    } 

    //收回
    public void Recovery(GameObject recovery)
    {
        _pool.Enqueue(recovery);
        recovery.SetActive(false);
    }
}
