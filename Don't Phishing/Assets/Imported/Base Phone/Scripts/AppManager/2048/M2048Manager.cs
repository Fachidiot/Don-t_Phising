using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class M2048Manager : BaseAppManager
{
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;
    [SerializeField] private float scale = 200;

    [SerializeField] private Node nodePrefab;
    [SerializeField] private Block blockPrefab;

    [SerializeField] private GameObject nodeParent;
    [SerializeField] private List<BlockType> typeList;
    [SerializeField] private float travelTime = 0.2f;
    [SerializeField] private int windCondition = 2048;

    [SerializeField] private GameObject winScreen, loseScreen;

    private List<Node> nodeList = new List<Node>();
    private List<Block> blockList = new List<Block>();
    private Game2048State state;
    private int round;

    private BlockType GetBlockTypeByValue(int _value) => typeList.First(t=>t.value == _value);

    void Start()
    {
        ChangeState(Game2048State.GenerateLevel);
    }

    private void ChangeState(Game2048State _state)
    {
        state = _state;

        switch(state)
        {
            case Game2048State.GenerateLevel:
                GenerateGrid();
                break;
            case Game2048State.SpawningBlocks:
                SpawnBlocks(round++ == 0 ? 2 : 1);
                break;
            case Game2048State.WaitingInput:
                break;
            case Game2048State.Moving:
                break;
            case Game2048State.Win:
                winScreen.SetActive(true);
                break;
            case Game2048State.Lose:
                loseScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state != Game2048State.WaitingInput)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Shift(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Shift(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Shift(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Shift(Vector2.down);
    }

    void GenerateGrid()
    {
        round = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = Instantiate(nodePrefab, nodeParent.transform);
                node.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * scale, y * scale);
                nodeList.Add(node);
            }
        }

        ChangeState(Game2048State.SpawningBlocks);
    }

    void SpawnBlocks(int _amount)
    {
        List<Node> freeNodes = nodeList.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();

        foreach (Node node in freeNodes.Take(_amount))
        {
            SpawnBlock(node, UnityEngine.Random.value > 0.8f ? 4 : 2);
        }

        if (freeNodes.Count() == 1 && !CanMerge())
        {   // Lose the game.
            ChangeState(Game2048State.Lose);
            return; 
        }

        ChangeState(blockList.Any(b => b.value == windCondition) ? Game2048State.Win : Game2048State.WaitingInput);
    }

    void SpawnBlock(Node _node, int _value)
    {
        Block block = Instantiate(blockPrefab, new Vector3(_node.Pos.x, _node.Pos.y, 0), Quaternion.identity);
        block.transform.SetParent(nodeParent.transform, false);
        block.Init(GetBlockTypeByValue(_value));
        block.SetBlock(_node);
        blockList.Add(block);
    }

    void Shift(Vector2 _direction)
    {
        ChangeState(Game2048State.Moving);

        List<Block> orderedBlocks = blockList.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if (_direction == Vector2.right || _direction == Vector2.up) orderedBlocks.Reverse();

        foreach (Block block in orderedBlocks)
        {
            Node next = block.node;
            do
            {
                block.SetBlock(next);

                Node possibleNode = GetNodeAtPosition(next.Pos + _direction * scale);
                if (possibleNode != null)
                {
                    // We know a node is present.
                    // If it's possible to merge, set merge.
                    if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.value))
                    {
                        block.MergeBlock(possibleNode.OccupiedBlock);
                    }
                    // Otherwise, can we move to this spot?
                    else if (possibleNode.OccupiedBlock == null)
                        next = possibleNode;

                    // None hit? End do while loop.
                }

            }
            while (next != block.node);
        }

        var sequence = DOTween.Sequence();

        foreach (Block block in orderedBlocks)
        {
            Vector3 movePoint = block.mergingBlock != null ? block.mergingBlock.node.Pos : block.node.Pos;

            //sequence.Insert(0, block.transform.DOMove(movePoint, travelTime));
            block.GetComponent<RectTransform>().anchoredPosition = movePoint;
        }

        sequence.OnComplete(() =>
        {
            foreach (Block block in orderedBlocks.Where(b => b.mergingBlock != null))
            {
                MergeBlocks(block.mergingBlock, block);
            }

            ChangeState(Game2048State.SpawningBlocks);
        });
    }

    bool CanMerge()
    {
        List<Block> orderedBlocks = blockList.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();

        return CheckDirection(Vector2.left, orderedBlocks) || CheckDirection(Vector2.right, orderedBlocks)
            || CheckDirection(Vector2.up, orderedBlocks) || CheckDirection(Vector2.down, orderedBlocks); 
    }

    bool CheckDirection(Vector2 _direction, List<Block> _orderedBlocks)
    {
        if (_direction == Vector2.right || _direction == Vector2.up) _orderedBlocks.Reverse();

        foreach (Block block in _orderedBlocks)
        {
            Node next = block.node;
            do
            {
                block.SetBlock(next);

                Node possibleNode = GetNodeAtPosition(next.Pos + _direction * scale);
                if (possibleNode != null)
                {
                    // If it's possible to merge
                    if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.value))
                        return true;
                    // Otherwise, can we move to this spot?
                    else if (possibleNode.OccupiedBlock == null)
                        next = possibleNode;

                    // None hit? End do while loop.
                }

            }
            while (next != block.node);
        }

        return false;
    }

    void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        SpawnBlock(baseBlock.node, baseBlock.value * 2);

        RemoveBlock(baseBlock);
        RemoveBlock(mergingBlock);
    }

    void RemoveBlock(Block block)
    {
        blockList.Remove(block);
        Destroy(block.gameObject);
    }

    Node GetNodeAtPosition(Vector2 _pos)
    {
        return nodeList.FirstOrDefault(n=>n.Pos == _pos);
    }

    public override void ResetApp()
    {
        blockList.Clear();
        nodeList.Clear();
        ChangeState(Game2048State.GenerateLevel);

        return;
    }

    public override void SetText()
    {
        return;
    }
}

[Serializable]
public struct BlockType
{
    public int value;
    public Color color;
}

public enum Game2048State
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInput,
    Moving,
    Win,
    Lose
}