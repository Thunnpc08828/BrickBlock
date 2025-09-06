using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockData
{
    public BlockType Type;
    public int Health;
    public float X;
    public float Y;


}

public enum BlockType
{
    Empty,
    Normal,
    Bomb,
}
