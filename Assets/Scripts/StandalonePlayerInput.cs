using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITank
{
	public class StandalonePlayerInput : MonoBehaviour
	{

		public Tank target;

		private void Update()
		{
			var h = Input.GetAxis("Horizontal1");
			var v = Input.GetAxis("Vertical1");
			target.SetMove(v);
			target.SetRotate(h);
			if (Input.GetKey(KeyCode.Space)) target.Shoot();
		}
	}
}
