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

        for (int i =0; i <boardHeight; ++i)
        {
            var col = new GameObject((boardHeight - i - 1).ToString()); // ToString()은 c#기본기능, 정수를 문자열로 변환해 주는 메서드
            col.transform.position = new Vector3(0, halfHeight- i, 0);  // 각 노드는 이름의 위치의 가로 중앙에 위치한다.
            col.transform.parent = boardNode;                           // 보드 노드의 자식으로 만들어준다
        }

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
            moveDir.x = 1;
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
        Vector3 oldPos = tetrominoNode.transform.position;      // 이동 가능한지 확인 후 불가능하면 
        Quaternion oldRot = tetrominoNode.transform.rotation;   // 원래 위치로 되돌리기 위해 현재 값을 저장

        tetrominoNode.transform.position += moveDir;
        if (isRotate)
        {
            tetrominoNode.transform.rotation *= Quaternion.Euler(0, 0, 90); // z 기준으로 회전
        }

        if(!CanMoveTo(tetrominoNode))   // 반환값이 거짓(false)이면 이동이 불가능, 위에서 저장해둔 기존 위치로 되돌아간다.
        {
            tetrominoNode.transform.position = oldPos;
            tetrominoNode.transform.rotation = oldRot;

            if((int)moveDir.y == -1 && (int)moveDir.x == 0&& isRotate ==false)
            {
                AddToBoard(tetrominoNode);
                CheckBoardColumn();
                CreateTetromino();
            }

            return false;
        }

        return true;
    }

    private void AddToBoard(Transform root)
    {
        while (root.childCount > 0)     // 테트로미노의 모든 자식 타일을 가져오는 코드
        {
           var node = root.GetChild(0); // childCount가 바뀐다는 뜻으로 모든 자식을 이동시킬 수 없음.
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);        // 좌표
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);   // 변경

            node.parent = boardNode.Find(y.ToString()); // y 좌표로 보드 노드에 있는 행 노드를 찾아와서 그 자식으로 만들어 줍니다.
            node.name =x.ToString();                    
        }
    }

    private void CheckBoardColumn()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  이동 가능한지 체크 ,true 가능 / false 불가능
    /// </summary>
    /// <param name="root">root에 붙어 있는 i번째 자식을 가져오는 것</param>
    /// <returns></returns>
    bool CanMoveTo(Transform root)
    {
        for(int i = 0; i< root.childCount; ++i) // 자식들에 붙어 있는 모든 타일들
        {
            var node = root.GetChild(i);    // var는 Transform과 같은 의미
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);

            if (x < 0 || x > boardWidth - 1)
         //변환한 좌표가 안전한 위치인지 확인하는 코드입니다. 0 ~ boardWidth -1까지가 안전한 위치이고 그렇지 않으면 이동이 불가능한 영역
                return false;

            if (y < 0)
         //기본적으로 테트로미노는 아래로만 이동이 가능하므로 아래쪽 0 보다 작으면 이동이 불가능한 영역
                return false;

            var column = boardNode.Find(y.ToString());  // 현재 y위치에 해당하는 행 노드를 가져온다.

            if (column != null && column.Find(x.ToString()) != null) // 추가된 타일의 이름은 x 값으로 만들어질예정
                return false;
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
