using System.Collections.Generic;


namespace ssb.Collections
{
    /// <summary>
    /// 最大容量付きキュー
    /// </summary>
    public class FixedQueue<T> : IEnumerable<T>
    {
        private Queue<T> _Queue;

        public int Count => _Queue.Count;

        public int Capacity { get; private set; }

        public FixedQueue(int capacity)
        {
            Capacity    = capacity;
            _Queue      = new Queue<T>(capacity);
        }

        public void Enqueue(T item)
        {
            _Queue.Enqueue(item);

            if (Count > Capacity) Dequeue();
        }

        public T Dequeue()      => _Queue.Dequeue();

        public T Peek()         => _Queue.Peek();

        public void Clear()     => _Queue.Clear();

        public T[] ToArray()    => _Queue.ToArray();

        public IEnumerator<T> GetEnumerator()       => _Queue.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()  => _Queue.GetEnumerator();
    }
}
