using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{

    //旗を持っているか
    private bool _ishaveFlag = true;
    [SerializeField, Header("旗の設置可能クールタイム")]
    private float _flagGetCoolTime = default;
    //旗が設置可能になるまでの計測時間
    private float _flagGetCount = default;
    //目の前に何かあるか判定
    private Ray _ray = default;
    private RaycastHit _hit = default;
    void Start()
    {
        _ray = new Ray(transform.position,transform.forward);
        _flagGetCount = 999;
    }
    /// <summary>
    /// 旗の設置処理（プレイヤー側で呼び出し）
    /// </summary>
    private void PutAndGetFlag()
    {
        //旗が設置可能になっていない
        if(_ishaveFlag&&_flagGetCoolTime>_flagGetCount)
        {
            return;
        }
        //目の前に何かある
        if(Physics.Raycast(_ray,out _hit,1))
        {
            if(_hit.transform.tag =="Block")
            {
                Block block = _hit.transform.GetComponent<Block>();
                bool befor = _ishaveFlag;
                //自分が旗を持っていないとき
                if(!_ishaveFlag&&block.ImBreakWall())
                {
                    _ishaveFlag = block.Flag(_ishaveFlag);
                }
                //自分が旗を持っているとき
                else if(_flagGetCoolTime<=_flagGetCount&&block.ImBreakWall())
                {
                    _ishaveFlag = block.Flag(_ishaveFlag);
                }
                //旗を回収した
                if (befor != _ishaveFlag && _ishaveFlag == true)
                {
                    //再設置可能までの計測時間を初期化
                    _flagGetCount = 0;
                }
            }
        }
    }
}
