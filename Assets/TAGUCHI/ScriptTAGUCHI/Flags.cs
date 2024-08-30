using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{

    //���������Ă��邩
    private bool _ishaveFlag = true;
    [SerializeField, Header("���̐ݒu�\�N�[���^�C��")]
    private float _flagGetCoolTime = default;
    //�����ݒu�\�ɂȂ�܂ł̌v������
    private float _flagGetCount = default;
    //�ڂ̑O�ɉ������邩����
    private Ray _ray = default;
    private RaycastHit _hit = default;
    void Start()
    {
        _ray = new Ray(transform.position,transform.forward);
        _flagGetCount = 999;
    }
    /// <summary>
    /// ���̐ݒu�����i�v���C���[���ŌĂяo���j
    /// </summary>
    private void PutAndGetFlag()
    {
        //�����ݒu�\�ɂȂ��Ă��Ȃ�
        if(_ishaveFlag&&_flagGetCoolTime>_flagGetCount)
        {
            return;
        }
        //�ڂ̑O�ɉ�������
        if(Physics.Raycast(_ray,out _hit,1))
        {
            if(_hit.transform.tag =="Block")
            {
                Block block = _hit.transform.GetComponent<Block>();
                bool befor = _ishaveFlag;
                //���������������Ă��Ȃ��Ƃ�
                if(!_ishaveFlag&&block.ImBreakWall())
                {
                    _ishaveFlag = block.Flag(_ishaveFlag);
                }
                //���������������Ă���Ƃ�
                else if(_flagGetCoolTime<=_flagGetCount&&block.ImBreakWall())
                {
                    _ishaveFlag = block.Flag(_ishaveFlag);
                }
                //�����������
                if (befor != _ishaveFlag && _ishaveFlag == true)
                {
                    //�Đݒu�\�܂ł̌v�����Ԃ�������
                    _flagGetCount = 0;
                }
            }
        }
    }
}
