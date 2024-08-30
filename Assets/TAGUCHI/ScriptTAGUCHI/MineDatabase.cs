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
    [SerializeField,Header("�n���̖��O")]
    private string _mineName;
    public enum ExploType
    {
        LINE,
        CROSS,
        SIRCLE,
    }
    [SerializeField,Header("���j�̃^�C�v")]
    private ExploType _explotype;
    public enum ExploAbility
    {
        NONE,//������ʂȂ�
        WALLBREAK,//�i���j�󂹂Ȃ��ǂ�j��
        DEATH,//(��)����
    }
    [SerializeField,Header("�������")]
    private ExploAbility _exploAbility;
    [SerializeField,Header("���j�̈З�")]
    private int _exploPower;
    [SerializeField,Header("���j�p������")]
    private float _exploTime;
    [SerializeField, Header("�Ֆʂɑ��݂ł��铯���n���̐�")]
    private int _sameMineMax;
   
    public ExploType Type {  get { return _explotype; } }
    public ExploAbility Ability {  get { return _exploAbility; } }
    public int ExploPower {  get { return _exploPower; } }
    public float ExploTime {  get { return _exploTime; } }
    public int SameMineMax { get { return _sameMineMax; } }
}


