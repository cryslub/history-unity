﻿// Code from https://forum.unity.com/threads/easy-curved-line-renderer-free-utility.391219/

using UnityEngine;
using System.Collections;

public class CurvedLinePoint : MonoBehaviour 
{
	[HideInInspector] public bool showGizmo = false;
	[HideInInspector] public float gizmoSize = 0.1f;
	[HideInInspector] public Color gizmoColor = new Color(1,0,0,0.5f);

	void OnDrawGizmos()
	{
//		if( showGizmo == true )
//		{
//			Gizmos.color = gizmoColor;

	//		Gizmos.DrawSphere( this.transform.position, gizmoSize );
//		}
	}

	//update parent line when this point moved
	void OnDrawGizmosSelected()
	{
		CurvedLineRenderer curvedLine = this.transform.parent.GetComponent<CurvedLineRenderer>();

		if( curvedLine != null )
		{
			//curvedLine.LateUpdate();
		}
	}
}
