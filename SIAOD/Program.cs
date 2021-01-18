using System;

namespace SIAOD
{
    // Ввести 10-15 целых чисел и построить из них бинарное дерево поиска.
    //1.  Выполнить симметричную прошивку бинарного дерева поиска.Обойти его согласно симметричному порядку следования элементов. Реализовать вставку и удаление элементов из симметрично прошитого бинарного дерева.
    //2.  Выполнить прямую прошивку бинарного дерева поиска. Обойти его согласно прямому порядку следования элементов.Реализовать вставку и удаление элементов из прямо прошитого бинарного дерева.



    class Program
    {
        //класс узла - наш объект в стеке
        public unsafe struct Node
        {
            //данные
            public int data;
            //указатали
            public Node* left;
            public Node* right;

            //тип связи
            //false - прошивочная нить
            public bool isNoThreadLeft;
            public bool isNoThreadRight;
        }

        //Прошитое бинароное дерево поиска
        public unsafe class Tree
        {
            // Вставка
            public Node* Insert(Node* rootNode, Node* newNode)
            {
                Node* par = null;
                var node = rootNode;

                while (node != null)
                {
                    par = node;
                    // в левое поддерево
                    if (newNode->data < node->data)
                    {
                        //если дерево не прошитое, то меняем указатель 
                        if (node->isNoThreadLeft == false)
                        {
                            node = node->left;
                        }
                        else
                        {
                            break;
                        }
                    }
                    // в правое поддерево
                    else
                    {
                        if (node->isNoThreadRight == false)
                        {
                            node = node->right;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //дерево пустое, добавляем в корень
                if (par == null)
                {
                    rootNode = newNode;
                    newNode->left = null;
                    newNode->right = null;
                }
                //вставляем как левый потомок родителя
                else if (newNode->data < par->data)
                {
                    newNode->left = par->left;
                    newNode->right = par;
                    par->isNoThreadLeft = false;
                    par->left = newNode;
                }
                else
                {
                    newNode->left = par;
                    newNode->right = par->right;
                    par->isNoThreadRight = false;
                    par->right = newNode;
                }

                return rootNode;
            }

            // возвращает наследника использую правую ветку
            private Node* OrderSuccessor(Node* ptr)
            {
                // идем в правое поддерево
                if (ptr->isNoThreadRight)
                {
                    return ptr->right;
                }

                // идем в левое поддерево
                ptr = ptr->right;
                while (ptr->isNoThreadLeft == false)
                {
                    ptr = ptr->left;
                }

                return ptr;
            }

            // выводимм дерево
            public void OrderTraversal(Node* root)
            {
                if (root == null)
                {
                    return;
                }

                // находим левую ноду
                Node* ptr = root;
                while (ptr->isNoThreadLeft == false)
                {
                    ptr = ptr->left;
                }

                // печатаем
                while (ptr != null)
                {
                    Console.Write(ptr->data + " ");
                    ptr = OrderSuccessor(ptr);
                }
            }

            // удаление 
            public Node* RemoveNode(Node* rootNode, int data)
            {
                var isFoundElement = false;
                Node* par = null, ptr = rootNode;

                // цикл
                while (ptr != null)
                {
                    //нашли - выходим
                    if (data == ptr->data)
                    {
                        isFoundElement = true;
                        break;
                    }

                    par = ptr;
                    if (data < ptr->data)
                    {
                        if (ptr->isNoThreadLeft == false)
                            //устанавливаем указатель на левого потомка
                            ptr = ptr->left;
                        else
                            break;
                    }
                    else
                    {
                        if (ptr->isNoThreadRight == false)
                            //устанавливаем указатель на правого потомка
                            ptr = ptr->right;
                        else
                            break;
                    }
                }

                if (isFoundElement == false)
                {
                    Console.WriteLine("Нет искомого элемента");
                }
                // ситуация когда два потомка
                else if (ptr->isNoThreadLeft == false && ptr->isNoThreadRight == false)
                {
                    rootNode = caseC(rootNode, ptr);
                }
                // только левый потом
                else if (ptr->isNoThreadLeft == false)
                {
                    rootNode = caseB(rootNode, par, ptr);
                }
                // только правый потомок
                else if (ptr->isNoThreadRight == false)
                {
                    rootNode = caseB(rootNode, par, ptr);
                }
                // нету потомков
                else
                {
                    rootNode = caseA(rootNode, par, ptr);
                }

                return rootNode;
            }

            // удаление левого листа
            public Node* caseA(Node* root, Node* par, Node* ptr)
            {
                // если нода для удаления вершина
                if (par == null)
                    root = null;

                // левая или родитель
                else if (ptr == par->left)
                {
                    par->isNoThreadLeft = true;
                    par->left = ptr->left;
                }
                else
                {
                    par->isNoThreadRight = true;
                    par->right = ptr->right;
                }

                return root;
            }

            // нода для удаления имеет только одного потомка
            private Node* caseB(Node* root, Node* par,
                Node* ptr)
            {

                // находим преемника
                Node* inSucc(Node* ptr)
                {
                    if (ptr->isNoThreadRight)
                    {
                        return ptr->right;
                    }

                    ptr = ptr->right;
                    while (ptr->isNoThreadLeft == false)
                    {
                        ptr = ptr->left;
                    }

                    return ptr;
                }

                // находим предшественника 
                Node* inPred(Node* ptr)
                {
                    if (ptr->isNoThreadLeft == true)
                    {
                        return ptr->left;
                    }

                    ptr = ptr->left;
                    while (ptr->isNoThreadRight == false)
                    {
                        ptr = ptr->right;
                    }

                    return ptr;
                }
                
                Node* child;

                // имеет только левого потомка
                if (ptr->isNoThreadLeft == false)
                {
                    child = ptr->left;
                }
                // только правый потомок
                else
                {
                    child = ptr->right;
                }

                // вершина
                if (par == null)
                {
                    root = child;
                }
                // нода для удаления левый ребенок родителя
                else if (ptr == par->left)
                {
                    par->left = child;
                }
                else
                {
                    par->right = child;
                }

                // находим приемника и предшественника
                var s = inSucc(ptr);
                var p = inPred(ptr);

                // If ptr has left subtree.
                if (ptr->isNoThreadLeft == false)
                {
                    p->right = s;
                }
                // если существует правая ветка
                else
                {
                    if (ptr->isNoThreadRight == false)
                    {
                        s->left = p;
                    }
                }

                return root;
            }




            // ситуация когда у ноды для удаления два наследника
            Node* caseC(Node* root, Node* ptr)
            {
                // находим родителя
                Node* parsucc = ptr;
                Node* succ = ptr->right;

                // находим левого потомка наследника
                while (succ->isNoThreadLeft == false)
                {
                    parsucc = succ;
                    succ = succ->left;
                }

                ptr->data = succ->data;

                if (succ->isNoThreadLeft == true && succ->isNoThreadRight == true)
                {
                    root = caseA(root, parsucc, succ);
                }
                else
                {
                    root = caseB(root, parsucc, succ);
                }

                return root;
            }
        }

    static unsafe void Main(string[] args)
        {
            var tree = new Tree();

            // создание узлов
            var root = new Node { data = 20, isNoThreadLeft = true, isNoThreadRight = true };
            var node1 = new Node { data = 10, isNoThreadLeft = true, isNoThreadRight = true };
            var node2 = new Node { data = 30, isNoThreadLeft = true, isNoThreadRight = true };
            var node3 = new Node { data = 5, isNoThreadLeft = true, isNoThreadRight = true };
            var node4 = new Node { data = 16, isNoThreadLeft = true, isNoThreadRight = true };
            var node5 = new Node { data = 14, isNoThreadLeft = true, isNoThreadRight = true };
            var node6 = new Node { data = 17, isNoThreadLeft = true, isNoThreadRight = true };
            var node7 = new Node { data = 13, isNoThreadLeft = true, isNoThreadRight = true };
            
            //добавляем в дерево
            var rootPointer = &root;
            rootPointer = tree.Insert(null, rootPointer);
            rootPointer = tree.Insert(rootPointer, &node1);
            rootPointer = tree.Insert(rootPointer, &node2);
            rootPointer = tree.Insert(rootPointer, &node3);
            rootPointer = tree.Insert(rootPointer, &node4);
            rootPointer = tree.Insert(rootPointer, &node5);
            rootPointer = tree.Insert(rootPointer, &node6);
            rootPointer = tree.Insert(rootPointer, &node7);

            // выводим дерево
            tree.OrderTraversal(rootPointer);
            // удаляем 20
            tree.RemoveNode(rootPointer, 20);
            Console.WriteLine();
            // выводим
            tree.OrderTraversal(rootPointer);

        }
    }
}
