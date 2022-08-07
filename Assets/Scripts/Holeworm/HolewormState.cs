using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112.Holeworms
{
    public struct HolewormState
    {
        public struct Entry
        {
            public Vector3 Position;
            public float Angle;

            public void ApplyToRigidbody(Rigidbody2D rigidbody, bool teleport)
            {
                if (teleport)
                {
                    rigidbody.position = Position;
                    rigidbody.rotation = Angle;
                }
                else
                {
                    rigidbody.MovePosition(Position);
                    rigidbody.MoveRotation(Angle);
                }
            }
        }

        public readonly Entry[] Entries;

        public HolewormState (int size)
        {
            Entries = new Entry[size];
        }

        public static HolewormState CreateInitial (HolewormHead holewormHead)
        {
            // +1 is necessary due to how records are retrieved backward
            return new HolewormState(holewormHead.SegmentFollowDelay * holewormHead.Segments.Count + 1);
        }

        public static HolewormState CopyOf (HolewormState other)
        {
            int size = other.Entries.Length;
            HolewormState copy = new(size);
            Array.Copy(other.Entries, copy.Entries, size);
            return copy;
        }

        public void RecordEntry (Vector3 position, float angle)
        {
            // shift left
            Array.Copy(Entries, 1, Entries, 0, Entries.Length - 1);

            Entries[^1] = new Entry
            {
                Position = position,
                Angle = angle
            };
        }
    }
}
