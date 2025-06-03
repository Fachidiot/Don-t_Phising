using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int value;
    public Node node;
    public Block mergingBlock;
    public bool merging;

    public Vector2 Pos => GetComponent<RectTransform>().anchoredPosition;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    public void Init(BlockType _type)
    {
        value = _type.value;
        image.color = _type.color;
        text.text = _type.value.ToString();
    }

    public void SetBlock(Node _node)
    {
        if (node != null)
            node.OccupiedBlock = null;
        node = _node;
        node.OccupiedBlock = this;
    }

    public void MergeBlock(Block _blockToMergeWith)
    {   // Set the block we are merging with.
        mergingBlock = _blockToMergeWith;

        // Set current nodes as unoccupied to allow blocks to use it.
        node.OccupiedBlock = null;

        // Set the base block as merging, so it does not get used twice.
        _blockToMergeWith.merging = true;
    }

    public bool CanMerge(int _value) => _value.Equals(value) && !merging &&mergingBlock == null;
}
