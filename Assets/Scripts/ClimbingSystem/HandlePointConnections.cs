#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Climbing{
	[ExecuteInEditMode]
	public class HandlePointConnections : MonoBehaviour {

		public float minDistance = 2.5f;
		public float directThreshold = 1;
		public bool updateConnections;
		public bool resetConnections; 
		List<Point> allPoints = new List<Point>();
		Vector3[] availableDirections = new Vector3[8];

		void CreateDirections() {
			availableDirections[0] = new Vector3(1, 0, 0);
			availableDirections[1] = new Vector3(-1, 0, 0);
			availableDirections[2] = new Vector3(0, 1, 0);
			availableDirections[3] = new Vector3(0, -1, 0);
			availableDirections[4] = new Vector3(-1, -1, 0);
			availableDirections[5] = new Vector3(1, 1, 0);
			availableDirections[6] = new Vector3(1, -1, 0);
			availableDirections[7] = new Vector3(-1, 1, 0);
			

		}
		
		// Update is called once per frame
		void Update () {
			if(updateConnections){
				GetPoints();
				CreateDirections();
				CreateConnections();
				FindDismountCandidates();
				RefreshAll();
				updateConnections = false;
			}

			if(resetConnections){
				GetPoints();
				for(int p = 0; p < allPoints.Count; p++){
					allPoints[p].neighbours.Clear();
				}
				RefreshAll();
				resetConnections = false;

			}
		}

		void GetPoints(){
			allPoints.Clear();
			Point[] hp = GetComponentsInChildren<Point>();
			allPoints.AddRange(hp);
		}

		void CreateConnections(){
			for(int p = 0; p < allPoints.Count; p++){
				Point curPoint = allPoints[p];
				for(int d = 0; d < availableDirections.Length; d++){
					List<Point> candidatePoints = CandidatePointsOnDirection(availableDirections[d], curPoint);
					Point closest = ReturnClosest(candidatePoints, curPoint);
					if(closest != null){
						if(Vector3.Distance(curPoint.transform.position, closest.transform.position) < minDistance)
						{
							if(Mathf.Abs(availableDirections[d].y) > 0 && Mathf.Abs(availableDirections[d].x) > 0)
							{
								if(Vector3.Distance(curPoint.transform.position, closest.transform.position) > directThreshold){
									continue;
								}
							}

							AddNeighbour(curPoint, closest, availableDirections[d]);
						}
					}
				}
			}
		}

		Point ReturnClosest(List<Point> l, Point from)
		{
			Point retVal = null;
			float minDist = Mathf.Infinity;
			for(int i = 0; i < l.Count; i++){
				float tempDist = Vector3.Distance(l[i].transform.position, from.transform.position);
				if(tempDist < minDist && l[i] != from)
				{
					minDist = tempDist;
					retVal = l[i];
				}
			}
			return retVal;
		}
		List<Point> CandidatePointsOnDirection(Vector3 targetDirection, Point from)
		{
			List<Point> retVal = new List<Point>();

			for(int p = 0; p < allPoints.Count; p++){
				Point targetPoint = allPoints[p];
				Vector3 direction = targetPoint.transform.position - from.transform.position;
				Vector3 relativeDirection = from.transform.InverseTransformDirection(direction);

				if(IsDirectionValid(targetDirection, relativeDirection)){
					retVal.Add(targetPoint);
				}
			}
			return retVal;
		}
		bool IsDirectionValid(Vector3 targetDirection, Vector3 candidate)
		{
			bool retVal = false;
			float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.y) * Mathf.Rad2Deg;
			float angle = Mathf.Atan2(candidate.x, candidate.y) * Mathf.Rad2Deg;

			if(angle < targetAngle + 22.5f && angle > targetAngle - 22.5f)
			{
				retVal = true;
			}
			return retVal;
		}
	}
}

#endif