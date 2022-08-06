using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace mj112
{
    public class TimeEchoer : MonoBehaviour, IClockFollower
    {
        class EchoStateRepository
        {
            private readonly Dictionary<(int, Guid), object> storage = new ();
            private readonly HashSet<Guid> dataProcessedThisFrame = new ();

            public void SetRecord (int frameNumber, Guid echoGuid, object state)
            {
                storage[(frameNumber, echoGuid)] = state;
            }

            public bool TryGetRecord (int frameNumber, Guid echoGuid, out object state)
            {
                return storage.TryGetValue((frameNumber, echoGuid), out state);
            }

            public void MarkAsProcessed (Guid echoGuid)
            {
                dataProcessedThisFrame.Add(echoGuid);
            }

            public void ClearProcessedCache ()
            {
                dataProcessedThisFrame.Clear();
            }

            public IEnumerable<(Guid, object)> GetUnprocessedData (int frameNumber)
            {
                foreach (var key in storage.Keys)
                {
                    if (key.Item1 == frameNumber && !dataProcessedThisFrame.Contains(key.Item2))
                    {
                        yield return (key.Item2, storage[key]);
                    }
                }
            }
        }

        public Echoable Echoable;
        public int PoolDefaultCapacity = 10, PoolMaxSize = 10000;

        readonly EchoStateRepository echoStateRepository = new ();
        readonly List<Echo> currentEchoes = new ();

        ObjectPool<Echo> echoPool;
        Guid recordingGuid;

        void Start ()
        {
            Clock.Instance.Register(this, onTimeJumped);

            echoPool = new ObjectPool<Echo>
            (
                createFunc: Echoable.CreateEcho,
                actionOnGet: e => e.SetActive(true),
                actionOnRelease: e => e.SetActive(false),
                actionOnDestroy: e => e.Kill(),
                collectionCheck: true,
                defaultCapacity: PoolDefaultCapacity,
                maxSize: PoolMaxSize
            );

            onTimeJumped();
        }

        void OnDestroy ()
        {
            Clock.Instance.Deregister(this, onTimeJumped);
        }

        public void TimedUpdate ()
        {
            int frame = Clock.Instance.FramesElapsedInLoop;

            manageEchoes(frame);
            recordActivity(frame);
        }

        void onTimeJumped ()
        {
            recordingGuid = Guid.NewGuid();
        }

        void manageEchoes (int frame)
        {
            for (int i = currentEchoes.Count - 1; i >= 0; i--)
            {
                Echo echo = currentEchoes[i];
                if (echoStateRepository.TryGetRecord(frame, echo.Guid, out object state))
                {
                    echo.ApplyState(state);
                    echoStateRepository.MarkAsProcessed(echo.Guid);
                }
                else
                {
                    currentEchoes.RemoveAt(i);
                    echoPool.Release(echo);
                }
            }

            foreach (var (guid, state) in echoStateRepository.GetUnprocessedData(frame))
            {
                Echo echo = echoPool.Get();
                echo.AssignGuid(guid);
                echo.ApplyState(state);

                currentEchoes.Add(echo);
            }

            echoStateRepository.ClearProcessedCache();
        }

        void recordActivity (int frame)
        {
            echoStateRepository.SetRecord(frame, recordingGuid, Echoable.GetState());
        }
    }
}
