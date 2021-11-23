using UnityEngine;
using UnityEngine.UI;

public enum Select {Origin,Destination,Wall,Floor}
public class Selection : MonoBehaviour
{
    // Start is called before the first frame update
    private Map map;
    public Button [] buttons;
    private bool mouseIsDown = false;
    private Select select = Select.Origin;
    private GridLayout gridLayout;
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        gridLayout = GetComponent<GridLayout>();
        buttons[(int)this.select].interactable = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
            mouseIsDown = false;
        
        if (Input.GetMouseButtonDown(0))
            mouseIsDown = true;

        if (mouseIsDown)
            MouseDown();
    }

    public void Clicked(int newSelect)
    {
        buttons[(int)select].interactable = true;
        buttons[newSelect].interactable = false;
        select = (Select)newSelect;
    }
    public void MouseDown()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosition = new Vector3(MousePosition.x + 10f, MousePosition.y + 10f, 0);
        Vector3Int position = gridLayout.WorldToCell(MousePosition);
        Vector2Int position2Int = new Vector2Int(position.x, position.y);
        
        if (map.IsPositionValid(position2Int))
        {
            switch (select)
            {
                case Select.Origin:
                    map.SetOrigin(position2Int); break;
                case Select.Destination:
                    map.SetDestination(position2Int); break;
                case Select.Wall:
                    map.SetWall(position2Int); break;
                case Select.Floor:
                    map.SetFloor(position2Int); break;
            }          
        }
    }
    

}

