using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NeigbourCubes : MonoBehaviour
{
    [Header("A* properties")]
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public int EnemyMoveToValue = 0;
    public float EnemyTargetValue = 0f;

    [Serializable]
    public struct Neighbours
    {
        /*
         * Once we get to the objects on edges and vertices, we use the following naming method:
         * 1st letter is comes from forwards/backwards, 2nd letter comes from up/down, and 3rd letter comes from left/right
         * Example, the cube that is on the edge of the forward and right faces is FR
         * whereas the cube on the vertex of the backwards, up, and left face is BUR
         */
        [Header("Neigbours On Faces")]
        public GameObject forwardNeigbour;
        public GameObject backwardNeigbour;
        public GameObject leftNeigbour;
        public GameObject rightNeigbour;
        public GameObject upNeigbour;
        public GameObject downNeigbour;
        [Header("Neighbours on edges")]
        public GameObject FU;
        public GameObject FD;
        public GameObject FL;
        public GameObject FR;
        public GameObject BU;
        public GameObject BD;
        public GameObject BL;
        public GameObject BR;
        public GameObject UL;
        public GameObject UR;
        public GameObject DL;
        public GameObject DR;
        [Header("Neighbours on vertices")]
        public GameObject FDR;
        public GameObject FDL;
        public GameObject FUL;
        public GameObject FUR;
        public GameObject BDR;
        public GameObject BDL;
        public GameObject BUL;
        public GameObject BUR;
        [Header("Indicator of inpassable point")]
        public bool ObstacleFilled;
        public bool PlayerPartFilled;
        public bool PlayerHull;
        public bool EnemyPartFilled;

        public GameObject self;
    }
    public Neighbours neighbours;
    private CubeGrid grid;

    private void Start()
    {
        grid = CubeGrid.instance;
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        neighbours.ObstacleFilled = false;
        neighbours.PlayerPartFilled = false;
        neighbours.EnemyPartFilled = false;
        neighbours.PlayerHull = false;
    }

    public Neighbours GetNeigbhours()
    { return neighbours; }
    public Neighbours FindNeigbours()
    {
        int Distance = grid.Distance;
        neighbours.self = this.gameObject;
        float forward = transform.position.z + Distance;
        float backward = transform.position.z - Distance;
        float left = transform.position.x - Distance;
        float right = transform.position.x + Distance;
        float up = transform.position.y + Distance;
        float down = transform.position.y - Distance;
        foreach (GameObject obj in grid.Grid)
        {
            Vector3 Pos = obj.transform.position;

            /*Check the faces*/
            if (Pos == new Vector3(transform.position.x, transform.position.y, forward))
            { neighbours.forwardNeigbour = obj; }
            else if (Pos == new Vector3(transform.position.x, transform.position.y, backward))
            { neighbours.backwardNeigbour = obj; }
            else if (Pos == new Vector3(left, transform.position.y, transform.position.z))
            { neighbours.leftNeigbour = obj; }
            else if (Pos == new Vector3(right, transform.position.y, transform.position.z))
            { neighbours.rightNeigbour = obj; }
            else if (Pos == new Vector3(transform.position.x, up, transform.position.z))
            { neighbours.upNeigbour = obj; }
            else if (Pos == new Vector3(transform.position.x, down, transform.position.z))
            { neighbours.downNeigbour = obj; }
            /*Check the edges*/
            else if (Pos == new Vector3(left, transform.position.y, forward))
            { neighbours.FL = obj; }
            else if (Pos == new Vector3(right, transform.position.y, forward))
            { neighbours.FR = obj; }
            else if (Pos == new Vector3(transform.position.x, up, forward))
            { neighbours.FU = obj; }
            else if (Pos == new Vector3(transform.position.x, down, forward))
            { neighbours.FD = obj; }
            else if (Pos == new Vector3(left, transform.position.y, backward))
            { neighbours.BL = obj; }
            else if (Pos == new Vector3(right, transform.position.y, backward))
            { neighbours.BR = obj; }
            else if (Pos == new Vector3(transform.position.x, up, backward))
            { neighbours.BU = obj; }
            else if (Pos == new Vector3(transform.position.x, down, backward))
            { neighbours.BD = obj; }
            else if (Pos == new Vector3(left, up, transform.position.z))
            { neighbours.UL = obj; }
            else if (Pos == new Vector3(right, up, transform.position.z))
            { neighbours.UR = obj; }
            else if (Pos == new Vector3(left, down, transform.position.z))
            { neighbours.DL = obj; }
            else if (Pos == new Vector3(right, down, transform.position.z))
            { neighbours.DR = obj; }
            /*Now onto the 8 vertices*/
            else if (Pos == new Vector3(right, down, forward))
            { neighbours.FDR = obj; }
            else if (Pos == new Vector3(left, down, forward))
            { neighbours.FDL = obj; }
            else if(Pos == new Vector3(left, up, forward))
            { neighbours.FUL = obj; }
            else if(Pos == new Vector3(right, up, forward))
            { neighbours.FUR = obj; }
            else if(Pos == new Vector3(right, down, backward))
            { neighbours.BDR = obj; }
            else if(Pos == new Vector3(left, down, backward))
            { neighbours.BDL = obj; }
            else if(Pos == new Vector3(left, up, backward))
            { neighbours.BUL = obj; }
            else if(Pos == new Vector3(right, up, backward))
            { neighbours.BUR = obj;}
            /*
            //forward
            Vector3 foward = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            //backward
            Vector3 backward = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
            //left
            Vector3 left = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
            //right
            Vector3 right = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
            //up
            Vector3 up = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            //down
            Vector3 down = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
            

            if(obj.transform.position == foward)
            { neighbours.forwardNeigbour = obj; Forward = true; }
            else if (obj.transform.position == backward)
            { neighbours.backwardNeigbour = obj; Backward = true; }
            else if (obj.transform.position == left)
            { neighbours.leftNeigbour = obj; Left = true; }
            else if (obj.transform.position == right)
            { neighbours.rightNeigbour = obj; Right = true; }
            else if (obj.transform.position == up)
            { neighbours.upNeigbour = obj; Up = true; }
            else if (obj.transform.position == down)
            { neighbours.downNeigbour = obj; Down = true; }
            else if(Forward && Backward && Left && Right && Up && Down)
            { break; }
            */
        }
        return neighbours;
    }

    public void HasObstacle()
    {
        neighbours.ObstacleFilled = true;
    }

    private void ListNeigbours()
    {
        grid.neighbours.Add(FindNeigbours());
    }

    public void EmptyPoint()
    {
        neighbours.EnemyPartFilled = !neighbours.EnemyPartFilled;
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        Debug.Log("Updating enemy bool at " + this.gameObject.name);
        grid.HideGrid(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { neighbours.PlayerHull = true; }

        else if (other.gameObject.CompareTag("Enemy"))
        { neighbours.EnemyPartFilled = true; GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red); }

        else if (other.gameObject.CompareTag("PlayerShipPart"))
        { neighbours.PlayerPartFilled = true; }

        else
        { neighbours.ObstacleFilled = true; GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red); }
    }

    private void OnTriggerExit(Collider other)
    {
        neighbours.ObstacleFilled = false;
        neighbours.PlayerPartFilled = false;
        neighbours.EnemyPartFilled = false;
        neighbours.PlayerHull = false;
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { neighbours.PlayerHull = true; }

        else if (other.gameObject.CompareTag("Enemy"))
        { 
            neighbours.EnemyPartFilled = true; 
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            //Debug.Log(other.name);
            //other.GetComponent<InitRemover>().cube = this.gameObject;
        }

        else if (other.gameObject.CompareTag("PlayerShipPart"))
        { neighbours.PlayerPartFilled = true; }

        else
        { neighbours.ObstacleFilled = true; GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red); }
    }*/
}
