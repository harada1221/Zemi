using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MineDatabase : ScriptableObject
{
    [SerializeField]
    private MineData[] _minedata = new MineData[0];
    public MineData GetMineData(int index)
    {
        return _minedata[index];
    }
    public MineData[] MinesData {  get { return _minedata; } }
}
[System.Serializable]
public struct MineData
{
    [SerializeField,Header("地雷の名前")]
    private string _mineName;
    public enum ExploType
    {
        LINE,
        CROSS,
        SIRCLE,
    }
    [SerializeField,Header("爆破のタイプ")]
    private ExploType _explotype;
    public enum ExploAbility
    {
        NONE,//特殊効果なし
        WALLBREAK,//（仮）壊せない壁を破壊
        DEATH,//(仮)即死
    }
    [SerializeField,Header("特殊効果")]
    private ExploAbility _exploAbility;
    [SerializeField,Header("爆破の威力")]
    private int _exploPower;
    [SerializeField,Header("爆破継続時間")]
    private float _exploTime;
    [SerializeField, Header("盤面に存在できる同じ地雷の数")]
    private int _sameMineMax;
   
    public ExploType Type {  get { return _explotype; } }
    public ExploAbility Ability {  get { return _exploAbility; } }
    public int ExploPower {  get { return _exploPower; } }
    public float ExploTime {  get { return _exploTime; } }
    public int SameMineMax { get { return _sameMineMax; } }
}


