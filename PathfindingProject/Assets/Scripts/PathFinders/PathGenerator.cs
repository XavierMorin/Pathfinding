using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathGenerator : MonoBehaviour
{
    private PathFinder pathFinder;
    private Map map;
    private Bug bug;
    public Gradient gradient;
    private Dropdown dropdown;
   

    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        bug = GameObject.Find("Bug").GetComponent<Bug>();
        dropdown = GetComponent<Dropdown>();

        DropDownChanged();
    }
   

    public void DropDownChanged()
    {
        int option=dropdown.value;
        switch (option)
        {
            case 0: pathFinder = new AStar();       break;
            case 1: pathFinder = new DFS();         break;
            case 2: pathFinder = new BFS();         break;
            case 3: pathFinder = new Dijkstra();    break;

        }

    }
    public void FindPath()
    {       
        List<Vector2Int> path = pathFinder.FindPath(map.origin, map.destination);
        List<double[,]> megaPath = pathFinder.GetSnapShots();
        StartCoroutine(drawMegaPath(megaPath, path));
    }


    IEnumerator drawMegaPath(List<double[,]> megaPaths, List<Vector2Int> path)
    {
        if (megaPaths.Count > 0)
        {
            int i = 0;
            double[,] newMegaPath;
            while (i < megaPaths.Count)
            {
                newMegaPath = Clamp(megaPaths[i]);
                map.ColorGradeMap(newMegaPath, gradient);
                i += 1;
                yield return new WaitForSeconds(0.005f);
            }
            newMegaPath = Clamp(megaPaths[megaPaths.Count - 1]);
            map.ColorGradeMap(newMegaPath, gradient);
        }
        if(path == null || path.Count ==0)
            map.Clean();
        else
        {
            bug.SetPath(path);
            bug.Go(map.origin);
        }
        
    }

    private double[,] Clamp(double[,] list)
    {
        double[,] newList = new double[map.Columns, map.Rows];
        double max = Max(ref list, map.Rows, map.Columns);
        if (max <= 1)
            return list;
        for (int i = 0; i < map.Columns; i++)
        {
            for (int j = 0; j < map.Rows; j++)
            {
                if (list[i, j] != -1) 
                    newList[i, j]= list[i,j]/max;
            }
        }
        return newList;

    }
    private double Max(ref double[,] list, int height, int width)
    {
        double max = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (list[i, j] > max)
                    max = list[i, j];
            }
        }
        return max;


    }

   
        
    

}

