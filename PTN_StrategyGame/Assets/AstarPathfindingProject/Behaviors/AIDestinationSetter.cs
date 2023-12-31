﻿using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		IAstarAI ai;
        [SerializeField] private Animator anim;
        public bool isMovingToTarget = false;

        void OnEnable () {
            ai = GetComponent<IAstarAI>();
            // Update the destination right before searching for a path as well.
            // This is enough in theory, but this script will also update the destination every
            // frame as the destination is used for debugging and may be used for other things by other
            // scripts as well. So it makes sense that it is up to date every frame.
            if (ai != null) ai.onSearchPath += Update;
		}


        void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

        private float stopDistance = 0.3f; // The distance at which the AI should stop

        public void SetMovePosition(Transform movePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Soldier")) // Sadece Soldier etiketli nesneleri kontrol et
            {
                this.target = movePosition;
                isMovingToTarget = true;

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 stopPosition = target.position - dirToTarget * stopDistance;

                ai.destination = stopPosition;
            }
            else
            {
                if (ai != null && movePosition != null)
                {
                    this.target = movePosition;
                    isMovingToTarget = true;

                    ai.destination = target.position;
                }
            }
        }
    
        /// <summary>Updates the AI's destination every frame</summary>
        private void Update()
        {
            if(!ai.reachedDestination && !ai.reachedEndOfPath && ai.remainingDistance.ToString() != "Infinity")
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);

            }
        }
    }
}
