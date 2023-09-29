using System;
using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] Vector2 _pos;

    [SerializeField] float _gemSpeed;
    Board _board;
    Gem _otherGem;
    Vector2 _startTouchPosition;
    Vector2 _finishTouchPosition;
    bool _touchPressed;
    float _swipeAngle=0;

    void Update()
    {
        if (Vector2.Distance(transform.localPosition, _pos) > .01f)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, _pos, _gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = _pos;
            _board.GemsOnBoard[(int)_pos.x, (int)_pos.y] = this;
        }
        if (_touchPressed && Input.GetMouseButtonUp(0))
        {
            _touchPressed = false;
            if (Camera.main != null)
            {
                _finishTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
                if (Vector2.Distance(_finishTouchPosition, _startTouchPosition) > 0.5f)
                {
                    MoveGem();
                }
                
            }
        }
    }

    public void SetUpGem(Vector2 pos, Board board)
    {
        _pos = pos;
        _board = board;
    }

    void OnMouseDown()
    {
        if (Camera.main != null)
        {
            _startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _touchPressed = true;   
        }
    }

    void CalculateAngle()
    {
        _swipeAngle = Mathf.Atan2(_finishTouchPosition.x - _startTouchPosition.x,
            _finishTouchPosition.y - _startTouchPosition.y);
        _swipeAngle = _swipeAngle * 180 / Mathf.PI;
        
    }

    void MoveGem()
    {
            //UP
        if (_swipeAngle is >= -45 and < 45 && (_pos.y + 1) < _board.Height)
        {
            _otherGem = _board.GemsOnBoard[(int)_pos.x, (int)_pos.y + 1];
            _otherGem._pos.y --;
            _pos.y ++;
            
        } else if (_swipeAngle is >= 45 and < 135 && (_pos.x + 1) < _board.Width)
            //RIGHT
        {
            _otherGem = _board.GemsOnBoard[(int)_pos.x + 1, (int)_pos.y];
            _otherGem._pos.x --;
            _pos.x ++;

        } else if (_swipeAngle is < -45 and >= -135 &&  _pos.x > 0)
            //LEFT
        {
            _otherGem = _board.GemsOnBoard[(int)_pos.x - 1, (int)_pos.y];
            _otherGem._pos.x ++;
            _pos.x --;

        } else if (_swipeAngle is < -135 or >= 135 &&  _pos.y > 0)
            //DOWN
        {
            _otherGem = _board.GemsOnBoard[(int)_pos.x, (int)_pos.y - 1];
            _otherGem._pos.y ++;
            _pos.y --;

        }
        _board.GemsOnBoard[(int)_pos.x, (int)_pos.y] = this;
        _board.GemsOnBoard[(int)_otherGem._pos.x, (int)_otherGem._pos.y] = _otherGem;

    }
}
