using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;


    public class path
    {
        public List<Vector3> waypoints = new List<Vector3>();
        public  bool looped;
        public int next;

        public path(){
        looped = false;
            next =0;
        }

        public Vector3 NextWaypoint()
        
        {
            if(next < waypoints.Count)
            {
                return waypoints[next];
            }else{return Vector3.zero;}
        }

        public void Advance()
        {
            if(looped)
            {
                next = (next + 1) % waypoints.Count;
            }else{
                if(next < (waypoints.Count -1))
                {
                    next++;
                }
            }
        }

        public bool IsLast()
        {
            return((!looped)&&(next != waypoints.Count-1));
        }

    }

