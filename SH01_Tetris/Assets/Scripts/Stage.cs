using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Editor Objects")]          // 인스펙터에 Editor Objects 텍스트 출력해 그룹을 구분함
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;

    [Header("Game Settings")]
    [Range(4, 40)]                      // Range(min,max) 최소/최댓 값
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCyecle = 1.0f;

    private int halfWidth;
    private int halfHeight;
    /// <summary>
    /// 다음에 테트로미노가 떨어질 시간을 저장해 둘 필드
    /// </summary>
    private float nextFallTime;

    private void Start()
    {
        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);    //Round 반올림 ToInt int형으로 반환한다.
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);

        nextFallTime = Time.time + fallCyecle;      // fallCyecle을 사용해 다음에 떨어질 시간을 지정

        CreateBackground();
        CreateTetromino();
    }

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;
        bool isRotate = false;              // 회전여부

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
        //방향벡터가 0이고 회전이 없다면 입력값이 없다는 의미이므로 그렇지 않은 경우에만 테트로미노를 이동하는 메서드를 호출
        if (moveDir != Vector3.zero || isRotate)
        {
            MoveTetromino(moveDir, isRotate);
        }

        
        if(Time.time > nextFallTime)
        { // 현재시간이 다음 떨어질 시간보다 크면 강제로 이동시키고 , 사용자 입력을 무시

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
    /// <returns>방향과 회전을 인자로 받으며 이동이 가능하면 true, 이동이 불가능하면 false를 반환
    bool MoveTetromino(Vector3 moveDir, bool isRotate)
    {
        tetrominoNode.transform.position += moveDir;
        if (isRotate)
        {
            tetrominoNode.transform.rotation *= Quaternion.Euler(0, 0, 90); // z 기준으로 회전
        }

        return true;
    }


    /// <summary>
    /// 타일 만들기
    /// </summary>
    /// <param name="parent">어떤 노드의 하위(자식)으로 연겨해 줄것인지 지정. 부모역활</param>
    /// <param name="position">(x,y)좌표 </param>
    /// <param name="color">타일의 색상</param>
    /// <param name="order">레이어에서의 정렬 순서(Sorting Oder)로 그릴 순서</param>
    /// <returns></returns>
    Tile CreateTile(Transform parent, Vector2 position, Color color, int order=1)
    {
        var go = Instantiate(tilePrefab); // 프리팹으로부터 복제한 후 부모를 지정하고 색상 및 정렬순서를 지정
        go.transform.parent = parent;
        go.transform.localPosition = position;

        var tile = go.GetComponent<Tile>();
        tile.color= color;
        tile.sortingOrder = order;

        return tile;
    }
    /// <summary>
    /// 배경 타일을 생성
    /// </summary>
    private void CreateBackground()
    {
        Color color = Color.gray;

        // 타일 보드
        color.a = 0.5f;
        for(int x=  -halfWidth; x < halfWidth; ++x)
        {
            for (int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        //좌우 테두리
        color.a = 1.0f;
        for(int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // 아래 테두리
        for(int x = -halfWidth -1 ; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }
    /// <summary>
    /// 테트로미노 생성
    /// </summary>
    private void CreateTetromino()
    {
        int index = UnityEngine.Random.Range(0, 7);
        Color32 color = Color.white;

        tetrominoNode.rotation = Quaternion.identity;           // 회전 없음.
        tetrominoNode.position = new Vector2(0, halfHeight);    // 기본위치를 지정

        switch (index)
        {
            // I : 하늘색
            case 0:
                color = new Color32(115, 251, 253, 255);
                CreateTile(tetrominoNode, new Vector2(-2f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                break;

            // J : 파란색
            case 1:
                color = new Color32(0,33,245,255);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 1.0f), color);
                break;

            // L : 귤색
            case 2:
                color = new Color32(243,168,59,255);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 1.0f), color);
                break;

            // O : 노란색
            case 3:
                color = new Color32(255,253,84,255);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 1f), color);
                break;

            // S : 녹색
            case 4:
                color = new Color32(117, 250,76,255);
                CreateTile(tetrominoNode, new Vector2(-1f, -1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, -1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                break;

            // T: 자주색
            case 5:
                color = new Color32(115, 47, 246, 255);
                CreateTile(tetrominoNode, new Vector2(-1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                break;
            // Z : 빨간색
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
