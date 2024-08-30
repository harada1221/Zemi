using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    //”š”j‚Ìí—Ş
    private bool _isBombExplodeCross = default;
    [SerializeField,Header("”š’e‚ÌŠî‘bˆĞ—Í")]
    private int _bombPower = default;
    [SerializeField, Header("debug—p:Œ»İ‚Ì‰Î—Íã¸’l")]
    private int _bombUpPower = default;
    [SerializeField, Header("‰Î—Í”{—¦")]
    private int _bombMagnification = default;
    //”š”­‚Ì”š”­‚Ì‘å‚«‚³
    private float _explodePower = default;
    [SerializeField, Header("’u‚©‚ê‚Ä‚©‚ç”š”­‚·‚é‚Ü‚Å‚ÌŠÔ")]
    private float _timeToExplode = default;
    [SerializeField, Header("ÀÛ‚É”š”­‚µ‚Ä‚¢‚éŠÔi“–‚½‚è”»’è‚Ìc‚éŠÔj")]
    private float _explodeTime = default;
    //ŠÔ‚ÌŒv‘ª
    private float _timeCnt = default;
    private Ray _rightRay = default;
    private Ray _leftRay = default;
    private Ray _upRay = default;
    private Ray _downRay = default;
    //‡‚Éã‰º¶‰E‚Ì”š”j—Í
    private float[] _rayLength = new float[4];
    private RaycastHit _hit = default;
    /// <summary>
    /// ”š’e‚ÌŒü‚«
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
        //”š”j—Í
        _explodePower = _bombPower + _bombUpPower*_bombMagnification+0.5f;
        //‚»‚ê‚¼‚ê‚Ì•ûŒü‚Ì”š”j—Í‚ğŠi”[
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
    /// ”š”­‚µ‚½‚Ìˆ—
    /// </summary>
    private void Explosion()
    {
        //”š”jŠÔ‚ªI—¹‚µ‚½
        if(_timeCnt>=_timeToExplode+_explodeTime)
        {
            Destroy(gameObject);
        }
        //ˆê•ûŒü”š”j
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
        //\šŒ^”š”j
        
        for(int i =0;_rayLength.Length>i;i++)
        {
            ExploCross(i);
        }
    }
    /// <summary>
    /// ˆê•ûŒü”š”­‚Ìˆ—
    /// </summary>
    private void ExploLine(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * _explodePower, Color.black);
        //”š”j‚ª‰½‚©‚É‚Ô‚Â‚©‚Á‚½‚Æ‚«
        if (Physics.Raycast(ray, out _hit, _explodePower))
        {
            ExploCollision(_hit);
            //‚ ‚½‚Á‚½êŠ‚Ü‚Å”š”­‚ÌˆĞ—Í‚ğŒ¸­‚³‚¹‚é
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
        //”š”j‚ª‰½‚©‚É‚Ô‚Â‚©‚Á‚½‚Æ‚«
        if (Physics.Raycast(ray, out _hit, _rayLength[direct]))
        {
            ExploCollision(_hit);
            //‚ ‚½‚Á‚½êŠ‚Ü‚Å”š”­‚ÌˆĞ—Í‚ğŒ¸­‚³‚¹‚é
            _rayLength[direct] = Mathf.FloorToInt(Vector3.Distance(this.transform.position, _hit.transform.position));
        }
    }
    private void ExploCollision(RaycastHit hit)
    {
        //ƒvƒŒƒCƒ„[‚É“–‚½‚Á‚½
        if (_hit.transform.tag == "Player")
        {


            //‚±‚±‚ÉƒvƒŒƒCƒ„[‚ª“–‚½‚Á‚½‚Ìˆ—‚ğ•`‚­


            return;
        }
        //”š’e‚É“–‚½‚Á‚½
        else if (_hit.transform.tag == "Bomb")
        {
            //—U”šˆ—
            _hit.transform.GetComponent<Bomb>().Sympathetic();
            return;
        }
        Block block = _hit.transform.GetComponent<Block>();
        //’n—‹‚ÆŠø‚Ì‚È‚¢‰ó‚¹‚éƒuƒƒbƒN‚É“–‚½‚Á‚½
        if (!block.IsHaveFlag && block.IsBreakWall && !block.IsMine)
        {
            block.WallBreak();
            return;
        }
    }
    /// <summary>
    /// ”š”­‚Ü‚Å‚ÌƒJƒEƒ“ƒg
    /// </summary>
    private void ExplodeCnt()
    {
        _timeCnt += Time.deltaTime;
        //”š”­ŠJn
        if(_timeCnt>_timeToExplode)
        {
            Explosion();
        }
    }
    /// <summary>
    /// —U”š‚µ‚½
    /// </summary>
    private void Sympathetic()
    {
        if(_timeCnt<_timeToExplode)
        {
            _timeCnt = _timeToExplode;
        }
    }
}
