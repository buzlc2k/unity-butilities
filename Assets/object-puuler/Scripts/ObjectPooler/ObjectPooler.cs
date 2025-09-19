using System.Collections.Generic;
using UnityEngine;

namespace ObjectPuuler
{
    public class ObjectPooler<T> where T : MonoBehaviour, IPooled
    {
        private readonly Queue<T> Pool;
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPooler(T prefab, int size)
        {
            Pool = new Queue<T>(size);
            _prefab = prefab;
            _parent = new GameObject().transform;
            for (var i = 0; i < size; i++)
            {
                Create();
            }
        }

        public ObjectPooler(T prefab, Transform parent, int size)
        {
            Pool = new Queue<T>(size);
            _prefab = prefab;
            _parent = parent;
            for (var i = 0; i < size; i++)
            {
                Create();
            }
        }

        private T Create()
        {
            var newObj = Object.Instantiate(_prefab, _parent);
            newObj.ReleaseCallback += Release;
            newObj.gameObject.SetActive(false);
            return newObj;
        }

        private void Release(GameObject _object)
        {
            Pool.Enqueue(_object.GetComponent<T>());
        }

        public T Get()
        {
            if (Pool.Count == 0)
            {
                var newObj = Create();
                Pool.Enqueue(newObj);
            }

            var Obj = Pool.Dequeue();
            Obj.gameObject.SetActive(true);
            return Obj;
        }

        public T Get(Vector3 position)
        {
            if (Pool.Count == 0)
            {
                var newObj = Create();
                Pool.Enqueue(newObj);
            }

            var Obj = Pool.Dequeue();
            Obj.transform.position = position;
            Obj.gameObject.SetActive(true);
            return Obj;
        }

        public T Get(Vector3 position, Vector3 rotation)
        {
            if (Pool.Count == 0)
            {
                var newObj = Create();
                Pool.Enqueue(newObj);
            }

            var Obj = Pool.Dequeue();
            Obj.transform.position = position;
            Obj.transform.up = rotation;
            Obj.gameObject.SetActive(true);
            return Obj;
        }
    }
}