using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    //���j�̎��
    private bool _isBombExplodeCross = default;
    [SerializeField,Header("���e�̊�b�З�")]
    private int _bombPower = default;
    [SerializeField, Header("debug�p:���݂̉Η͏㏸�l")]
    private int _bombUpPower = default;
    [SerializeField, Header("�Η͔{��")]
    private int _bombMagnification = default;
    //�������̔����̑傫��
    private float _explodePower = default;
    [SerializeField, Header("�u����Ă��甚������܂ł̎���")]
    private float _timeToExplode = default;
    [SerializeField, Header("���ۂɔ������Ă��鎞�ԁi�����蔻��̎c�鎞�ԁj")]
    private float _explodeTime = default;
    //���Ԃ̌v��
    private float _timeCnt = default;
    private Ray _rightRay = default;
    private Ray _leftRay = default;
    private Ray _upRay = default;
    private Ray _downRay = default;
    //���ɏ㉺���E�̔��j��
    private float[] _rayLength = new float[4];
    private RaycastHit _hit = default;
    /// <summary>
    /// ���e�̌���
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
        //���j��
        _explodePower = _bombPower + _bombUpPower*_bombMagnification+0.5f;
        //���ꂼ��̕����̔��j�͂��i�[
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
    /// �����������̏���
    /// </summary>
    private void Explosion()
    {
        //���j���Ԃ��I������
        if(_timeCnt>=_timeToExplode+_explodeTime)
        {
            Destroy(gameObject);
        }
        //��������j
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
        //�\���^���j
        
        for(int i =0;_rayLength.Length>i;i++)
        {
            ExploCross(i);
        }
    }
    /// <summary>
    /// ����������̏���
    /// </summary>
    private void ExploLine(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * _explodePower, Color.black);
        //���j�������ɂԂ������Ƃ�
        if (Physics.Raycast(ray, out _hit, _explodePower))
        {
            ExploCollision(_hit);
            //���������ꏊ�܂Ŕ����̈З͂�����������
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
        //���j�������ɂԂ������Ƃ�
        if (Physics.Raycast(ray, out _hit, _rayLength[direct]))
        {
            ExploCollision(_hit);
            //���������ꏊ�܂Ŕ����̈З͂�����������
            _rayLength[direct] = Mathf.FloorToInt(Vector3.Distance(this.transform.position, _hit.transform.position));
        }
    }
    private void ExploCollision(RaycastHit hit)
    {
        //�v���C���[�ɓ���������
        if (_hit.transform.tag == "Player")
        {


            //�����Ƀv���C���[�������������̏�����`��


            return;
        }
        //���e�ɓ���������
        else if (_hit.transform.tag == "Bomb")
        {
            //�U������
            _hit.transform.GetComponent<Bomb>().Sympathetic();
            return;
        }
        Block block = _hit.transform.GetComponent<Block>();
        //�n���Ɗ��̂Ȃ��󂹂�u���b�N�ɓ���������
        if (!block.IsHaveFlag && block.IsBreakWall && !block.IsMine)
        {
            block.WallBreak();
            return;
        }
    }
    /// <summary>
    /// �����܂ł̃J�E���g
    /// </summary>
    private void ExplodeCnt()
    {
        _timeCnt += Time.deltaTime;
        //�����J�n
        if(_timeCnt>_timeToExplode)
        {
            Explosion();
        }
    }
    /// <summary>
    /// �U������
    /// </summary>
    private void Sympathetic()
    {
        if(_timeCnt<_timeToExplode)
        {
            _timeCnt = _timeToExplode;
        }
    }
}
