using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFiller : MonoBehaviour
{
    public CubeGrid grid;
    private int Distance;
    public GameObject Cube;
    public GameObject GridManager;
    // Start is called before the first frame update
    void Start()
    {
        Cube = GameObject.Find("(0, 0, 0)");
        grid = GameObject.Find("Grid").GetComponent<CubeGrid>();
        Distance = grid.Distance;
        GridManager = GameObject.Find("Grid");
        StartCoroutine(Fill());
    }

    private IEnumerator Fill()
    {
        for(int i = 0; i < grid.GridSize; i++)
        {
            var prefabUp = Instantiate(Cube, new Vector3(transform.position.x, transform.position.y + (Distance * (i + 1)), transform.position.z), Quaternion.identity);
            var prefabDown = Instantiate(Cube, new Vector3(transform.position.x, transform.position.y - (Distance * (i + 1)), transform.position.z), Quaternion.identity);
            grid.Naming(prefabUp);
            grid.Naming(prefabDown);
            prefabUp.transform.parent = GridManager.transform;
            prefabDown.transform.parent = GridManager.transform;
            grid.AddObj(prefabUp);
            grid.AddObj(prefabDown);
            yield return new WaitForSeconds(.1f);
        }
    }
}
