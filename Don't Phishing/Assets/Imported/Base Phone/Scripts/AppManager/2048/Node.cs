using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 Pos => GetComponent<RectTransform>().anchoredPosition;
    public Block OccupiedBlock;
}
