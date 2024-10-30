using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    //выделение мышью, оверлей клеток, экземпл€р клетки выделени€ и фигура, выбранна€ на предыдущем этапе
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;

    //список значений GridPoint
    private List<Vector2Int> moveLocations;
    //список клеток-оверлеев, клеток, доступных дл€ хода
    private List<GameObject> locationsHighlights;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    //сохран€ет перемещаемую фигуру и включает себ€
    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        this.enabled = true;

        //получение списка допустимых позиций и создание пустого списка
        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationsHighlights = new List<GameObject>();

        //обход позиций в списке
        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                //оверлей нападени€ дл€ противника
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc),
                    Quaternion.identity, gameObject.transform);
            }
            else
            {
                //оверлей хода дл€ текущего игрока
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc),
                    Quaternion.identity, gameObject.transform);
            }
            locationsHighlights.Add(highlight);
        }
    }

    // Update is called once per frame
    //снова махинации с лучом
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
                //проверка на допустимость хода
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                {
                    GameManager.instance.Move(movingPiece, gridPoint);
                }

                //вз€тие фигуры
                else
                {
                    GameManager.instance.CapturePieceAt(gridPoint);
                    GameManager.instance.Move(movingPiece, gridPoint);
                }

                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    //сбрасывает и подготовливает к следующему ходу
    private void ExitState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();

        GameManager.instance.NextPlayer();

        selector.EnterState();

        foreach (GameObject highlight in locationsHighlights)
        {
            //игрок выбрал ход, происходит удаление объектов оверлеев
            Destroy(highlight);
        }
    }
}
