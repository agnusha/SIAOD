using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SIAOD
{
    class Program
    {
        //11. Создать два однонаправленных списка, состоящих из n символов латинского алфавита.
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
            Node<T> head; // головной/первый элемент
            Node<T> tail; // последний/хвостовой элемент
            int count;  // количество элементов в списке

            // добавление элемента
            public void Add(T data)
            {
                Node<T> node = new Node<T>(data);

                if (head == null)
                    head = node;
                else
                    tail.Next = node;
                tail = node;

                count++;
            }


            // добвление в начало
            public void AppendFirst(T data)
            {
                Node<T> node = new Node<T>(data)
                {
                    Next = head
                };
                head = node;
                if (count == 0)
                    tail = head;
                count++;
            }


            // удаление элемента
            public bool Remove(T data)
            {
                Node<T> current = head;
                Node<T> previous = null;

                while (current != null)
                {
                    if (current.Data.Equals(data))
                    {
                        // Если узел в середине или в конце
                        if (previous != null)
                        {
                            // убираем узел current, теперь previous ссылается не на current, а на current.Next
                            previous.Next = current.Next;

                            // Если current.Next не установлен, значит узел последний,
                            // изменяем переменную tail
                            if (current.Next == null)
                                tail = previous;
                        }
                        else
                        {
                            // если удаляется первый элемент
                            // переустанавливаем значение head
                            head = head.Next;

                            // если после удаления список пуст, сбрасываем tail
                            if (head == null)
                                tail = null;
                        }
                        count--;
                        return true;
                    }

                    previous = current;
                    current = current.Next;
                }
                return false;
            }
            public int Count { get { return count; } }
            public bool IsEmpty { get { return count == 0; } }

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

            // реализация интерфейса IEnumerable
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this).GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                Node<T> current = head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
        }

        public static class ListHelpers
        {
            private const string alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

            //заполнение списка
            public static LinkedList<char> CreateLinkedList(int n)
            {
                //создаем пустой список
                LinkedList<char> list = new LinkedList<char>();

                foreach (var i in Enumerable.Range(0, n))
                {
                    // добавляем элементы в список
                    list.Add(alphabet[i]);
                }
                return list;
            }


            // перемещаем в третий список: сначала строчные символы из обоих списков, а затем – прописные.
            public static LinkedList<char> MoveToResultLinkedList(LinkedList<char> list1, LinkedList<char> list2)
            {
                // соединяем два списка
                static LinkedList<char> MergeLinkedList(LinkedList<char> list1, LinkedList<char> list2)
                {
                    foreach (var item in list2)
                    {
                        list1.Add(item);
                    }
                    return list1;
                }

                //создаем пустой список
                LinkedList<char> lowerCase = new LinkedList<char>();
                LinkedList<char> upperCase = new LinkedList<char>();

                foreach (var item in MergeLinkedList(list1, list2))
                {
                    if (char.IsLower(item))
                    {
                        lowerCase.Add(item);
                    }
                    else
                    {
                        upperCase.Add(item);
                    }
                }

                return MergeLinkedList(lowerCase, upperCase);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hi. Write n - count element of first list");
            var list1 = ListHelpers.CreateLinkedList(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("Write n - count element of second list");
            var list2 = ListHelpers.CreateLinkedList(Convert.ToInt32(Console.ReadLine()));

            var list3 = ListHelpers.MoveToResultLinkedList(list1, list2);
            list3.WriteList();
        }
    }
}
