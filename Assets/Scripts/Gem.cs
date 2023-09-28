using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Vector2 _pos;
    Board _board;

    public void SetUpGem(Vector2 pos, Board board)
    {
        _pos = pos;
        _board = board;
    }
}
