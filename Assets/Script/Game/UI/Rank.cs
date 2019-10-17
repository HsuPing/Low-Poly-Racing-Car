using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct RankInformation
{
    public Sprite _Sprite; //武器圖片
}

[CreateAssetMenu()]
public class Rank : ScriptableObject
{
    [SerializeField]
    RankInformation[] _RankInformations = null;

    public RankInformation[] RankInformations
    {
        get
        {
            return _RankInformations;
        }
        set
        {
            _RankInformations = value;
        }
    }
}
