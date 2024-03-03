using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    Coroutine _coSkill;
    bool _rangedSkill = false;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateAnim()
    {
        if (_state == CreatureState.Idle)
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
        else if (_state == CreatureState.Moving)
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
                    _anim.Play(_rangedSkill ? "ATTACK_BACK" : "ATTACK_WEAPONE_BACK");
                    break;
                case MoveDir.Down:
                    _anim.Play(_rangedSkill ? "ATTACK_FRONT" : "ATTACK_WEAPONE_FRONT");
                    break;
                case MoveDir.Left:
                    _anim.Play(_rangedSkill ? "ATTACK_SIDE" : "ATTACK_WEAPONE_SIDE");
                    break;
                case MoveDir.Right:
                    _anim.Play(_rangedSkill ? "ATTACK_SIDE" : "ATTACK_WEAPONE_SIDE");
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

    protected override void UpdateController()
    {
        switch(State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;
            case CreatureState.Moving:
                GetDirInput();
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                break;
        }

        base.UpdateController();
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }


    protected override void UpdateIdle()
    {
        if(Dir != MoveDir.None)
        {
            State = CreatureState.Moving;
            return;
        }    

        if (Input.GetKey(KeyCode.Space))
        {
            State = CreatureState.Skill;
            _coSkill = StartCoroutine(CoStartShootArrow());
        }
    }

    IEnumerator CoStartPunch()
    {
        //피격 판정
        GameObject go = Managers.Object.Find(GetFrontCellPos());
        if(go != null)
        {
            Debug.Log(go.name);
        }

        //대기 시간
        _rangedSkill = false;
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    IEnumerator CoStartShootArrow()
    {
        GameObject go = Managers.Resource.Instantiate("Creature/Arrow");
        ArrowController ac = go.GetOrAddComponent<ArrowController>();
        ac.Dir = _lastDir;
        ac.CellPos = CellPos;
        _rangedSkill = true;
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }
    }

    public override void OnDamaged()
    {
        Debug.Log("Player Hit");
    }
}