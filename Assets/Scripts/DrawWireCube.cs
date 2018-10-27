#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Climbing{
		public class DrawWireCube : MonoBehaviour {

		public List<IKPositions> ikPos = new List<IKPositions>();
		public bool refresh;
	
		// Update is called once per frame
		void Update () {
			if(refresh){
				ikPos.Clear();
				refresh = false;
			}
		
		}

	}
}

#endif