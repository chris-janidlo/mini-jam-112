using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace mj112.Holeworms
{
    public class HolewormHead : MonoBehaviour
    {
        public bool Echo;

        [Foldout("Movement")]
        [Tooltip("In units per Clock-second")]
        public float MoveSpeed;
        [Foldout("Movement")]
        [Tooltip("In degress per Clock-second")]
        public float TurnSpeed;

        [Foldout("Segment Control")]
        [Tooltip("In frames")]
        public int SegmentFollowDelay;
        [Foldout("Segment Control")]
        public List<HolewormSegement> Segments;

        [Foldout("References")]
        public Rigidbody2D Rigidbody;

        HolewormState state;

        void Start ()
        {
            if (!Echo)
            {
                // TODO: pool this
                state = HolewormState.CreateInitial(this);
            }
        }

        public void ApplyState (HolewormState state, bool teleport)
        {
            state.Entries[^1].ApplyToRigidbody(Rigidbody, teleport);

            int bufferCursor = state.Entries.Length - 1 - SegmentFollowDelay;
            foreach (var segment in Segments)
            {
                segment.SetLocation(state.Entries[bufferCursor], teleport);
                bufferCursor -= SegmentFollowDelay;
            }
        }

        public HolewormState NewStateFrom (float input)
        {
            float dt = Clock.Instance.DeltaTime;

            var targetPosition = Rigidbody.position + MoveSpeed * dt * (Vector2)transform.up;
            var targetRotation = Rigidbody.rotation - input * TurnSpeed * dt;

            state = HolewormState.CopyOf(state);
            state.RecordEntry(targetPosition, targetRotation);

            return state;
        }
    }
}
