using UnityEngine;
using System.Collections;

/**
 * A tool used for mining ore and chopping wood.
 * Tools have strength that gets used up each time
 * they are used. When their strength is depleted
 * they should be destroyed by the user.
 */
public class ToolComponent : MonoBehaviour
{

	public float strength; // [0..1] or 0% to 100%

	void Start ()
	{
		strength = 1; // full strength
	}

	/**
	 * Use up the tool by causing damage. Damage should be a percent
	 * from 0 to 1, where 1 is 100%.
	 */
	public void use(float damage) {
		strength -= damage;
	}

	public bool destroyed() {
		return strength <= 0f;
	}
}

