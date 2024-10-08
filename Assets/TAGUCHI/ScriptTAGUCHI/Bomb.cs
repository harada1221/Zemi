using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    //爆破の種類
    private bool _isBombExplodeCross = default;
    [SerializeField,Header("爆弾の基礎威力")]
    private int _bombPower = default;
    [SerializeField, Header("debug用:現在の火力上昇値")]
    private int _bombUpPower = default;
    [SerializeField, Header("火力倍率")]
    private int _bombMagnification = default;
    //爆発時の爆発の大きさ
    private float _explodePower = default;
    [SerializeField, Header("置かれてから爆発するまでの時間")]
    private float _timeToExplode = default;
    [SerializeField, Header("実際に爆発している時間（当たり判定の残る時間）")]
    private float _explodeTime = default;
    //時間の計測
    private float _timeCnt = default;
    private Ray _rightRay = default;
    private Ray _leftRay = default;
    private Ray _upRay = default;
    private Ray _downRay = default;
    //順に上下左右の爆破力
    private float[] _rayLength = new float[4];
    private RaycastHit _hit = default;
    /// <summary>
    /// 爆弾の向き
    /// </summary>
    public enum BombRote{
        Up,
        Down,
        Right,
        Left,
    }
    [SerializeField]
    BombRote _bombRote = default;
    private void Awake()
    {
        //爆破力
        _explodePower = _bombPower + _bombUpPower*_bombMagnification+0.5f;
        //それぞれの方向の爆破力を格納
        for (int i = 0; _rayLength.Length > i; i++)
        {
            _rayLength[i] = _explodePower;
        }
        _rightRay = new Ray(transform.position, new Vector3(1, 0, 0));
        _leftRay = new Ray(transform.position, new Vector3(-1, 0, 0));
        _upRay = new Ray(transform.position, new Vector3(0, 0, 1));
        _downRay = new Ray(transform.position, new Vector3(0, 0, -1));
    }
    private void Update()
    {
        ExplodeCnt();
    }
    /// <summary>
    /// 爆発した時の処理
    /// </summary>
    private void Explosion()
    {
        //爆破時間が終了した
        if(_timeCnt>=_timeToExplode+_explodeTime)
        {
            Destroy(gameObject);
        }
        //一方向爆破
        if(!_isBombExplodeCross)
        {
            switch(_bombRote)
            {
                case BombRote.Up:
                    ExploLine(_upRay);
                    break;
                case BombRote.Down:
                    ExploLine(_downRay);
                    break;
                case BombRote.Right:
                    ExploLine(_rightRay);
                    break;
                case BombRote.Left:
                    ExploLine(_leftRay);
                    break;
            }
            return;
        }
        //十字型爆破
        
        for(int i =0;_rayLength.Length>i;i++)
        {
            ExploCross(i);
        }
    }
    /// <summary>
    /// 一方向爆発の処理
    /// </summary>
    private void ExploLine(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * _explodePower, Color.black);
        //爆破が何かにぶつかったとき
        if (Physics.Raycast(ray, out _hit, _explodePower))
        {
            ExploCollision(_hit);
            //あたった場所まで爆発の威力を減少させる
            _explodePower = Mathf.FloorToInt(Vector3.Distance(this.transform.position, _hit.transform.position));
        }
    }
    private void ExploCross(int direct)
    {
        Ray ray = default;
        switch(direct)
        {
            case 0:
                ray = _upRay;
                break;
            case 1:
                ray = _downRay;
                break;
            case 2:
                ray = _leftRay;
                break;
            case 3:
                ray = _rightRay;
                break;
        }
        Debug.DrawRay(ray.origin,ray.direction*_rayLength[direct],Color.black);
        //爆破が何かにぶつかったとき
        if (Physics.Raycast(ray, out _hit, _rayLength[direct]))
        {
            ExploCollision(_hit);
            //あたった場所まで爆発の威力を減少させる
            _rayLength[direct] = Mathf.FloorToInt(Vector3.Distance(this.transform.position, _hit.transform.position));
        }
    }
    private void ExploCollision(RaycastHit hit)
    {
        //プレイヤーに当たった時
        if (_hit.transform.tag == "Player")
        {


            //ここにプレイヤーが当たった時の処理を描く


            return;
        }
        //爆弾に当たった時
        else if (_hit.transform.tag == "Bomb")
        {
            //誘爆処理
            _hit.transform.GetComponent<Bomb>().Sympathetic();
            return;
        }
        Block block = _hit.transform.GetComponent<Block>();
        //地雷と旗のない壊せるブロックに当たった時
        if (!block.IsHaveFlag && block.IsBreakWall && !block.IsMine)
        {
            block.WallBreak();
            return;
        }
    }
    /// <summary>
    /// 爆発までのカウント
    /// </summary>
    private void ExplodeCnt()
    {
        _timeCnt += Time.deltaTime;
        //爆発開始
        if(_timeCnt>_timeToExplode)
        {
            Explosion();
        }
    }
    /// <summary>
    /// 誘爆した
    /// </summary>
    private void Sympathetic()
    {
        if(_timeCnt<_timeToExplode)
        {
            _timeCnt = _timeToExplode;
        }
    }
}
