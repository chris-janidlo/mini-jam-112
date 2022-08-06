using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace mj112
{
    public class TimeEchoer : MonoBehaviour, IClockFollower
    {
        public struct TransformRecord
        {
            public Vector3 Position, Scale;
            public Quaternion Rotation;
        }

        public GameObject EchoPrefab;
        public int PoolDefaultCapacity = 10, PoolMaxSize = 10000;

        ObjectPool<GameObject> prefabPool;
        Dictionary<int, List<TransformRecord>> frameBuffer;
        List<GameObject> lastFramesEchoes;

        void Start ()
        {
            Clock.Instance.Register(this);

            prefabPool = new ObjectPool<GameObject>
            (
                poolCreateFunc,
                poolActionOnGet,
                poolActionOnRelease,
                poolActionOnDestroy,
                collectionCheck: true,
                defaultCapacity: PoolDefaultCapacity,
                maxSize: PoolMaxSize
            );

            frameBuffer = new Dictionary<int, List<TransformRecord>>();
            lastFramesEchoes = new List<GameObject>();

            for (int i = 0; i < Clock.Instance.FramesPerLoop; i++)
            {
                frameBuffer[i] = new List<TransformRecord>();
            }
        }

        void OnDestroy ()
        {
            Clock.Instance.Deregister(this);
        }

        public void TimedUpdate ()
        {
            foreach (var echo in lastFramesEchoes)
            {
                prefabPool.Release(echo);
            }
            lastFramesEchoes.Clear();

            foreach (var record in frameBuffer[Clock.Instance.FramesElapsedInLoop])
            {
                var echo = prefabPool.Get();

                echo.transform.SetPositionAndRotation(record.Position, record.Rotation);
                echo.transform.localScale = record.Scale;

                lastFramesEchoes.Add(echo);
            }

            frameBuffer[Clock.Instance.FramesElapsedInLoop].Add(new TransformRecord
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale,
            });
        }

        GameObject poolCreateFunc ()
        {
            return Instantiate(EchoPrefab);
        }

        void poolActionOnGet (GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        void poolActionOnRelease (GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        void poolActionOnDestroy (GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}
