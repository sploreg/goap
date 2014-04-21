
using System;
using UnityEngine;

public class MineOreAction : GoapAction
{
	private bool mined = false;
	private IronRockComponent targetRock; // where we get the ore from

	private float startTime = 0;
	public float miningDuration = 2; // seconds

	public MineOreAction () {
		addPrecondition ("hasTool", true); // we need a tool to do this
		addPrecondition ("hasOre", false); // if we have ore we don't want more
		addEffect ("hasOre", true);
	}
	
	
	public override void reset ()
	{
		mined = false;
		targetRock = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return mined;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a rock
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest rock that we can mine
		IronRockComponent[] rocks = FindObjectsOfType ( typeof(IronRockComponent) ) as IronRockComponent[];
		IronRockComponent closest = null;
		float closestDist = 0;
		
		foreach (IronRockComponent rock in rocks) {
			if (closest == null) {
				// first one, so choose it for now
				closest = rock;
				closestDist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = rock;
					closestDist = dist;
				}
			}
		}
		targetRock = closest;
		target = targetRock.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > miningDuration) {
			// finished mining
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numOre += 2;
			mined = true;
			ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;
			tool.use(0.5f);
			if (tool.destroyed()) {
				Destroy(backpack.tool);
				backpack.tool = null;
			}
		}
		return true;
	}
	
}


