
using System;
using UnityEngine;

public class ForgeToolAction : GoapAction
{
	private bool forged = false;
	private ForgeComponent targetForge; // where we forge tools
	
	private float startTime = 0;
	public float forgeDuration = 2; // seconds
	
	public ForgeToolAction () {
		addPrecondition ("hasOre", true);
		addEffect ("hasNewTools", true);
	}
	
	
	public override void reset ()
	{
		forged = false;
		targetForge = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return forged;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a forge
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest forge
		ForgeComponent[] forges = (ForgeComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(ForgeComponent) );
		ForgeComponent closest = null;
		float closestDist = 0;
		
		foreach (ForgeComponent forge in forges) {
			if (closest == null) {
				// first one, so choose it for now
				closest = forge;
				closestDist = (forge.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (forge.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = forge;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetForge = closest;
		target = targetForge.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > forgeDuration) {
			// finished forging a tool
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numOre = 0;
			forged = true;
		}
		return true;
	}
	
}
