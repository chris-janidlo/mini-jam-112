using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace mj112
{
    public class Holeworm : Echoable, IClockFollower
    {
        [Tooltip("In units per Clock-second")]
        public float MoveSpeed;

        [Tooltip("In degress per Clock-second")]
        public float TurnSpeed;

        public Rigidbody2D Rigidbody;

        float horizontalInput;

        void Start ()
        {
            Clock.Instance.Register(this);
        }

        void OnDestroy ()
        {
            Clock.Instance.Deregister(this);
        }

        public override Echo CreateEcho ()
        {
            throw new System.NotImplementedException();
        }

        public override object GetState ()
        {
            throw new System.NotImplementedException();
        }

        public void TimedUpdate ()
        {
            float dt = Clock.Instance.DeltaTime;

            var targetPosition = Rigidbody.position + MoveSpeed * dt * (Vector2) transform.up;
            var targetRotation = Rigidbody.rotation - horizontalInput * TurnSpeed * dt;

            Rigidbody.MovePosition(targetPosition);
            Rigidbody.MoveRotation(targetRotation);
        }

        public void OnHorizontalInput (InputAction.CallbackContext context)
        {
            horizontalInput = context.ReadValue<float>();
        }
    }
}
