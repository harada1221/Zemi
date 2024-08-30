using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //旗を持っているかどうか
    private bool _isHaveFlag = default;
    [SerializeField,Header("デバッグ用：壊せる壁かどうか")]
    private bool _isBreakWall = default;
    //壁に地雷が埋まっているかどうか
    private bool _isMine = default;
    public bool IsHaveFlag {  get { return _isHaveFlag; } }
    public bool IsBreakWall {  get { return _isBreakWall; } }
    public bool IsMine {  get { return _isMine; } }
    /// <summary>
    /// 爆破の影響を受けた
    /// </summary>
    public void WallBreak()
    {
        //地雷が埋まっている時
        if(_isMine)
        {

        }
        Destroy(gameObject);
    }
    /// <summary>
    /// 旗をもらった、回収された
    /// </summary>
    public bool Flag(bool PlayerHave)
    {
        //どちらも旗を持っていないとき
        if(!_isHaveFlag&&!PlayerHave)
        {
            return false;
        }
        //自分が旗を持っているとき
        else if(_isHaveFlag)
        {
            _isHaveFlag = false;
            return true;
        }
        //プレイヤーが旗を持っているとき
        _isHaveFlag = true;
        return false;
    }
    /// <summary>
    /// 自分が壊れる壁の時にtrueを返す
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
