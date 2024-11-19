using UnityEngine;

public class ConeCastExample : MonoBehaviour {

    public float radius;
    public float depth;
    public float angle;
    public LayerMask mask;

    private Physics physics;

	void FixedUpdate () {

        //RaycastHit[] coneHits = physics.ConeCastAll(transform.position, radius, transform.forward, depth, angle);
        RaycastHit[] coneHits = physics.ConeCastAll(transform.position, radius, transform.forward, depth, angle, mask);

        if (coneHits.Length > 0)
        {
            for (int i = 0; i < coneHits.Length; i++)
            {
                //do something with collider information
                coneHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
            }
        }
	}
}
