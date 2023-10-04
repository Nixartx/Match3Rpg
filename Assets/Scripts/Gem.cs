using UnityEngine;

public class Gem : MonoBehaviour
{
    Vector2 _pos;

    public int PosX
    {
        get { return (int)_pos.x; }
        set { _pos.x = value; }
    }
    public int PosY
    {
        get { return (int)_pos.y; }
        set { _pos.y = value; }
    }
    [SerializeField] GemType _gemType;

    public GemType GemType
    {
        get { return _gemType; }
    }

    [SerializeField] float _gemSpeed;
    Board _board;
    Vector2 _startTouchPosition;
    Vector2 _finishTouchPosition;
    bool _touchPressed;
    float _swipeAngle=0;
    [SerializeField] bool _isMatched;
    public bool IsMatched
    {
        get { return _isMatched; }
        set { _isMatched = value; }
    }

    void Update()
    {
        if (Vector2.Distance(transform.localPosition, _pos) > .01f)
            transform.localPosition = Vector2.Lerp(transform.localPosition, _pos, _gemSpeed * Time.deltaTime);
        else
            transform.localPosition = _pos;
        
        if (_touchPressed && Input.GetMouseButtonUp(0))
        {
            _touchPressed = false;
            if (Camera.main != null)
            {
                _finishTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
                if (Vector2.Distance(_finishTouchPosition, _startTouchPosition) > 0.5f)
                {
                    _board.MoveGems(new Vector2Int(PosX,PosY), _swipeAngle);
                }
                
            }
        }
    }

    public void SetUpGem(Vector2 pos, Board board)
    {
        this._pos = pos;
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
    
}

public enum GemType
{
    Red,Yellow,Purple,Green,Blue
}