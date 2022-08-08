using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using NaughtyAttributes;
using crass;

namespace mj112.Holeworms
{
    public class HolewormHead : MonoBehaviour
    {
        public bool Echo;

        [Foldout("Movement")]
        [Tooltip("In units per Clock-second")]
        public float MoveSpeedMin, MoveSpeedMax, MoveSpeedAcceleration, MoveSpeedDeceleration;
        [Foldout("Movement")]
        [Tooltip("In degress per Clock-second")]
        public float TurnSpeedMin, TurnSpeedMax, TurnSpeedAcceleration;

        [Foldout("Segment Control")]
        [Tooltip("In frames")]
        public int SegmentFollowDelay;
        [Foldout("Segment Control")]
        public List<HolewormSegement> Segments;

        public BagRandomizer<AudioClip> EatSounds, DeathSounds;

        [Foldout("References")]
        public AudioSource AudioSource;
        [Foldout("References")]
        public Rigidbody2D Rigidbody;
        [Foldout("References")]
        public IntVariable ParadoxesEaten;
        [Foldout("References")]
        [Tag]
        public string HolewormEchoTag, ParadoxTag;

        HolewormState state;

        float currentMoveSpeed, currentTurnSpeed;

        void Start ()
        {
            if (!Echo)
            {
                // TODO: pool this
                state = HolewormState.CreateInitial(this);
            }
        }

        void OnTriggerEnter2D (Collider2D collider)
        {
            if (Echo) return;

            if (collider.CompareTag(HolewormEchoTag))
            {
                AudioSource.PlayOneShot(DeathSounds.GetNext());
                GameOverManager.Instance.EndGame();
            }
            else if (collider.CompareTag(ParadoxTag))
            {
                AudioSource.PlayOneShot(EatSounds.GetNext());
                collider.GetComponent<Paradox>().Kill();
                ParadoxesEaten.Value++;
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

            if (input == 0)
            {
                currentMoveSpeed = Mathf.Min(currentMoveSpeed + dt * MoveSpeedAcceleration, MoveSpeedMax);
                currentTurnSpeed = TurnSpeedMin;
            }
            else
            {
                currentTurnSpeed = Mathf.Min(currentTurnSpeed + dt * TurnSpeedAcceleration, TurnSpeedMax);
                currentMoveSpeed = Mathf.Max(currentMoveSpeed - dt * MoveSpeedDeceleration, MoveSpeedMin);
            }

            var targetPosition = Rigidbody.position + currentMoveSpeed * dt * (Vector2)transform.up;
            var targetRotation = Rigidbody.rotation - input * currentTurnSpeed * dt;

            state = HolewormState.CopyOf(state);
            state.RecordEntry(targetPosition, targetRotation);

            return state;
        }
    }
}
