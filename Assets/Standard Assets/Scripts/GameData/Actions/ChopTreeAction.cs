
using System;
using UnityEngine;

public class ChopTreeAction : GoapAction
{
	private bool chopped = false;
	private TreeComponent targetTree; // where we get the logs from
	
	private float startTime = 0;
	public float workDuration = 2; // seconds
	
	public ChopTreeAction () {
		addPrecondition ("hasTool", true); // we need a tool to do this
		addPrecondition ("hasLogs", false); // if we have logs we don't want more
		addEffect ("hasLogs", true);
	}
	
	
	public override void reset ()
	{
		chopped = false;
		targetTree = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return chopped;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a tree
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest tree that we can chop
		TreeComponent[] trees = (TreeComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(TreeComponent) );
		TreeComponent closest = null;
		float closestDist = 0;
		
		foreach (TreeComponent tree in trees) {
			if (closest == null) {
				// first one, so choose it for now
				closest = tree;
				closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = tree;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetTree = closest;
		target = targetTree.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished chopping
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numLogs += 1;
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