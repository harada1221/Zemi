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
    [SerializeField,Header("’n—‹‚Ì–¼‘O")]
    private string _mineName;
    public enum ExploType
    {
        LINE,
        CROSS,
        SIRCLE,
    }
    [SerializeField,Header("”š”j‚Ìƒ^ƒCƒv")]
    private ExploType _explotype;
    public enum ExploAbility
    {
        NONE,//“ÁêŒø‰Ê‚È‚µ
        WALLBREAK,//i‰¼j‰ó‚¹‚È‚¢•Ç‚ğ”j‰ó
        DEATH,//(‰¼)‘¦€
    }
    [SerializeField,Header("“ÁêŒø‰Ê")]
    private ExploAbility _exploAbility;
    [SerializeField,Header("”š”j‚ÌˆĞ—Í")]
    private int _exploPower;
    [SerializeField,Header("”š”jŒp‘±ŠÔ")]
    private float _exploTime;
    [SerializeField, Header("”Õ–Ê‚É‘¶İ‚Å‚«‚é“¯‚¶’n—‹‚Ì”")]
    private int _sameMineMax;
   
    public ExploType Type {  get { return _explotype; } }
    public ExploAbility Ability {  get { return _exploAbility; } }
    public int ExploPower {  get { return _exploPower; } }
    public float ExploTime {  get { return _exploTime; } }
    public int SameMineMax { get { return _sameMineMax; } }
}


