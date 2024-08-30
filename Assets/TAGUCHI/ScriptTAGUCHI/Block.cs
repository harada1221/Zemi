using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //���������Ă��邩�ǂ���
    private bool _isHaveFlag = default;
    [SerializeField,Header("�f�o�b�O�p�F�󂹂�ǂ��ǂ���")]
    private bool _isBreakWall = default;
    //�ǂɒn�������܂��Ă��邩�ǂ���
    private bool _isMine = default;
    public bool IsHaveFlag {  get { return _isHaveFlag; } }
    public bool IsBreakWall {  get { return _isBreakWall; } }
    public bool IsMine {  get { return _isMine; } }
    /// <summary>
    /// ���j�̉e�����󂯂�
    /// </summary>
    public void WallBreak()
    {
        //�n�������܂��Ă��鎞
        if(_isMine)
        {

        }
        Destroy(gameObject);
    }
    /// <summary>
    /// ������������A������ꂽ
    /// </summary>
    public bool Flag(bool PlayerHave)
    {
        //�ǂ�������������Ă��Ȃ��Ƃ�
        if(!_isHaveFlag&&!PlayerHave)
        {
            return false;
        }
        //���������������Ă���Ƃ�
        else if(_isHaveFlag)
        {
            _isHaveFlag = false;
            return true;
        }
        //�v���C���[�����������Ă���Ƃ�
        _isHaveFlag = true;
        return false;
    }
    /// <summary>
    /// ����������ǂ̎���true��Ԃ�
    /// </summary>
    /// <returns></returns>
    public bool ImBreakWall()
    {
        if(_isBreakWall)
        {
            return true;
        }
        return false;
    }
}
