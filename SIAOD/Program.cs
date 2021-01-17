﻿using System;

namespace SIAOD
{
    class Program
    {
        //  Необходимо реализовать очередь на базе списков, применяя комбинированный алгоритм для ее обслуживания.
        //  Затем продемонстрировать выполнение основных операций с элементами очереди: поиск, добавление, удаление.

        /*
         * Перечисление приоритетов узлов.
         * Чем больше число, тем выше приоритет
         */
        public enum Priority
        {
            LOW = 1, // Низкий
            MEDIUM = 2, // Средний
            HIGH = 3 // Высокий
        }

        //класс узла - наш объект в очереди
        public class Node<T> 
        {
            // следующий узел
            public Node<T> next;
            // данные
            public T data;
            // приоритет
            public Priority? priority;
            
            public Node(T data)
            {
                this.data = data;
                this.next = null;
            }
            
            public Node(T data, Priority priority, Node<T> next)
            {
                this.data = data;
                this.next = next;
                this.priority = priority;
            }
        }

        public class Queue<T>
        {
            //голова очереди
            private Node<T> _head; 

            //добавление узла без приоритета
            public void AddNodeWithoutPriority(T data)
            {
                //если очередь пустая - создаем, устанавливает голову
                if (_head == null)
                {
                    _head = new Node<T>(data); 
                }
                else
                {
                    var createdNode = _head;
                    // цикл while на поиск последнего узла очереди
                    while (createdNode.next != null) createdNode = createdNode.next;
                    //устанавливаем указатель на созданный узел
                    createdNode.next = new Node<T>(data);
                }
            }
            
            //добавление узла с приоритетом
            public Node<T> AddNodeWithPriority(T data, Priority priority)
            {
                Node<T> previous = null; // ссылка на предыдущий узел
                var current = _head;

                // итерация по очереди, пока не достигнут конец или более высокий приоритет
                while (current != null && current.priority >= priority)
                {
                    previous = current;
                    current = current.next;
                }

                //создаем узел
                var createdNode = new Node<T>(data, priority, current);

                //если не сделали итераций, добавляем в начало
                if (previous == null) 
                {
                    _head = createdNode;
                    return _head;
                }

                previous.next = createdNode;
                return createdNode;
            }


            //поиск по значению
            public int FindNodePosition(T data)
            {
                var node = _head;
                var i = 0;

                //цикл while проходит по очереди пока не найдет результ
                while (node.next != null && !node.data.Equals(data))
                {
                    //меняем указатель на след элемент
                    node = node.next;
                    i++;
                }

                // нет узла - возвращаем -1
                if (node.next == null && !node.data.Equals(data))
                {
                    i = -1;
                }

                return i;
            }


            //удаление узла
            public void DeleteNode(T data)
            {
                var node = _head;
                Node<T> previous = null;

                //цикл while проходит по очереди пока не найдет результ
                while (node != null && !node.data.Equals(data))
                {
                    previous = node;
                    node = node.next;
                }

                // переставляем указатели, тем самым удаляя
                previous.next = node.next;
            }

            //вывод информации о очереди
            public void WriteInfo()
            {
                Console.WriteLine("Queue:");
                
                var node = _head;
                //цикл while проходит по очереди
                while (node != null)
                {
                    Console.Write(node.data + " ---> ");
                    node = node.next;
                }
                Console.WriteLine();
                Console.WriteLine("-------------");
            }

        }

        static void Main(string[] args)
        {
            var testQueue = new Queue<string>();

            // Добавление текстовых сообщений для обработки в очередь
            testQueue.AddNodeWithoutPriority("Hello!");
            testQueue.AddNodeWithoutPriority("How are you?");
            testQueue.AddNodeWithPriority("This is important!", Priority.MEDIUM);
            testQueue.AddNodeWithPriority("Check it when possible", Priority.LOW);
            testQueue.AddNodeWithoutPriority("Did you see the news?");
            testQueue.AddNodeWithPriority("We need you ASAP!!", Priority.HIGH);
            testQueue.AddNodeWithPriority("Come to me", Priority.LOW);

            testQueue.WriteInfo();

            // Получения индекса сообщения в очереди
            var msg = "Did you see the news?";
            var msgIndex = testQueue.FindNodePosition(msg);
            Console.WriteLine("Your message \"" + msg + "\" is " + msgIndex + " in the queue");

            // Удаление сообщений из очереди
            testQueue.DeleteNode("Did you see the news?");
            testQueue.DeleteNode("Come to me");

            testQueue.WriteInfo();

        }
    }
}
