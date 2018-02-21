using UnityEngine;

namespace PhysicsLibrary
{
    public static class PhysicsCalculations
    {
        /// <summary>
        /// Cheat and change the velocity of the ball so that it hits the target and flies at a give maximum height
        /// Works given assumption that table is at height 0!
        /// </summary>
        /// <param name="target">Target landing position</param>
        /// <param name="currPos">Current starting position of the trajectory</param>
        /// <param name="maxHeightTarget">Maximum height during the flight</param>
        /// <param name="g">value of gravity</param>
        /// <returns></returns>
        public static Vector3 velFromTraj(Vector3 target, Vector3 currPos, float maxHeightTarget, float g)
        {
			float h0 = currPos.y-target.y+0.01f;
			float H = Mathf.Max(maxHeightTarget - target.y, h0);
			Debug.Log (h0.ToString ());
			Debug.Log (H.ToString ());

            Vector3 dist = currPos - target;
            dist.y = 0;
            float l = dist.magnitude; // distance to target

            float v_y = Mathf.Sqrt((H - h0) * 2 * g);
            float v_x = l * g / (v_y + Mathf.Sqrt(2 * g * H));

            Vector3 vel = target - currPos;
            vel.y = 0;
            vel = vel.normalized;
            vel.y = v_y;
            vel.x *= v_x;
            vel.z *= v_x;
            return vel;
        }

		/**
		 * @param: batNorm -> normalised normal of the bat surface
		 */
		public static Vector3 batHit(Vector3 ballVel, Vector3 batVel, Vector3 batNorm)
		{
/*			Vector3 v_ball_primed = ballVel - batVel;
			batNorm = batNorm.normalized;
			Vector3 vel = v_ball_primed - 2 * Vector3.Dot (v_ball_primed, batNorm) * batNorm;
			vel += batVel;*/
			Vector3 vel = batVel + 2 * batNorm;
		//	vel.y *= 2;
			return vel;
		}

		public static Vector3 aboveNet(){
			return Vector3.zero;
		}
    }
}
