using UnityEngine;

namespace Slothsoft.Effects.Triggers {
    sealed class Physics3DEffectTrigger : EffectTriggerBase<EPhysicsTriggerType> {
        void OnCollisionEnter(Collision collision) {
            Execute(EPhysicsTriggerType.OnCollisionEnter, CollisionInfo.FromCollision(collision));
        }

        void OnCollisionStay(Collision collision) {
            Execute(EPhysicsTriggerType.OnCollisionStay, CollisionInfo.FromCollision(collision));
        }

        void OnCollisionExit(Collision collision) {
            Execute(EPhysicsTriggerType.OnCollisionExit, CollisionInfo.FromCollision(collision));
        }

        void OnTriggerEnter(Collider collider) {
            Execute(EPhysicsTriggerType.OnTriggerEnter, CollisionInfo.FromTrigger(gameObject, collider));
        }

        void OnTriggerStay(Collider collider) {
            Execute(EPhysicsTriggerType.OnTriggerStay, CollisionInfo.FromTrigger(gameObject, collider));
        }

        void OnTriggerExit(Collider collider) {
            Execute(EPhysicsTriggerType.OnTriggerExit, CollisionInfo.FromTrigger(gameObject, collider));
        }
    }
}