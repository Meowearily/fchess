using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    //указатель на клетку под курсором
    public GameObject tileHighlightPrefab;
    private GameObject tileHighlight;

    // Start is called before the first frame update
    //получает номер клетки, превращает в точку и создает из префаба игровой объект
    void Start()
    {
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    //позволяет снова включать компонент при выборе другой фигуры
    public void EnterState()
    {
        enabled = true;
    }

    // Update is called once per frame
    //махинации с лучом
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position =
                Geometry.PointFromGrid(gridPoint);

            //проверка нажата ли клавиша
            if (Input.GetMouseButtonDown(0))
            {
                GameObject selectedPiece =
                    GameManager.instance.PieceAtGrid(gridPoint);
                if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                {
                    GameManager.instance.SelectPiece(selectedPiece);

                    ExitState(selectedPiece);
                }
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    //скрывает оверлей клетки и отключает TileSelector
    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        move.EnterState(movingPiece);
    }
}
