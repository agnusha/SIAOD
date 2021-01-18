using System;

namespace SIAOD
{
    //1. Используя стек, реализовать алгоритм преобразования алгебраического выражения из инфиксной формы записи в постфиксную форму представления.
    //2. Используя стек, реализовать алгоритм преобразования алгебраического выражения из инфиксной формы записи в префиксную форму представления.
    //Для обоих алгоритмов предусмотреть вхождение операций с различными приоритетами, а также наличие скобок в инфиксных выражениях.


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

        //стек
        public class Stack<T>
        {
            //голова стека
            private Node<T> _head;
            
            //количество элементов
            private int _count;

            public bool IsEmpty => _count == 0;

            //добавление в стек
            public void Push(T item)
            {
                // создаем узел
                var node = new Node<T>(item)
                {
                    //перемещаем указатель
                    next = _head
                };
                _head = node;
                //увеличиваем количество
                _count++;
            }

            //извлечение из стека
            public T Pop()
            {
                // бросаем исключение при пустом стеке
                if (IsEmpty)
                    throw new Exception("В стеке нет элементов.");
                
                var node = _head;
                //перемещаем указатель
                _head = _head.next;
                //уменьшаем количество
                _count--;
                
                return node.data;
            }

            //возрат верхнего элемента из стека
            public T Peek()
            {
                // бросаем исключение при пустом стеке
                if (IsEmpty)
                    throw new InvalidOperationException("В стеке нет элементов.");
                
                return _head.data;
            }
        }

        public static class StackHelpers
        {
            //получаем приоритет символа
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

            //алгоритм преобразования алгебраического выражения из инфиксной формы записи в постфиксную форму представления
            internal static string InfixToPrefix(string infix)
            {
                var result = string.Empty;
                
                // в обратном порядке
                for (var i = infix.Length - 1; i >= 0; i--)
                {
                    result += infix[i] switch
                    {
                        ')' => '(',
                        '(' => ')',
                        _ => infix[i]
                    };
                }


                // транслирование
                var resultArray = InfixToPostfix(result).ToCharArray();
                
                //переворачивание
                Array.Reverse(resultArray);
                return new string(resultArray);
            }

            //алгоритм преобразования алгебраического выражения из инфиксной формы записи в префиксную форму представления.
            internal static string InfixToPostfix(string infix)
            {
                static bool IsOperator(char symbol)
                {
                    const string stackOperationList = "*/+-()";
                    return stackOperationList.Contains(symbol);
                }

                var result = string.Empty;
                var stack = new Stack<char>();

                foreach (var symbol in infix)
                {
                    if (IsOperator(symbol))
                    {
                        var priority = GetPriority(symbol);
                        // стек пустой или нашли открывающуюся скобку
                        if (stack.IsEmpty || symbol == '(')
                        {
                            //добавляем в стек
                            stack.Push(symbol);
                            continue;
                        }

                        var node = stack.Peek();
                        var nodePriority = GetPriority(node);

                        if (symbol == ')')
                        {
                            while (!stack.IsEmpty)
                            {
                                if (node == '(')
                                {
                                    // выход из цикла
                                    // когда нашли открывающуюся скобку
                                    stack.Pop();
                                    break;
                                }
                                //добавляем к результату
                                result += node;
                                //извлечение из стека
                                stack.Pop();
                                node = stack.Peek();
                            }

                            continue;
                        }

                        // когда приоритет меньше 
                        // добавляем в стек
                        if (nodePriority < priority)
                        {
                            stack.Push(symbol);
                        }
                        else
                        {
                            while (nodePriority >= priority)
                            {
                                // когда нашли открывающуюся скобку
                                //добавляем к результату

                                if (node != '(')
                                {
                                    result += node;
                                }
                                
                                //извлечение из стека
                                stack.Pop();
                                
                                //итерация окончена
                                if (stack.IsEmpty)
                                {
                                    break;
                                }

                                node = stack.Peek();
                                nodePriority = GetPriority(node);
                            }

                            stack.Push(symbol);
                        }
                    }
                    else
                    {
                        //сразу добавляем к результату
                        result += symbol;
                    }
                }

                while (!stack.IsEmpty)
                {
                    // добавляем к результату верхний элемент 
                    //пока стек не пустой
                    result += stack.Peek();
                    stack.Pop();
                }

                return result;
            }
        }


        static void Main(string[] args)
        {
            var infix1 = "А*В+С*В";
            Console.WriteLine(infix1);

            var infix2 = "(А+В)*(С+D)";
            Console.WriteLine(infix2);
            Console.WriteLine();


            Console.WriteLine("Prefix: " + StackHelpers.InfixToPrefix(infix1));
            Console.WriteLine("Postfix: " + StackHelpers.InfixToPostfix(infix1));
            Console.WriteLine("--------");
            Console.WriteLine();

            Console.WriteLine("Prefix: " + StackHelpers.InfixToPrefix(infix2));
            Console.WriteLine("Postfix: " + StackHelpers.InfixToPostfix(infix2));
            Console.WriteLine("--------");
            Console.WriteLine();

        }
    }
}
