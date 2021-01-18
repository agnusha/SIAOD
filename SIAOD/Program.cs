using System;
using System.Runtime.CompilerServices;

namespace SIAOD
{
    //  Необходимо реализовать очередь на базе списков, применяя комбинированный алгоритм для ее обслуживания.
    //  Затем продемонстрировать выполнение основных операций с элементами очереди: поиск, добавление, удаление.




    class Program
    {
        //класс узла - наш объект в стеке
        public class Node<T>
        {
            // следующий узел
            public Node<T> next;
            // данные
            public T data;

            public Node(T data)
            {
                this.data = data;
            }
        }

        //стэк
        public class Stack<T>
        {
            Node<T> head;
            int count;

            public bool IsEmpty
            {
                get { return count == 0; }
            }

            public void Push(T item)
            {
                // увеличиваем стек
                Node<T> node = new Node<T>(item);
                node.next = head; // переустанавливаем верхушку стека на новый элемент
                head = node;
                count++;
            }

            public T Pop()
            {
                // если стек пуст, выбрасываем исключение
                if (IsEmpty)
                    throw new InvalidOperationException("Стек пуст");
                Node<T> temp = head;
                head = head.next; // переустанавливаем верхушку стека на следующий элемент
                count--;
                return temp.data;
            }

            public T Peek()
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Стек пуст");
                return head.data;
            }
        }

        public static class StackHelpers
        {

            static string operators = "*/+-()";
            static bool isOperator(char symbol)
            {
                return operators.Contains(symbol);
            }


            private static int GetPriority(char symbol)
            {
                switch (symbol)
                {
                    case '(':
                        return 0;
                    case '+':
                    case '-':
                        return 1;
                    case '/':
                    case '*':
                        return 2;
                    default:
                        return -1;
                }
            }


            internal static string infixToPrefix(string input)
            {
                var result = "";
                // переворачиваем строку
                for (var i = input.Length - 1; i >= 0; i--)
                {
                    if (input[i] == ')')
                    {
                        result += '(';
                    }
                    else if (input[i] == '(')
                    {
                        result += ')';
                    }
                    else
                    {
                        result += input[i];
                    }
                }


                // транслируем
                result = infixToPostfix(result);
                //переворачиваем
                var charArray = result.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }

            internal static string infixToPostfix(string input)
            {
                var result = "";
                var stack = new Stack<char>();

                foreach (var symbol in input)
                {
                    if (!isOperator(symbol))
                    {
                        result += symbol;
                    }
                    else
                    {
                        var symbolPriority = GetPriority(symbol);
                        if (stack.IsEmpty || symbol == '(')
                        {
                            stack.Push(symbol);
                            continue;
                        }


                        var current = stack.Peek();
                        var currentPriority = GetPriority(current);


                        if (symbol == ')')
                        {
                            while (!stack.IsEmpty)
                            {
                                if (current == '(')
                                {
                                    // нашли открывающуюся скобку, завершаем цикл
                                    stack.Pop();
                                    break;
                                }

                                result += current;
                                // извлекаем элемент из стека для следующей итерации
                                stack.Pop();
                                current = stack.Peek();
                            }

                            continue;
                        }

                        // если приоритет меньше - помещаем в стек
                        if (currentPriority < symbolPriority)
                        {
                            stack.Push(symbol);
                        }
                        else
                        {
                            // извлекаем все элементы и помещаем в рузультат
                            // пока приоритет больше или равен, или
                            // находим открывающуюся скобку


                            while (currentPriority >= symbolPriority)
                            {
                                if (current != '(')
                                {
                                    result += current;
                                }

                                stack.Pop();
                                if (stack.IsEmpty)
                                {
                                    break;
                                }

                                current = stack.Peek();
                                currentPriority = GetPriority(current);
                            }

                            stack.Push(symbol);
                        }
                    }
                }

                while (!stack.IsEmpty)
                {
                    result += stack.Peek();
                    stack.Pop();
                }

                return result;
            }
        }


        static void Main(string[] args)
        {
            var infixInput = "a+b/(c-d)";
            Console.WriteLine(infixInput);
            Console.WriteLine("postfix result = " + StackHelpers.infixToPostfix(infixInput));
            Console.WriteLine("prefix result = " + StackHelpers.infixToPrefix(infixInput));
        }
    }
}
