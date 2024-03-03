using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureController : MonoBehaviour
{
    public float _speed = 5.0f;

    public Vector3Int CellPos { get; set; } = Vector3Int.zero;
    protected Animator _anim;
    protected SpriteRenderer _spriteRenderer;

    public virtual CreatureState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state == value)
                return;

            _state = value;
            UpdateAnim();
        }
    }

    protected CreatureState _state = CreatureState.Idle;

    public MoveDir Dir
    {
        get
        {
            return _dir;
        }
        set
        {
            if (_dir == value)
                return;

            _dir = value;
            if (value != MoveDir.None)
                _lastDir = value;
            UpdateAnim();
        }
    }

    public MoveDir GetDirFromVec(Vector3Int dir)
    {
        if (dir.x > 0)
            return MoveDir.Right;
        else if (dir.x < 0)
            return MoveDir.Left;
        else if (dir.y > 0)
            return MoveDir.Up;
        else if (dir.y < 0)
            return MoveDir.Down;
        else
            return MoveDir.None;
    }

    protected MoveDir _dir = MoveDir.Down;
    protected MoveDir _lastDir = MoveDir.Down;

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;
        switch(_lastDir)
        {
            case MoveDir.Up:
                cellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
        }

        return cellPos;
    }

    protected virtual void UpdateAnim()
    {
        if(_state == CreatureState.Idle)
        {
            switch (_lastDir)
            {
                case MoveDir.Up:
                    _anim.Play("IDLE_BACK");
                    break;
                case MoveDir.Down:
                    _anim.Play("IDLE_FRONT");
                    break;
                case MoveDir.Left:
                    _anim.Play("IDLE_SIDE");
                    break;
                case MoveDir.Right:
                    _anim.Play("IDLE_SIDE");
                    break;
            }
        }
        else if(_state == CreatureState.Moving)
        {
            switch (_dir)
            {
                case MoveDir.Up:
                    _anim.Play("WALK_BACK");
                    break;
                case MoveDir.Down:
                    _anim.Play("WALK_FRONT");
                    break;
                case MoveDir.Left:
                    _anim.Play("WALK_SIDE");
                    break;
                case MoveDir.Right:
                    _anim.Play("WALK_SIDE");
                    break;
            }
        }
        else if (_state == CreatureState.Skill)
        {
            switch (_lastDir)
            {
                case MoveDir.Up:
                    _anim.Play("ATTACK_BACK");
                    break;            
                case MoveDir.Down:    
                    _anim.Play("ATTACK_FRONT");
                    break;            
                case MoveDir.Left:    
                    _anim.Play("ATTACK_SIDE");
                    break;            
                case MoveDir.Right:   
                    _anim.Play("ATTACK_SIDE");
                    break;
            }
        }
        else
        {

        }

        if (_lastDir == MoveDir.Left)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }
    protected virtual void Init()
    {
        transform.position = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void UpdateController()
    {
        switch(State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
        
    }

    protected virtual void UpdateMoving()
    {
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;

        float distance = moveDir.magnitude;
        if (distance < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            MoveToNextPos();
        }
        else
        {
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    protected virtual void MoveToNextPos()
    {
        if(Dir == MoveDir.None)
        {
            State = CreatureState.Idle;
            return;
        }
        else
        {
            Vector3Int destPos = CellPos;

            switch (_dir)
            {
                case MoveDir.Up:
                    destPos += Vector3Int.up;
                    break;
                case MoveDir.Down:
                    destPos += Vector3Int.down;
                    break;
                case MoveDir.Left:
                    destPos += Vector3Int.left;
                    break;
                case MoveDir.Right:
                    destPos += Vector3Int.right;
                    break;
            }

            if (Managers.Map.CanGo(destPos))
            {
                if (Managers.Object.Find(destPos) == null)
                    CellPos = destPos;
            }
        }
    }

    protected virtual void UpdateSkill()
    {

    }

    protected virtual void UpdateDead()
    {

    }

    public virtual void OnDamaged()
    {

    }
}
