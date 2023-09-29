using System;
using UnityEngine;
using Random = UnityEngine.Random;



public class Board : MonoBehaviour
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    public int Width { get => _width;}
    public int Height { get => _height;}
    [SerializeField] float _scale;
    [SerializeField] Gem[] _gems;
    
    [SerializeField]  GameObject _backgroundPrefabTile;
    public Gem[,] GemsOnBoard { get; private set; }


    void Start()
    {
        GemsOnBoard = new Gem[_width, _height];
        SetUp();
    }

    private void SetUp()
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

    private void SpawnGem(Vector2Int pos,Gem gemPrefab)
    {
        Gem gem = Instantiate(gemPrefab, new Vector3(pos.x, pos.y,0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = $"Gem: {pos.x},{pos.y}";
        gem.SetUpGem(new Vector2(pos.x, pos.y), this);
        GemsOnBoard[pos.x, pos.y] = gem;
    }
}
