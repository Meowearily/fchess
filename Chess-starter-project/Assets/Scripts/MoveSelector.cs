using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    //��������� �����, ������� ������, ��������� ������ ��������� � ������, ��������� �� ���������� �����
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;

    //������ �������� GridPoint
    private List<Vector2Int> moveLocations;
    //������ ������-��������, ������, ��������� ��� ����
    private List<GameObject> locationsHighlights;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    //��������� ������������ ������ � �������� ����
    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        this.enabled = true;

        //��������� ������ ���������� ������� � �������� ������� ������
        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationsHighlights = new List<GameObject>();

        //����� ������� � ������
        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                //������� ��������� ��� ����������
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc),
                    Quaternion.identity, gameObject.transform);
            }
            else
            {
                //������� ���� ��� �������� ������
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc),
                    Quaternion.identity, gameObject.transform);
            }
            locationsHighlights.Add(highlight);
        }
    }

    // Update is called once per frame
    //����� ��������� � �����
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
            if (Input.GetMouseButtonDown(0))
            {
                //�������� �� ������������ ����
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                {
                    GameManager.instance.Move(movingPiece, gridPoint);
                }

                //3

                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    //���������� � �������������� � ���������� ����
    private void ExitState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();

        foreach (GameObject highlight in locationsHighlights)
        {
            //����� ������ ���, ���������� �������� �������� ��������
            Destroy(highlight);
        }
    }
}
