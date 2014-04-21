
using System;
using UnityEngine;

public class ChopFirewoodAction : GoapAction
{
	private bool chopped = false;
	private ChoppingBlockComponent targetChoppingBlock; // where we chop the firewood
	
	private float startTime = 0;
	public float workDuration = 2; // seconds
	
	public ChopFirewoodAction () {
		addPrecondition ("hasTool", true); // we need a tool to do this
		addPrecondition ("hasFirewood", false); // if we have firewood we don't want more
		addEffect ("hasFirewood", true);
	}
	
	
	public override void reset ()
	{
		chopped = false;
		targetChoppingBlock = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return chopped;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a chopping block
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest chopping block that we can chop our wood at
		ChoppingBlockComponent[] blocks = (ChoppingBlockComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(ChoppingBlockComponent) );
		ChoppingBlockComponent closest = null;
		float closestDist = 0;
		
		foreach (ChoppingBlockComponent block in blocks) {
			if (closest == null) {
				// first one, so choose it for now
				closest = block;
				closestDist = (block.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (block.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = block;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetChoppingBlock = closest;
		target = targetChoppingBlock.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished chopping
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numFirewood += 5;
			chopped = true;
			ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;
			tool.use(0.34f);
			if (tool.destroyed()) {
				Destroy(backpack.tool);
				backpack.tool = null;
			}
		}
		return true;
	}
	
}

