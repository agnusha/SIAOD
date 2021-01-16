using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SIAOD
{
    class Program
    {
        // Var 11. Создать два однонаправленных списка, состоящих из n символов латинского алфавита.
        // Переместить все данные в третий список таким образом, чтобы сначала шли все строчные
        // символы из обоих списков, а затем – прописные.

        //класс узла - наш объект в списке
        public class Node<T>
        {
            //конструктор
            public Node(T data)
            {
                Data = data;
            }

            //данные
            public T Data { get; set; }
            //указатель на следующий элемент
            public Node<T> Next { get; set; }
        }

        // класс - односвязный список
        public class LinkedList<T> : IEnumerable<T>  
        {
            private Node<T> _head; // головной/первый элемент
            private Node<T> _tail; // последний/хвостовой элемент

            // добавление элемента
            public void Add(T data)
            {
                var node = new Node<T>(data);

                if (_head == null)
                    _head = node;
                else
                    _tail.Next = node;
                _tail = node;
            }

            // вывод информации
            public void WriteList()
            {
                Console.WriteLine("-------------");
                Console.WriteLine("List:");
                foreach (var item in this)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("-------------");
            }

            // implement IEnumerable
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this).GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                var current = _head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
        }

        public static class ListHelpers
        {
            private const string Alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

            // заполнение списка
            public static LinkedList<char> CreateLinkedList(int n)
            {
                // создаем пустой список
                var list = new LinkedList<char>();

                foreach (var i in Enumerable.Range(0, n))
                {
                    // добавляем элементы в список
                    list.Add(Alphabet[i]);
                }
                return list;
            }


            // перемещаем в третий список: сначала строчные символы из обоих списков, а затем – прописные.
            public static LinkedList<char> MoveToResultLinkedList(LinkedList<char> list1, LinkedList<char> list2)
            {
                // соединяем два списка
                static LinkedList<char> MergeLinkedList(LinkedList<char> list1, LinkedList<char> list2)
                {
                    foreach (var item in list2) list1.Add(item);
                    return list1;
                }

                //создаем пустой список
                var lowerCase = new LinkedList<char>();
                var upperCase = new LinkedList<char>();

                foreach (var item in MergeLinkedList(list1, list2))
                {
                    if (char.IsLower(item))
                        lowerCase.Add(item);
                    else
                        upperCase.Add(item);
                }

                return MergeLinkedList(lowerCase, upperCase);
            }
        }

        static void Main(string[] args)
        {
            // ввод данных для первого списка
            Console.WriteLine("Hi. Write n - count element of first list");
            var list1 = ListHelpers.CreateLinkedList(Convert.ToInt32(Console.ReadLine()));

            // ввод данных для второго списка
            Console.WriteLine("Write n - count element of second list");
            var list2 = ListHelpers.CreateLinkedList(Convert.ToInt32(Console.ReadLine()));
            
            //получение и вывод результата
            var list3 = ListHelpers.MoveToResultLinkedList(list1, list2);
            list3.WriteList();
        }
    }
}
