using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    public class Heap<T> where T : IComparable
    {
        private List<T> elements = new List<T>();
        public int GetSize()
        {
            return elements.Count;
        }
        public T? GetMin()
        {
            return this.elements.Count > 0 ? this.elements[0] : default;
        }
        public void Add(T item)
        {
            elements.Add(item);
            this.HeapifyUp(elements.Count - 1);
        }
        public T? PopMin()
        {
            if (elements.Count > 0)
            {
                T item = elements[0];
                elements[0] = elements[elements.Count - 1];
                elements.RemoveAt(elements.Count - 1);

                this.HeapifyDown(0);
                return item;
            }

            throw new InvalidOperationException("no element in heap");
        }
        private void HeapifyUp(int index)
        {
            var parent = Heap<T>.GetParent(index);
            if (parent >= 0 && elements[index].CompareTo(elements[parent]) < 0)
            {
                var temp = elements[index];
                elements[index] = elements[parent];
                elements[parent] = temp;
                this.HeapifyUp(parent);
            }
        }
        private void HeapifyDown(int index)
        {
            var smallest = index;
            var left = Heap<T>.GetLeft(index);
            var right = Heap<T>.GetRight(index);

            if (left < this.GetSize() && elements[left].CompareTo(elements[index]) < 0)
                smallest = left;

            if (right < this.GetSize() && elements[right].CompareTo(elements[smallest]) < 0)
                smallest = right;

            if (smallest != index)
            {
                var temp = elements[index];
                elements[index] = elements[smallest];
                elements[smallest] = temp;
                this.HeapifyDown(smallest);
            }
        }
        private static int GetParent(int index)
        {
            if (index <= 0)
                return -1;

            return (index - 1) / 2;
        }
        private static int GetLeft(int index)
        {
            return 2 * index + 1;
        }
        private static int GetRight(int index)
        {
            return 2 * index + 2;
        }
    }
}
