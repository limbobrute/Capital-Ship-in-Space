using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This uses the A* searching algorthim.
 */
public class Pathfinding
{
    List<GameObject> Path = new List<GameObject>();
    List<GameObject> NoGo = new List<GameObject>();
    List<GameObject> Option = new List<GameObject>();
    private CubeGrid grid =CubeGrid.instance;
    private GameObject Start;
    private GameObject End;


    public List<GameObject> FindPath(GameObject start, GameObject end)
    {
        if(Path.Count > 0)//Don't want to add onto the last path found in the grid
        { 
            Path.Clear(); 
            NoGo.Clear();
            Option.Clear();
        }
        Check();
        start.GetComponent<NeigbourCubes>().G = 0;
        Start = start;
        End = end;
        Distance(start.GetComponent<NeigbourCubes>().neighbours, end, 0);
        return Path;
    }

    private void Check()
    {
        foreach(GameObject obj in grid.Grid)
        {
            NeigbourCubes node = obj.GetComponent<NeigbourCubes>();
            if(node.neighbours.ObstacleFilled || node.neighbours.EnemyPartFilled || node.neighbours.PlayerHull)
            { NoGo.Add(node.neighbours.self); }
            else
            { Option.Add(node.neighbours.self); }
        }

        foreach(GameObject obj in Option)
        { 
            NeigbourCubes temp = obj.GetComponent<NeigbourCubes>();
            if(temp.neighbours.ObstacleFilled || temp.neighbours.EnemyPartFilled || temp.neighbours.PlayerHull)
            { Debug.Log("YOU FUCKED UP. " + temp.name + " SHOULD NOT BE AN ACCETABLE OPTION!!"); }
        }
    }

    private void Distance(NeigbourCubes.Neighbours start, GameObject end, int thisCubeG)
    {
        //Are we next to the endpoint that is being saught after?
        if (start.forwardNeigbour == end
            || start.backwardNeigbour == end
            || start.leftNeigbour == end
            || start.rightNeigbour == end
            || start.upNeigbour == end
            || start.downNeigbour == end
            || start.FU == end
            || start.FD == end
            || start.FL == end
            || start.FR == end
            || start.BU == end
            || start.BD == end
            || start.BL == end
            || start.BR == end
            || start.UL == end
            || start.UR == end
            || start.DL == end
            || start.DR == end
            || start.FDR == end
            || start.FDL == end
            || start.FUL == end
            || start.FUR == end
            || start.BDR == end
            || start.BDL == end
            || start.BUL == end
            || start.BUR == end)
            
        { Path.Add(end); Debug.Log("Found cube that CameraFollow is on, which is " + end.name); }
        //Let's look for the next closest option within the grid
        else
        {
            #region LOCAL_VARS
            int Fd = 0;
            int Bd = 0;
            int Ld = 0;
            int Rd = 0;
            int Ud = 0;
            int Dd = 0;
            int FUd = 0;
            int FDd = 0;
            int FLd = 0;
            int FRd = 0;
            int BUd = 0;
            int BDd = 0;
            int BLd = 0;
            int BRd = 0;
            int ULd = 0;
            int URd = 0;
            int DLd = 0;
            int DRd = 0;
            int FDRd = 0;
            int FDLd = 0;
            int FULd = 0;
            int FURd = 0;
            int BDRd = 0;
            int BDLd = 0;
            int BULd = 0;
            int BURd = 0;
            #endregion

            #region CHECK_FOR_POINT_ON_GRAPH
            #region FACE_DISTANCE_CHECK
            if (start.forwardNeigbour != null 
            && Option.Contains(start.forwardNeigbour)
            && !Path.Contains(start.forwardNeigbour))
            {
                start.forwardNeigbour.GetComponent<NeigbourCubes>().G = 1 + thisCubeG;
                start.forwardNeigbour.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.forwardNeigbour);
                Fd = start.forwardNeigbour.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front cube F value is " + Fd);
            }
            else//Either the object doesn't exist, or there is something there already. Either way, we can't have it considered as an option
            { Fd = 100000; Debug.Log("Front cube has something already in it, value is infinite."); }

            if (start.backwardNeigbour != null
            && Option.Contains(start.backwardNeigbour)
            && !Path.Contains(start.backwardNeigbour))
            {
                start.backwardNeigbour.GetComponent<NeigbourCubes>().G = 1 + thisCubeG;
                start.backwardNeigbour.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.backwardNeigbour);
                Bd = start.backwardNeigbour.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backside cube F value is " + Bd);
            }
            else
            { Bd = 100000; Debug.Log("Backside cube has something already in it, value is infinite."); }

            if (start.leftNeigbour != null
            && Option.Contains(start.leftNeigbour)
            && !Path.Contains(start.leftNeigbour))
            {
                start.leftNeigbour.GetComponent<NeigbourCubes>().G = 1 + thisCubeG;
                start.leftNeigbour.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.leftNeigbour);
                Ld = start.leftNeigbour.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Leftside cube F value is " + Ld);
            }
            else
            { Ld = 100000; Debug.Log("Leftside cube has something already in it, value is infinite."); }

            if (start.rightNeigbour != null 
            && Option.Contains(start.rightNeigbour)
            && !Path.Contains(start.rightNeigbour))
            {
                start.rightNeigbour.GetComponent<NeigbourCubes>().G = 1 + thisCubeG;
                start.rightNeigbour.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.rightNeigbour);
                Rd = start.rightNeigbour.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Rightside cube F value is " + Rd);
            }
            else
            { Rd = 100000; Debug.Log("Right cube has something already in it, value is infinite."); }

            if (start.upNeigbour != null 
            && Option.Contains(start.upNeigbour)
            && !Path.Contains(start.upNeigbour))
            {
                start.upNeigbour.GetComponent<NeigbourCubes>().G = 1 + thisCubeG;
                start.upNeigbour.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.upNeigbour);
                Ud = start.upNeigbour.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Topside cube F value is " + Ud);
            }
            else
            { Ud = 100000; Debug.Log("Topside cube has something already in it, value is infinite."); }

            if (start.downNeigbour != null 
            && Option.Contains(start.downNeigbour)
            && !Path.Contains(start.downNeigbour))
            {
                start.downNeigbour.GetComponent<NeigbourCubes>().G = 1 + thisCubeG;
                start.downNeigbour.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.downNeigbour);
                Dd = start.downNeigbour.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Bottomside cube F value is " + Dd);
            }
            else
            { Dd = 100000; Debug.Log("Bottomside cube has something already in it, value is infinite."); }
            #endregion
            #region EDGE_DISTANCE_CHECK
            if (start.FU != null 
            && Option.Contains(start.FU)
            && !Path.Contains(start.FU))
            {
                start.FU.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.FU.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FU);
                FUd = start.FU.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/top cube F value is " + FUd);
            }
            else//Either the object doesn't exist, or there is something there already. Either way, we can't have it considered as an option
            { FUd = 100000; Debug.Log("Front/top cube has something already in it, value is infinite."); }

            if (start.FD != null 
            && Option.Contains(start.FD)
            && !Path.Contains(start.FD))
            {
                start.FD.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.FD.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FD);
                FDd = start.FD.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/down cube F value is " + FDd);
            }
            else
            { FDd = 100000; Debug.Log("Front/down cube has something already in it, value is infinite."); }

            if (start.FL != null 
            && Option.Contains(start.FL)
            && !Path.Contains(start.FL))
            {
                start.FL.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.FL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FL);
                FLd = start.FL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/left cube F value is " + FLd);
            }
            else
            { FLd = 100000; Debug.Log("Front/left cube has something already in it, value is infinite."); }

            if (start.FR != null 
            && Option.Contains(start.FR)
            && !Path.Contains(start.FR))
            {
                start.FR.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.FR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FR);
                FRd = start.FR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/right cube F value is " + FRd);
            }
            else
            { FRd = 100000; Debug.Log("Front/right cube has something already in it, value is infinite."); }

            if (start.BU != null 
            && Option.Contains(start.BU)
            && !Path.Contains(start.BU))
            {
                start.BU.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.BU.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BU);
                BUd = start.BU.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backside/top cube F value is " + BUd);
            }
            else
            { BUd = 100000; Debug.Log("Backside/top cube has something already in it, value is infinite."); }

            if (start.BD != null 
            && Option.Contains(start.BD)
            && !Path.Contains(start.BD))
            {
                start.BD.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.BD.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BD);
                BDd = start.BD.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backside/bottom cube F value is " + BDd);
            }
            else
            { BDd = 100000; Debug.Log("Backside/bottom cube has something already in it, value is infinite."); }

            if (start.BL != null 
            && Option.Contains(start.BL)
            && !Path.Contains(start.BL))
            {
                start.BL.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.BL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BL);
                BLd = start.BL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backside/left cube F value is " + BLd);
            }
            else
            { BLd = 100000; Debug.Log("Backside/left cube has something already in it, value is infinite."); }

            if (start.BR != null
            && Option.Contains(start.BR)
            && !Path.Contains(start.BR))
            {
                start.BR.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.BR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BR);
                BRd = start.BR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backside/right cube F value is " + BRd);
            }
            else
            { BRd = 100000; Debug.Log("Backside/right cube has something already in it, value is infinite."); }

            if (start.UL != null 
            && Option.Contains(start.UL)
            && !Path.Contains(start.UL))
            {
                start.UL.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.UL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.UL);
                ULd = start.UL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Top/left cube F value is " + ULd);
            }
            else
            { ULd = 100000; Debug.Log("Top/left cube has something already in it, value is infinite."); }

            if (start.UR != null 
            && Option.Contains(start.UR)
            && !Path.Contains(start.UR))
            {
                start.UR.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.UR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.UR);
                URd = start.UR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Top/right cube F value is " + URd);
            }
            else
            { URd = 100000; Debug.Log("Top/right cube has something already in it, value is infinite."); }

            if (start.DL != null 
            && Option.Contains(start.DL)
            && !Path.Contains(start.DL))
            {
                start.DL.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.DL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.DL);
                DLd = start.DL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Bottom/left cube F value is " + DLd);
            }
            else
            { DLd = 100000; Debug.Log("Bottom/left cube has something already in it, value is infinite."); }

            if (start.DR != null
            && Option.Contains(start.DR)
            && !Path.Contains(start.DR))
            {
                start.DR.GetComponent<NeigbourCubes>().G = 2 + thisCubeG;
                start.DR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.DR);
                DRd = start.DR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Bottom/right cube F value is " + DRd);
            }
            else
            { DRd = 100000; Debug.Log("Bottom/right cube has something already in it, value is infinite."); }
            #endregion
            #region VERTEX_DISTANCE_CHECK
            if (start.FDR != null 
            && Option.Contains(start.FDR)
            && !Path.Contains(start.FDR))
            {
                start.FDR.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.FDR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FDR);
                FDRd = start.FDR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/bottom/right cube F value is " + FDRd);
            }
            else
            { FDRd = 100000; Debug.Log("Front/bottom/right cube has something already in it, value is infinite."); }

            if (start.FDL != null
            && Option.Contains(start.FDL)
            && !Path.Contains(start.FDL))
            {
                start.FDL.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.FDL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FDL);
                FDLd = start.FDL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/bottom/left cube F value is " + FDLd);
            }
            else
            { FDLd = 100000; Debug.Log("Front/bottom/left cube has something already in it, value is infinite."); }

            if (start.FUL != null 
            && Option.Contains(start.FUL)
            && !Path.Contains(start.FUL))
            {
                start.FUL.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.FUL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FUL);
                FULd = start.FUL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/top/left cube F value is " + FULd);
            }
            else
            { FULd = 100000; Debug.Log("Front/left/left cube has something already in it, value is infinite."); }

            if (start.FUR != null
            && Option.Contains(start.FUR)
            && !Path.Contains(start.FUR))
            {
                start.FUR.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.FUR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.FUR);
                FURd = start.FUR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Front/top/right cube F value is " + FURd);
            }
            else
            { FURd = 100000; Debug.Log("Front/top/right cube has something already in it, value is infinite."); }

            if (start.BDR != null 
            && Option.Contains(start.BDR)
            && !Path.Contains(start.BDR))
            {
                start.BDR.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.BDR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BDR);
                BDRd = start.BDR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backwards/bottom/right cube F value is " + BDRd);
            }
            else
            { BDRd = 100000; Debug.Log("Backwards/bottom/right cube has something already in it, value is infinite."); }

            if (start.BDL != null
            && Option.Contains(start.BDL)
            && !Path.Contains(start.BDL))
            {
                start.BDL.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.BDL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BDL);
                BDLd = start.BDL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backwards/bottom/left cube F value is " + BDLd);
            }
            else
            { BDLd = 100000; Debug.Log("Backwards/bottom/left cube has something already in it, value is infinite."); }

            if (start.BUL != null
            && Option.Contains(start.BUL)
            && !Path.Contains(start.BUL))
            {
                start.BUL.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.BUL.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BUL);
                BULd = start.BUL.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backwards/top/left cube F value is " + BULd);
            }
            else
            { BULd = 100000; Debug.Log("Backwards/top/left cube has something already in it, value is infinite."); }

            if (start.BUR != null
            && Option.Contains(start.BUR)
            && !Path.Contains(start.BUR))
            {
                start.BUR.GetComponent<NeigbourCubes>().G = 3 + thisCubeG;
                start.BUR.GetComponent<NeigbourCubes>().H = GetManhattenDistance(end, start.BUR);
                BURd = start.BUR.GetComponent<NeigbourCubes>().F;
                //Debug.Log("Backwards/top/right cube F value is " + BURd);
            }
            else
            { BURd = 100000; Debug.Log("Backwards/top/right cube has something already in it, value is infinite."); }
            #endregion
            #endregion

            #region ADD_POINT_TO_ROUTE
            if(Fd < Bd && Fd < Ld && Fd < Rd && Fd < Ud && Fd < Dd 
               && Fd < FUd && Fd < FDd && Fd < FLd && Fd < FRd && Fd < BUd && Fd < BDd && Fd < BLd && Fd < BRd && Fd < ULd && Fd < URd && Fd < DLd && Fd < DRd
               && Fd < FDRd && Fd < FDLd && Fd < FULd && Fd < FURd && Fd < BDRd && Fd < BDLd && Fd < BULd && Fd < BURd)
            {
                //Debug.Log("Add forward cube, which is at " + start.forwardNeigbour.name);
                Path.Add(start.forwardNeigbour);
                Option.Remove(start.forwardNeigbour);
                Distance(start.forwardNeigbour.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.forwardNeigbour.GetComponent<NeigbourCubes>().G);
            }
            else if (Bd < Fd && Bd < Ld && Bd < Rd && Bd < Ud && Bd < Dd
                     && Bd < FUd && Bd < FDd && Bd < FLd && Bd < FRd && Bd < BUd && Bd < BDd && Bd < BLd && Bd < BRd && Bd < ULd && Bd < URd && Bd < DLd && Bd < DRd
                     && Bd < FDRd && Bd < FDLd && Bd < FULd && Bd < FURd && Bd < BDRd && Bd < BDLd && Bd < BULd && Bd < BURd)
            {
                //Debug.Log("Add backside cube, which is at " + start.backwardNeigbour.name);
                Path.Add(start.backwardNeigbour);
                Option.Remove(start.backwardNeigbour);
                Distance(start.backwardNeigbour.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.backwardNeigbour.GetComponent<NeigbourCubes>().G);
            }
            else if (Ld < Fd && Ld < Bd && Ld < Rd && Ld < Ud && Ld < Dd
                     && Ld < FUd && Ld < FDd && Ld < FLd && Ld < FRd && Ld < BUd && Ld < BDd && Ld < BLd && Ld < BRd && Ld < ULd && Ld < URd && Ld < DLd && Ld < DRd
                     && Ld < FDRd && Ld < FDLd && Ld < FULd && Ld < FURd && Ld < BDRd && Ld < BDLd && Ld < BULd && Ld < BURd)
            {
                //Debug.Log("Add leftside cube, which is at " + start.leftNeigbour.name);
                Path.Add(start.leftNeigbour);
                Option.Remove(start.leftNeigbour);
                Distance(start.leftNeigbour.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.leftNeigbour.GetComponent<NeigbourCubes>().G);
            }
            else if (Rd < Fd && Rd < Ld && Rd < Ld && Rd < Ud && Rd < Dd
                     && Rd < FUd && Rd < FDd && Rd < FLd && Rd < FRd && Rd < BUd && Rd < BDd && Rd < BLd && Rd < BRd && Rd < ULd && Rd < URd && Rd < DLd && Rd < DRd
                     && Rd < FDRd && Rd < FDLd && Rd < FULd && Rd < FURd && Rd < BDRd && Rd < BDLd && Rd < BULd && Rd < BURd)
            {
                //Debug.Log("Add rightside cube, which is at " + start.rightNeigbour.name);
                Path.Add(start.rightNeigbour);
                Option.Remove(start.rightNeigbour);
                Distance(start.rightNeigbour.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.rightNeigbour.GetComponent<NeigbourCubes>().G);
            }
            else if (Ud < Fd && Ud < Ld && Ud < Rd && Ud < Rd && Ud < Dd
                     && Ud < FUd && Ud < FDd && Ud < FLd && Ud < FRd && Ud < BUd && Ud < BDd && Ud < BLd && Ud < BRd && Ud < ULd && Ud < URd && Ud < DLd && Ud < DRd
                     && Ud < FDRd && Ud < FDLd && Ud < FULd && Ud < FURd && Ud < BDRd && Ud < BDLd && Ud < BULd && Ud < BURd)
            {
                //Debug.Log("Add topside cube, which is at " + start.upNeigbour.name);
                Path.Add(start.upNeigbour);
                Option.Remove(start.upNeigbour);
                Distance(start.upNeigbour.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.upNeigbour.GetComponent<NeigbourCubes>().G);
            }
            else if (Dd < Fd && Dd < Ld && Dd < Rd && Dd < Ud && Dd < Bd
                     && Dd < FUd && Dd < FDd && Dd < FLd && Dd < FRd && Dd < BUd && Dd < BDd && Dd < BLd && Dd < BRd && Dd < ULd && Dd < URd && Dd < DLd && Dd < DRd
                     && Dd < FDRd && Dd < FDLd && Dd < FULd && Dd < FURd && Dd < BDRd && Dd < BDLd && Dd < BULd && Dd < BURd)
            {
                //Debug.Log("Add bottomside cube, which is at " + start.downNeigbour.name);
                Path.Add(start.downNeigbour);
                Option.Remove(start.downNeigbour);
                Distance(start.downNeigbour.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.downNeigbour.GetComponent<NeigbourCubes>().G);
            }
            else if (FUd < Fd && FUd < Ld && FUd < Rd && FUd < Ud && FUd < Dd
                     && FUd < Bd && FUd < FDd && FUd < FLd && FUd < FRd && FUd < BUd && FUd < BDd && FUd < BLd && FUd < BRd && FUd < ULd && FUd < URd && FUd < DLd && FUd < DRd
                     && FUd < FDRd && FUd < FDLd && FUd < FULd && FUd < FURd && FUd < BDRd && FUd < BDLd && FUd < BULd && FUd < BURd)
            {
                //Debug.Log("Add Front/top cube, which is at " + start.FU.name);
                Path.Add(start.FU);
                Option.Remove(start.FU);
                Distance(start.FU.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FU.GetComponent<NeigbourCubes>().G);
            }
            else if (FDd < Fd && FDd < Ld && FDd < Rd && FDd < Ud && FDd < Dd
                     && FDd < FUd && FDd < Bd && FDd < FLd && FDd < FRd && FDd < BUd && FDd < BDd && FDd < BLd && FDd < BRd && FDd < ULd && FDd < URd && FDd < DLd && FDd < DRd
                     && FDd < FDRd && FDd < FDLd && FDd < FULd && FDd < FURd && FDd < BDRd && FDd < BDLd && FDd < BULd && FDd < BURd)
            {
                //Debug.Log("Add Front/bottom cube, which is at " + start.FD.name);
                Path.Add(start.FD);
                Option.Remove(start.FD);
                Distance(start.FD.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FD.GetComponent<NeigbourCubes>().G);
            }
            else if (FLd < Fd && FLd < Ld && FLd < Rd && FLd < Ud && FLd < Dd
                     && FLd < FUd && FLd < FDd && FLd < Bd && FLd < FRd && FLd < BUd && FLd < BDd && FLd < BLd && FLd < BRd && FLd < ULd && FLd < URd && FLd < DLd && FLd < DRd
                     && FLd < FDRd && FLd < FDLd && FLd < FULd && FLd < FURd && FLd < BDRd && FLd < BDLd && FLd < BULd && FLd < BURd)
            {
                //Debug.Log("Add Front/left cube, which is at " + start.FL.name);
                Path.Add(start.FL);
                Option.Remove(start.FL);
                Distance(start.FL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FL.GetComponent<NeigbourCubes>().G);
            }
            else if (FRd < Fd && FRd < Ld && FRd < Rd && FRd < Ud && FRd < Dd
                     && FRd < FUd && FRd < FDd && FRd < FLd && FRd < Bd && FRd < BUd && FRd < BDd && FRd < BLd && FRd < BRd && FRd < ULd && FRd < URd && FRd < DLd && FRd < DRd
                     && FRd < FDRd && FRd < FDLd && FRd < FULd && FRd < FURd && FRd < BDRd && FRd < BDLd && FRd < BULd && FRd < BURd)
            {
                //Debug.Log("Add Front/right cube, which is at " + start.FR.name);
                Path.Add(start.FR);
                Option.Remove(start.FR);
                Distance(start.FR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FR.GetComponent<NeigbourCubes>().G);
            }
            else if (BUd < Fd && BUd < Ld && BUd < Rd && BUd < Ud && BUd < Dd
                     && BUd < FUd && BUd < FDd && BUd < FLd && BUd < FRd && BUd < Bd && BUd < BDd && BUd < BLd && BUd < BRd && BUd < ULd && BUd < URd && BUd < DLd && BUd < DRd
                     && BUd < FDRd && BUd < FDLd && BUd < FULd && BUd < FURd && BUd < BDRd && BUd < BDLd && BUd < BULd && BUd < BURd)
            {
                //Debug.Log("Add back/top cube, which is at " + start.BU.name);
                Path.Add(start.BU);
                Option.Remove(start.BU);
                Distance(start.BU.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BU.GetComponent<NeigbourCubes>().G);
            }
            else if (BDd < Fd && BDd < Ld && BDd < Rd && BDd < Ud && BDd < Dd
                     && BDd < FUd && BDd < FDd && BDd < FLd && BDd < FRd && BDd < BUd && BDd < Bd && BDd < BLd && BDd < BRd && BDd < ULd && BDd < URd && BDd < DLd && BDd < DRd
                     && BDd < FDRd && BDd < FDLd && BDd < FULd && BDd < FURd && BDd < BDRd && BDd < BDLd && BDd < BULd && BDd < BURd)
            {
                //Debug.Log("Add back/bottom cube, which is at " + start.BD.name);
                Path.Add(start.BD);
                Option.Remove(start.BD);
                Distance(start.BD.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BD.GetComponent<NeigbourCubes>().G);
            }
            else if (BLd < Fd && BLd < Ld && BLd < Rd && BLd < Ud && BLd < Dd
                     && BLd < FUd && BLd < FDd && BLd < FLd && BLd < FRd && BLd < BUd && BLd < BDd && BLd < Bd && BLd < BRd && BLd < ULd && BLd < URd && BLd < DLd && BLd < DRd
                     && BLd < FDRd && BLd < FDLd && BLd < FULd && BLd < FURd && BLd < BDRd && BLd < BDLd && BLd < BULd && BLd < BURd)
            {
                //Debug.Log("Add Back/left cube, which is at " + start.BL.name);
                Path.Add(start.BL);
                Option.Remove(start.BL);
                Distance(start.BL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BL.GetComponent<NeigbourCubes>().G);
            }
            else if (BRd < Fd && BRd < Ld && BRd < Rd && BRd < Ud && BRd < Dd
                     && BRd < FUd && BRd < FDd && BRd < FLd && BRd < FRd && BRd < BUd && BRd < BDd && BRd < BLd && BRd < Bd && BRd < ULd && BRd < URd && BRd < DLd && BRd < DRd
                     && BRd < FDRd && BRd < FDLd && BRd < FULd && BRd < FURd && BRd < BDRd && BRd < BDLd && BRd < BULd && BRd < BURd)
            {
                //Debug.Log("Add back/right cube, which is at " + start.BR.name);
                Path.Add(start.BR);
                Option.Remove(start.BR);
                Distance(start.BR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BR.GetComponent<NeigbourCubes>().G);
            }
            else if (ULd < Fd && ULd < Ld && ULd < Rd && ULd < Ud && ULd < Dd
                     && ULd < FUd && ULd < FDd && ULd < FLd && ULd < FRd && ULd < BUd && ULd < BDd && ULd < BLd && ULd < BRd && ULd < Bd && ULd < URd && ULd < DLd && ULd < DRd
                     && ULd < FDRd && ULd < FDLd && ULd < FULd && ULd < FURd && ULd < BDRd && ULd < BDLd && ULd < BULd && ULd < BURd)
            {
                //Debug.Log("Add Top/left cube, which is at " + start.UL.name);
                Path.Add(start.UL);
                Option.Remove(start.UL);
                Distance(start.UL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.UL.GetComponent<NeigbourCubes>().G);
            }
            else if (URd < Fd && URd < Ld && URd < Rd && URd < Ud && URd < Dd
                     && URd < FUd && URd < FDd && URd < FLd && URd < FRd && URd < BUd && URd < BDd && URd < BLd && URd < BRd && URd < ULd && URd < Bd && URd < DLd && URd < DRd
                     && URd < FDRd && URd < FDLd && URd < FULd && URd < FURd && URd < BDRd && URd < BDLd && URd < BULd && URd < BURd)
            {
                //Debug.Log("Add Top/right, which is at " + start.UR.name);
                Path.Add(start.UR);
                Option.Remove(start.UR);
                Distance(start.UR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.UR.GetComponent<NeigbourCubes>().G);
            }
            else if (DLd < Fd && DLd < Ld && DLd < Rd && DLd < Ud && DLd < Dd
                     && DLd < FUd && DLd < FDd && DLd < FLd && DLd < FRd && DLd < BUd && DLd < BDd && DLd < BLd && DLd < BRd && DLd < ULd && DLd < URd && DLd < Bd && Bd < DLd
                     && DLd < FDRd && DLd < FDLd && DLd < FULd && DLd < FURd && DLd < BDRd && DLd < BDLd && DLd < BULd && DLd < BURd)
            {
                //Debug.Log("Add Bottom/left cube, which is at " + start.DL.name);
                Path.Add(start.DL);
                Option.Remove(start.DL);
                Distance(start.DL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.DL.GetComponent<NeigbourCubes>().G);
            }
            else if (DRd < Fd && DRd < Ld && DRd < Rd && DRd < Ud && DRd < Dd
                     && DRd < FUd && DRd < FDd && DRd < FLd && DRd < FRd && DRd < BUd && DRd < BDd && DRd < BLd && DRd < BRd && DRd < ULd && DRd < URd && DRd < DLd && DRd < Bd
                     && DRd < FDRd && DRd < FDLd && DRd < FULd && DRd < FURd && DRd < BDRd && DRd < BDLd && DRd < BULd && DRd < BURd)
            {
                //Debug.Log("Add Bottom/right cube, which is at " + start.DR.name);
                Path.Add(start.DR);
                Option.Remove(start.DR);
                Distance(start.DR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.DR.GetComponent<NeigbourCubes>().G);
            }
            else if (FDRd < Fd && FDRd < Ld && FDRd < Rd && FDRd < Ud && FDRd < Dd
                     && FDRd < FUd && FDRd < FDd && FDRd < FLd && FDRd < FRd && FDRd < BUd && FDRd < BDd && FDRd < BLd && FDRd < BRd && FDRd < ULd && FDRd < URd && FDRd < DLd && FDRd < DRd
                     && FDRd < Bd && FDRd < FDLd && FDRd < FULd && FDRd < FURd && FDRd < BDRd && FDRd < BDLd && FDRd < BULd && FDRd < BURd)
            {
                //Debug.Log("Add Front/bottom/right cube, which is at " + start.FDR.name);
                Path.Add(start.FDR);
                Option.Remove(start.FDR);
                Distance(start.FDR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FDR.GetComponent<NeigbourCubes>().G);
            }
            else if (FDLd < Fd && FDLd < Ld && FDLd < Rd && FDLd < Ud && FDLd < Dd
                     && FDLd < FUd && FDLd < FDd && FDLd < FLd && FDLd < FRd && FDLd < BUd && FDLd < BDd && FDLd < BLd && FDLd < BRd && FDLd < ULd && FDLd < URd && FDLd < DLd && FDLd < DRd
                     && FDLd < FDRd && FDLd < Bd && FDLd < FULd && FDLd < FURd && FDLd < BDRd && FDLd < BDLd && FDLd < BULd && FDLd < BURd)
            {
                //Debug.Log("Add Front/bottom/left cube, which is at " + start.FDL.name);
                Path.Add(start.FDL);
                Option.Remove(start.FDL);
                Distance(start.FDL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FDL.GetComponent<NeigbourCubes>().G);
            }
            else if (FULd < Fd && FULd < Ld && FULd < Rd && FULd < Ud && FULd < Dd
                     && FULd < FUd && FULd < FDd && FULd < FLd && FULd < FRd && FULd < BUd && FULd < BDd && FULd < BLd && FULd < BRd && FULd < ULd && FULd < URd && FULd < DLd && FULd < DRd
                     && Bd < FDRd && Bd < FDLd && FULd < Bd && FULd  < FURd && FULd < BDRd && FULd < BDLd && FULd < BULd && FULd < BURd)
            {
                //Debug.Log("Add Front/top/left cube, which is at " + start.FUL.name);
                Path.Add(start.FUL);
                Option.Remove(start.FUL);
                Distance(start.FUL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FUL.GetComponent<NeigbourCubes>().G);
            }
            else if (FURd < Fd && FURd < Ld && FURd < Rd && FURd < Ud && FURd < Dd
                     && FURd < FUd && FURd < FDd && FURd < FLd && FURd < FRd && FURd < BUd && FURd < BDd && FURd < BLd && FURd < BRd && FURd < ULd && FURd < URd && FURd < DLd && FURd < DRd
                     && FURd < FDRd && FURd < FDLd && FURd < FULd && FURd < Bd && FURd < BDRd && FURd < BDLd && FURd < BULd && FURd < BURd)
            {
                //Debug.Log("Add Front/top/right cube, which is at " + start.FUR.name);
                Path.Add(start.FUR);
                Option.Remove(start.FUR);
                Distance(start.FUR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FUR.GetComponent<NeigbourCubes>().G);
            }
            else if (BDRd < Fd && BDRd < Ld && BDRd < Rd && BDRd < Ud && BDRd < Dd
                     && BDRd < FUd && BDRd < FDd && BDRd < FLd && BDRd < FRd && BDRd < BUd && BDRd < BDd && BDRd < BLd && BDRd < BRd && BDRd < ULd && BDRd < URd && BDRd < DLd && BDRd < DRd
                     && BDRd < FDRd && BDRd < FDLd && BDRd < FULd && BDRd < FURd && BDRd < Bd && BDRd < Bd && BDRd < Bd && BDRd < Bd)
            {
                //Debug.Log("Add Back/bottom/right cube, which is at " + start.BDR.name);
                Path.Add(start.BDR);
                Option.Remove(start.BDR);
                Distance(start.BDR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BDR.GetComponent<NeigbourCubes>().G);
            }
            else if (BDLd < Fd && BDLd < Ld && BDLd < Rd && BDLd < Ud && BDLd < Dd
                     && BDLd < FUd && BDLd < FDd && BDLd < FLd && BDLd < FRd && BDLd < BUd && BDLd < BDd && BDLd < BLd && BDLd < BRd && BDLd < ULd && BDLd < URd && BDLd < DLd && BDLd < DRd
                     && BDLd < FDRd && BDLd < FDLd && BDLd < FULd && BDLd < FURd && BDLd < BDRd && BDLd < Bd && BDLd < Bd && BDLd < Bd)
            {
                //Debug.Log("Add Back/bottom/left cube, which is at " + start.BDL.name);
                Path.Add(start.BDL);
                Option.Remove(start.BDL);
                Distance(start.BDL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BDL.GetComponent<NeigbourCubes>().G);
            }
            else if (BULd < Fd && BULd < Ld && BULd < Rd && BULd < Ud && BULd < Dd
                     && BULd < FUd && BULd < FDd && BULd < FLd && BULd < FRd && BULd < BUd && BULd < BDd && BULd < BLd && BULd < BRd && BULd < ULd && BULd < URd && BULd < DLd && BULd < DRd
                     && BULd < FDRd && BULd < FDLd && BULd < FULd && BULd < FURd && BULd < BDRd && BULd < BDLd && BULd < Bd && BULd < BURd)
            {
                //Debug.Log("Add Back/top/left cube, which is at " + start.BUL.name);
                Path.Add(start.BUL);
                Option.Remove(start.BUL);
                Distance(start.BUL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BUL.GetComponent<NeigbourCubes>().G);
            }
            else if (BURd < Fd && BURd < Ld && BURd < Rd && BURd < Ud && BURd < Dd
                    && BURd < FUd && BURd < FDd && BURd < FLd && BURd < FRd && BURd < BUd && BURd < BDd && BURd < BLd && BURd < BRd && BURd < ULd && BURd < URd && BURd < DLd && BURd < DRd
                    && BURd < FDRd && BURd < FDLd && BURd < FULd && BURd < FURd && BURd < BDRd && BURd < BDLd && BURd < BULd && BURd < Bd)
            {
                //Debug.Log("Add Back/top/right cube, which is at " + start.BUR.name);
                Path.Add(start.BUR);
                Option.Remove(start.BUR);
                Distance(start.BUR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BUR.GetComponent<NeigbourCubes>().G);
            }
            else if( FULd == FURd)
            {
                int r = Random.Range(0, 1);
                if(r == 0)
                {
                    //Debug.Log("Add Front/top/left cube, which is at " + start.FUL.name);
                    Path.Add(start.FUL);
                    Option.Remove(start.FUL);
                    Distance(start.FUL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FUL.GetComponent<NeigbourCubes>().G);
                }
                else
                {
                    //Debug.Log("Add Front/top/right cube, which is at " + start.FUR.name);
                    Path.Add(start.FUR);
                    Option.Remove(start.FUR);
                    Distance(start.FUR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FUR.GetComponent<NeigbourCubes>().G);
                }
            }
            else if(BULd == BURd)
            {
                int r = Random.Range(0, 1);
                if (r == 0)
                {
                    //Debug.Log("Add Back/top/left cube, which is at " + start.BUL.name);
                    Path.Add(start.BUL);
                    Option.Remove(start.BUL);
                    Distance(start.BUL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BUL.GetComponent<NeigbourCubes>().G);
                }
                else
                {
                    //Debug.Log("Add Back/top/right cube, which is at " + start.BUR.name);
                    Path.Add(start.BUR);
                    Option.Remove(start.BUR);
                    Distance(start.BUR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BUR.GetComponent<NeigbourCubes>().G);
                }
            }
            else if(FLd == FRd)
            {
                int r = Random.Range(0, 1);
                if(r == 0)
                {
                    //Debug.Log("Add Front/left cube, which is at " + start.FL.name);
                    Path.Add(start.FL);
                    Option.Remove(start.FL);
                    Distance(start.FL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FL.GetComponent<NeigbourCubes>().G);
                }
                else
                {
                    //Debug.Log("Add Front/right cube, which is at " + start.FR.name);
                    Path.Add(start.FR);
                    Option.Remove(start.FR);
                    Distance(start.FR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FR.GetComponent<NeigbourCubes>().G);
                }
            }
            else if(FDLd == FDRd)
            {
                int r = Random.Range(0, 1);
                if (r == 0)
                {
                    //Debug.Log("Add Front/bottom/left cube, which is at " + start.FDL.name);
                    Path.Add(start.FDL);
                    Option.Remove(start.FDL);
                    Distance(start.FDL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FDL.GetComponent<NeigbourCubes>().G);
                }
                else
                {
                    //Debug.Log("Add Front/bottom/right cube, which is at " + start.FDR.name);
                    Path.Add(start.FDR);
                    Option.Remove(start.FDR);
                    Distance(start.FDR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.FDR.GetComponent<NeigbourCubes>().G);
                }
            }
            else if(BDLd == BDRd)
            {
                int r = Random.Range(0, 1);
                if (r == 0)
                {
                    //Debug.Log("Add Back/bottom/left cube, which is at " + start.BDL.name);
                    Path.Add(start.BDL);
                    Option.Remove(start.BDL);
                    Distance(start.BDL.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BDL.GetComponent<NeigbourCubes>().G);
                }
                else
                {
                    //Debug.Log("Add Back/bottom/right cube, which is at " + start.BDR.name);
                    Path.Add(start.BDR);
                    Option.Remove(start.BDR);
                    Distance(start.BDR.GetComponent<NeigbourCubes>().GetNeigbhours(), end, start.BDR.GetComponent<NeigbourCubes>().G);
                }
            }
            else
            { 
                Debug.Log("Congrats, you've somehow broke the pathfinding method. Fix it.");
                //Path.Clear();
                var obj = Path[Path.Count - 1];
                Path.Remove(obj);
                Option.Remove(obj);
                var temp = Path[Path.Count - 1];
                Distance(temp.GetComponent<NeigbourCubes>().GetNeigbhours(), end, temp.GetComponent<NeigbourCubes>().G);
            }
            #endregion

        }
    }

    private int GetManhattenDistance(GameObject obj1, GameObject obj2)
    {
        float temp = Mathf.Abs(obj1.transform.position.x - obj2.transform.position.x) +
                     Mathf.Abs(obj1.transform.position.y - obj2.transform.position.y) +
                     Mathf.Abs(obj1.transform.position.z - obj2.transform.position.z);
        int giveBack = (int)temp;
        return giveBack;
    }
}
