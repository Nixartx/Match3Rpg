using System;
using UnityEngine;
using Random = UnityEngine.Random;



public class Board : MonoBehaviour
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] float _scale;
    [SerializeField] Gem[] _gems;
    [SerializeField]  GameObject _backgroundPrefabTile;
    Gem[,] _gemsOnBoard;
    Gem _firstGem;
    Gem _otherGem;

    void Start()
    {
        _gemsOnBoard = new Gem[_width, _height];
        SetUp();
    }

    void Update()
    {
        FindMatches();
    }

    void SetUp()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(_backgroundPrefabTile, pos, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = $"BG_Tile: {x}, {y}";
                int gemIndex = Random.Range(0, _gems.Length);
                SpawnGem(new Vector2Int(x,y), _gems[gemIndex]);
            }
        }
        float centerX = (_width / 2.0f) - 0.5f;
        float centerY = (_height / 2.0f) - 0.5f;
        transform.position = new Vector2(-centerX * _scale,-centerY * _scale);
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    void SpawnGem(Vector2Int pos,Gem gemPrefab)
    {
        Gem gem = Instantiate(gemPrefab, new Vector3(pos.x, pos.y,0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = $"Gem: {pos.x},{pos.y}";
        gem.SetUpGem(new Vector2(pos.x, pos.y), this);
        _gemsOnBoard[pos.x, pos.y] = gem;
    }
    
    public void MoveGems(Vector2Int firstGemPos, float swipeAngle)
    {
        _firstGem = _gemsOnBoard[firstGemPos.x, firstGemPos.y];
            //UP
        if (swipeAngle is >= -45 and < 45 && (_firstGem.PosY + 1) < _height)
        {
            _otherGem = _gemsOnBoard[_firstGem.PosX, _firstGem.PosY + 1];
            _otherGem.PosY --;
            _firstGem.PosY ++;
            
        } else if (swipeAngle is >= 45 and < 135 && (_firstGem.PosX + 1) < _width)
            //RIGHT
        {
            _otherGem = _gemsOnBoard[_firstGem.PosX + 1, _firstGem.PosY];
            _otherGem.PosX --;
            _firstGem.PosX ++;

        } else if (swipeAngle is < -45 and >= -135 &&  _firstGem.PosX > 0)
            //LEFT
        {
            _otherGem = _gemsOnBoard[_firstGem.PosX - 1, _firstGem.PosY];
            _otherGem.PosX ++;
            _firstGem.PosX --;

        } else if (swipeAngle is < -135 or >= 135 &&  _firstGem.PosY > 0)
            //DOWN
        {
            _otherGem = _gemsOnBoard[_firstGem.PosX, _firstGem.PosY - 1];
            _otherGem.PosY ++;
            _firstGem.PosY --;

        }
        _gemsOnBoard[_firstGem.PosX, _firstGem.PosY] = _firstGem;
        _gemsOnBoard[_otherGem.PosX, _otherGem.PosY] = _otherGem;

    }
    
    void FindMatches()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Gem currentGem = _gemsOnBoard[x, y];
                if (currentGem != null)
                {
                    //Horizontal
                    if (x > 0 && x < _width - 1)
                    {
                        Gem leftGem = _gemsOnBoard[x - 1, y];
                        Gem rightGem = _gemsOnBoard[x + 1, y];
                        if (leftGem != null && rightGem != null)
                        {
                            if (currentGem.GemType == leftGem.GemType && currentGem.GemType == rightGem.GemType)
                            {
                                currentGem.IsMatched = true;
                                leftGem.IsMatched = true;
                                rightGem.IsMatched = true;
                            }
                        }
                    }
                    //Vertical
                    if (y > 0 && y < _height - 1)
                    {
                        Gem topGem = _gemsOnBoard[x, y + 1];
                        Gem bottomGem = _gemsOnBoard[x, y - 1];
                        if (topGem != null && bottomGem != null)
                        {
                            if (currentGem.GemType == topGem.GemType && currentGem.GemType == bottomGem.GemType)
                            {
                                currentGem.IsMatched = true;
                                topGem.IsMatched = true;
                                bottomGem.IsMatched = true;
                            }
                        }
                    }
                }

            }
        }
    }
}
