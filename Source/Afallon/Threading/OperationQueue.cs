using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    // TODO: I'm sure this can be optimized
    internal class OperationQueue
    {
        public class Node
        {
            public DispatcherOperation Value;
            public Node Previous;
            public Node Next;
        }

        private int _count;
        private Node _root;

        public int Count
        {
            get { return _count; }
        }

        public DispatcherOperation Peek()
        {
            return _root == null ? null : _root.Value;
        }

        public DispatcherOperation Dequeue()
        {
            Node node = _root;

            if (node == null)
                return null;

            node.Value.QueueNode = null;
            _root = node.Next;
            _count--;

            return node.Value;
        }

        public void Enqueue(DispatcherOperation operation)
        {
            operation.QueueNode = new Node { Value = operation };
            Enqueue(operation.QueueNode);
        }

        public void UpdatePriority(DispatcherOperation operation)
        {
            Node node = operation.QueueNode;

            if (operation.QueueNode == null)
                return;

            Remove(node);
            Enqueue(node);
        }

        public void Remove(DispatcherOperation operation)
        {
            Node node = operation.QueueNode;

            if (node == null)
                return;

            Remove(node);
            operation.QueueNode = null;
        }

        private void Remove(Node node)
        {
            if (node == _root)
            {
                _root = node.Next;
            }
            else
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
            }
        }

        private void Enqueue(Node newNode)
        {
            if (_root != null)
            {
                Node node = _root;

                while (node.Next != null && node.Value.Priority > newNode.Value.Priority)
                    node = node.Next;

                if (node == _root)
                {
                    newNode.Next = node.Next;
                    node.Previous = newNode;
                    _root = newNode;
                }
                else
                {
                    newNode.Next = node.Next;
                    newNode.Previous = node;

                    if (newNode.Next != null)
                        newNode.Next.Previous = newNode;

                    node.Next = newNode;
                }
            }
            else
            {
                _root = newNode;
            }

            _count++;
        }
    }
}
