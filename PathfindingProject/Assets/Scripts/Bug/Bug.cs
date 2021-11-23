using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed=1;
    private Animator animator;
    private List<Vector2Int> path;
    private Vector3 destination;
    private Vector2Int currentDirection = Vector2Int.up;
    private bool isCrawling = false;
    private int id = 0;
    private Map map;
    private float timer = 0;
    public float intervalFootStep = 1;
    public GameObject footStep_prefab;
    void Start()
    {
        animator = GetComponent<Animator>();
        map = GameObject.Find("Map").GetComponent<Map>();
    }

    // Update is called once per frame
    public void SetPath(List<Vector2Int> path)
    {
        this.path = path;
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void Go(Vector2Int origin)
    {
        animator.SetBool("isWalking", true);
        GoIEnumarator(origin);
       

    }
   
    IEnumerator Rotate()
    {
        if (currentDirection != Vector2Int.zero)
        {

            Vector3 newAngle = new Vector3();

            if (currentDirection.y > 0)
                newAngle = new Vector3(0, 0, 0);
            if (currentDirection.x < 0)
                newAngle = new Vector3(0, 0, 90);
            if (currentDirection.y < 0)
                newAngle = new Vector3(0, 0, 180);
            if (currentDirection.x > 0)
                newAngle = new Vector3(0, 0, 270);    

            Vector3 oldAngle = transform.eulerAngles;
            
            //if(newAngle.z > oldAngle.z || oldAngle.z==270f && newAngle.z==0)
            //    transform.eulerAngles = oldAngle + new Vector3(0,0,45f);
            //else
            //    transform.eulerAngles = oldAngle - new Vector3(0, 0, 45f);

            yield return new WaitForSeconds(0.08f);


            transform.eulerAngles = newAngle;
        }



    }
    private void GoIEnumarator(Vector2Int origin)
    {
        Vector2Int lastPosition = origin;
        Vector2Int destination = path[0];
        Vector2Int currentDirection = destination - lastPosition;
       
        Vector2Int newDirection;
        List<Vector2Int> smallerPath = new List<Vector2Int>(); 
        
        for (int i=0; i < path.Count; i++)
        {      
            newDirection = path[i] - lastPosition;

            if (newDirection == currentDirection)
            {
                lastPosition = path[i];
                continue;
            }
            else
            {
                smallerPath.Add(lastPosition);
                currentDirection = newDirection;
            }
            lastPosition = path[i]; 

        }
        smallerPath.Add(path[path.Count - 1]);
       
        path = smallerPath;
        this.currentDirection = path[0] - origin;
        this.destination = map.CellToWorld(path[0]);
        isCrawling = true;
        id = 0;
      
        StartCoroutine(Rotate());



    }
   

    private void Update()
    {
        if (isCrawling)
        {
            timer += Time.deltaTime;
            if (timer > intervalFootStep)
            {
                timer = 0;
                GameObject go =GameObject.Instantiate(footStep_prefab);
                go.transform.position = transform.position;
                go.transform.eulerAngles = transform.eulerAngles;

            }


            Vector3 jump = Vector3.Normalize(new Vector3(currentDirection.x, currentDirection.y, 0));
            jump = speed * Time.deltaTime * jump; 
            transform.position=transform.position + jump;
           
            if (Vector3.Distance(destination,transform.position)<0.5 || HasMissedDestination())
            {
                transform.position = destination;
                id++;

                if (id >= path.Count)
                {
                    isCrawling = false;
                    animator.SetBool("isWalking", false);
                    map.SetOrigin(map.WorldToCell(transform.position));
                    map.Clean();
                    int x = Random.Range(0, map.Columns);
                    while(x == map.WorldToCell(transform.position).x)
                        x = Random.Range(0, map.Columns);

                    int y = Random.Range(0, map.Rows);
                    map.SetDestination(new Vector2Int(x,y));
                    return;
                }
                destination = map.CellToWorld(path[id]);
                currentDirection = path[id] - path[id - 1];
                StartCoroutine(Rotate());


            }

           

        }
    }
    private bool HasMissedDestination()
    {

        if (currentDirection.x > 0)
            if (transform.position.x > destination.x)
                return true;
        if (currentDirection.x < 0)
            if (transform.position.x < destination.x)
                return true;
        if (currentDirection.y > 0)
            if (transform.position.y > destination.y)
                return true;
        if (currentDirection.y < 0)
            if (transform.position.y < destination.y)
                return true;

        return false;


    }

}
