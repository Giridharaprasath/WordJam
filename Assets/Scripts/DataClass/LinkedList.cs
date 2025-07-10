using System;
using System.Collections.Generic;

namespace WordJam
{
    [Serializable]
    public class LinkedList
    {
        [Serializable]
        public class Node
        {
            public Node next;
            public int data;
        }

        public Node head = null;
        public int Count = 0;

        public LinkedList()
        {
            head = null;
        }

        public void Add(int data)
        {
            if (Count == 0)
            {
                head = new()
                {
                    data = data,
                    next = null
                };
            }
            else
            {
                Node newNode = new()
                {
                    data = data,
                    next = null
                };

                Node current = head;
                while (current.next != null)
                {
                    current = current.next;
                }

                current.next = newNode;
            }
            ++Count;
        }

        public void Remove(int data)
        {
            if (head == null) return;


            if (head.data == data)
            {
                --Count;
                head = head.next;
                return;
            }

            Node current = head;
            while (current.next != null)
            {
                if (current.next.data == data)
                {
                    --Count;
                    current.next = current.next.next;
                    return;
                }

                current = current.next;
            }
        }

        public void RemoveFrom(int data)
        {
            if (head == null) return;

            if (head.data == data)
            {
                Count = 0;
                head = null;
                return;
            }

            int tempCount = 0;

            Node current = head;
            while (current.next != null)
            {
                if (current.data == data)
                {
                    Count = tempCount;
                    current.next = null;
                    return;
                }

                current = current.next;
                tempCount++;
            }
        }

        public bool Contains(int data)
        {
            if (Count == 0) return false;

            if (head.data == data) return true;

            Node current = head;
            while (current != null)
            {
                if (current.data == data)
                {
                    return true;
                }

                current = current.next;
            }

            return false;
        }

        public int FindIndex(int data)
        {
            if (Count == 0) return -1;

            int index = 0;
            Node current = head;
            while (current != null)
            {
                if (current.data == data)
                {
                    return index;
                }

                index++;
                current = current.next;
            }

            return -1;
        }

        public void Clear()
        {
            head = null;
            Count = 0;
        }

        public List<int> GetData()
        {
            if (head == null) return null;

            List<int> allData = new();

            Node current = head;
            while (current != null)
            {
                allData.Add(current.data);
                current = current.next;
            }

            return allData;
        }
    }
}
