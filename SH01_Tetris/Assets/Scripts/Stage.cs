using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Editor Objects")]          // �ν����Ϳ� Editor Objects �ؽ�Ʈ ����� �׷��� ������
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;

    [Header("Game Settings")]
    [Range(4, 40)]                      // Range(min,max) �ּ�/�ִ� ��
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCyecle = 1.0f;

    private int halfWidth;
    private int halfHeight;
    /// <summary>
    /// ������ ��Ʈ�ι̳밡 ������ �ð��� ������ �� �ʵ�
    /// </summary>
    private float nextFallTime;

    private void Start()
    {
        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);    //Round �ݿø� ToInt int������ ��ȯ�Ѵ�.
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);

        nextFallTime = Time.time + fallCyecle;      // fallCyecle�� ����� ������ ������ �ð��� ����

        CreateBackground();
        CreateTetromino();
    }

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;
        bool isRotate = false;              // ȸ������

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir.x = -1;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir.x = -1;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotate = true;
        }
        else if( Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir.y = -1;
        }
        //���⺤�Ͱ� 0�̰� ȸ���� ���ٸ� �Է°��� ���ٴ� �ǹ��̹Ƿ� �׷��� ���� ��쿡�� ��Ʈ�ι̳븦 �̵��ϴ� �޼��带 ȣ��
        if (moveDir != Vector3.zero || isRotate)
        {
            MoveTetromino(moveDir, isRotate);
        }

        
        if(Time.time > nextFallTime)
        { // ����ð��� ���� ������ �ð����� ũ�� ������ �̵���Ű�� , ����� �Է��� ����

            nextFallTime = Time.time +fallCyecle;
            moveDir = Vector3.down;
            isRotate = false;
        }

        if(moveDir != Vector3.zero || isRotate)
        {
            MoveTetromino(moveDir, isRotate);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveDir"></param>
    /// <param name="isRotate"></param>
    /// <returns>����� ȸ���� ���ڷ� ������ �̵��� �����ϸ� true, �̵��� �Ұ����ϸ� false�� ��ȯ
    bool MoveTetromino(Vector3 moveDir, bool isRotate)
    {
        tetrominoNode.transform.position += moveDir;
        if (isRotate)
        {
            tetrominoNode.transform.rotation *= Quaternion.Euler(0, 0, 90); // z �������� ȸ��
        }

        return true;
    }


    /// <summary>
    /// Ÿ�� �����
    /// </summary>
    /// <param name="parent">� ����� ����(�ڽ�)���� ������ �ٰ����� ����. �θ�Ȱ</param>
    /// <param name="position">(x,y)��ǥ </param>
    /// <param name="color">Ÿ���� ����</param>
    /// <param name="order">���̾���� ���� ����(Sorting Oder)�� �׸� ����</param>
    /// <returns></returns>
    Tile CreateTile(Transform parent, Vector2 position, Color color, int order=1)
    {
        var go = Instantiate(tilePrefab); // ���������κ��� ������ �� �θ� �����ϰ� ���� �� ���ļ����� ����
        go.transform.parent = parent;
        go.transform.localPosition = position;

        var tile = go.GetComponent<Tile>();
        tile.color= color;
        tile.sortingOrder = order;

        return tile;
    }
    /// <summary>
    /// ��� Ÿ���� ����
    /// </summary>
    private void CreateBackground()
    {
        Color color = Color.gray;

        // Ÿ�� ����
        color.a = 0.5f;
        for(int x=  -halfWidth; x < halfWidth; ++x)
        {
            for (int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        //�¿� �׵θ�
        color.a = 1.0f;
        for(int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // �Ʒ� �׵θ�
        for(int x = -halfWidth -1 ; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }
    /// <summary>
    /// ��Ʈ�ι̳� ����
    /// </summary>
    private void CreateTetromino()
    {
        int index = UnityEngine.Random.Range(0, 7);
        Color32 color = Color.white;

        tetrominoNode.rotation = Quaternion.identity;           // ȸ�� ����.
        tetrominoNode.position = new Vector2(0, halfHeight);    // �⺻��ġ�� ����

        switch (index)
        {
            // I : �ϴû�
            case 0:
                color = new Color32(115, 251, 253, 255);
                CreateTile(tetrominoNode, new Vector2(-2f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                break;

            // J : �Ķ���
            case 1:
                color = new Color32(0,33,245,255);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 1.0f), color);
                break;

            // L : �ֻ�
            case 2:
                color = new Color32(243,168,59,255);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 1.0f), color);
                break;

            // O : �����
            case 3:
                color = new Color32(255,253,84,255);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 1f), color);
                break;

            // S : ���
            case 4:
                color = new Color32(117, 250,76,255);
                CreateTile(tetrominoNode, new Vector2(-1f, -1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, -1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                break;

            // T: ���ֻ�
            case 5:
                color = new Color32(115, 47, 246, 255);
                CreateTile(tetrominoNode, new Vector2(-1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                break;
            // Z : ������
            case 6:
                color = new Color32(235,51,35,255);
                CreateTile(tetrominoNode, new Vector2(-2f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                break;
        }
    }

}
