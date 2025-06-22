using UnityEngine;

namespace Slothsoft.Effects.Triggers {
    sealed class Physics3DEffectTrigger : EffectTriggerBase<PhysicsTriggerType> {
        void OnCollisionEnter(Collision collision) {
            Execute(PhysicsTriggerType.OnCollisionEnter, CollisionInfo.FromCollision(collision));
        }

        void OnCollisionStay(Collision collision) {
            Execute(PhysicsTriggerType.OnCollisionStay, CollisionInfo.FromCollision(collision));
        }

        void OnCollisionExit(Collision collision) {
            Execute(PhysicsTriggerType.OnCollisionExit, CollisionInfo.FromCollision(collision));
        }

        void OnTriggerEnter(Collider collider) {
            Execute(PhysicsTriggerType.OnTriggerEnter, CollisionInfo.FromTrigger(gameObject, collider));
        }

        void OnTriggerStay(Collider collider) {
            Execute(PhysicsTriggerType.OnTriggerStay, CollisionInfo.FromTrigger(gameObject, collider));
        }

        void OnTriggerExit(Collider collider) {
            Execute(PhysicsTriggerType.OnTriggerExit, CollisionInfo.FromTrigger(gameObject, collider));
        }
    }
}