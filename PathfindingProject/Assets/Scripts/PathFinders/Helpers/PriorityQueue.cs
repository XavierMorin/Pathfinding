
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private Node head;
    public PriorityQueue(T data, double priority)
    {
        Node node = newNode(data, priority);
        head = node;
    }
    private class Node
    {
        public T data;
        public double priority;
        public Node next;
    }


    private static Node newNode(T data, double priority)
    {
        Node temp = new Node();
        temp.data = data;
        temp.priority = priority;
        temp.next = null;
        return temp;
    }

   

    public T pop()
    {
        T temp = (head).data;
        (head) = (head).next;
        return temp;
    }

    public void push(T data, double priority)
    {
        if (isEmpty())
        {
            head = newNode(data, priority);
            return;
        }
        Node start = (head);
        Node temp = newNode(data, priority);
        if ((head).priority > priority)
        {
            temp.next = head;
            (head) = temp;
        }
        else
        {
            while (start.next != null && start.next.priority < priority)
            {
                start = start.next;
            }
            temp.next = start.next;
            start.next = temp;
        }
      
    }
    public bool isEmpty()
    {
        return ((head) == null) ? true : false;
    }

    public bool has(T data)
    {
        Node temp = head;

        while (temp != null)
        {
            if (EqualityComparer<T>.Default.Equals(temp.data,data))
                return true;
            temp = temp.next;
        }
        return false;

    }
    public void setPriority(T data, double newPriority)
    {

        Node temp = head;
        if(EqualityComparer<T>.Default.Equals(head.data, data))
        {
            pop();
            push(temp.data,newPriority);
            return;
        }

        while (temp.next != temp)
        {
            if (EqualityComparer<T>.Default.Equals(temp.next.data, data))
            {
                T nextTemp =temp.next.data;
                temp.next = temp.next.next;
                push(nextTemp, newPriority);
                return;

            }
               
            temp = temp.next;
        }
        

    }


}

