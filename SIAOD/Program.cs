using System;

namespace SIAOD
{
    //  Необходимо реализовать очередь на базе списков, применяя комбинированный алгоритм для ее обслуживания.
    //  Затем продемонстрировать выполнение основных операций с элементами очереди: поиск, добавление, удаление.

    class Program
    {
        //класс узла - наш объект в очереди
        public class Node<T>
        {
            // следующий узел
            public Node<T> next;
            // данные
            public T data;
            // приоритет
            public int? priority;
            
            public Node()
            {
            }

            public Node(T data)
            {
                this.data = data;
                this.next = null;
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
            public Node<T> AddNodeWithPriority(T data, int priority)
            {
                // вначале ссылка на предыдущий узел null
                Node<T> previous = null;
                var node = _head;

                // итерация по очереди, пока не достигнут конец или более высокий приоритет
                while (node != null && node.priority >= priority)
                {
                    previous = node;
                    node = node.next;
                }

                //создаем узел
                var createdNode = new Node<T>()
                {
                    data = data,
                    next = node,
                    priority = priority
                }; 

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
            public void FindNodePosition(T data)
            {
                int GetNodePosition(T nodeData)
                {
                    var node = _head;
                    var i = 0;

                    //цикл while проходит по очереди пока не найдет результ
                    while (node.next != null && !node.data.Equals(nodeData))
                    {
                        //меняем указатель на след элемент
                        node = node.next;
                        i++;
                    }

                    // нет узла - возвращаем -1
                    if (node.next == null && !node.data.Equals(nodeData))
                    {
                        i = -1;
                    }

                    return i;
                }

                void WriteInformation(int nodePosition)
                {
                    Console.WriteLine($"Element {data} is in the position {nodePosition}");
                    Console.WriteLine("-------------");
                    Console.WriteLine();


                }

                var position = GetNodePosition(data);
                WriteInformation(position);

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

                Console.WriteLine("Deleted element " + data);

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
                Console.WriteLine();

            }

        }

        static void Main(string[] args)
        {
            var testQueue = new Queue<string>();

            // добавим узлы с приоритетами и без
            testQueue.AddNodeWithoutPriority("1: Без приоритета");
            testQueue.AddNodeWithoutPriority("3: Без приоритета");
            testQueue.AddNodeWithPriority("4: Приоритет 3 - наивысший", 3);
            testQueue.AddNodeWithoutPriority("5: Без приоритета");
            testQueue.AddNodeWithPriority("6: Приоритет 2", 2);
            testQueue.AddNodeWithPriority("7: Приоритет 2", 2);
            testQueue.AddNodeWithPriority("8: Приоритет 1", 1);


            testQueue.WriteInfo();

            // поиск по значению
            const string searchElement = "5: Без приоритета";
            testQueue.FindNodePosition(searchElement);

            // удаление узла
            testQueue.DeleteNode("5: Без приоритета");

            testQueue.WriteInfo();

        }
    }
}
