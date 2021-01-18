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

        //Прошитое бинарное дерево поиска
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



            // обход дерева для вывода
            public void TreeTraversal(Node* root)
            {
                // Возвращает наследника
                Node* getSuccessor(Node* ptr)
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
                
                if (root == null)
                    return;

                var ptr = root;
                while (ptr->isNoThreadLeft == false)
                {
                    ptr = ptr->left;
                }

                // цикл для вывода информации
                while (ptr != null)
                {
                    Console.Write(ptr->data + " ");
                    ptr = getSuccessor(ptr);
                }
            }

            // удаление 
            public Node* RemoveNode(Node* rootNode, int data)
            {

                // левый лист удаляем
                Node* getNodeWhenNoSuccessors(Node* root, Node* par, Node* ptr)
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

                // у узла два наследника
                Node* getNodeWhenTwoSuccessors(Node* root, Node* ptr)
                {
                    // находим родителя
                    Node* parSuccessor = ptr;
                    Node* successor = ptr->right;

                    // находим левого потомка наследника
                    while (successor->isNoThreadLeft == false)
                    {
                        parSuccessor = successor;
                        successor = successor->left;
                    }

                    ptr->data = successor->data;

                    if (successor->isNoThreadLeft == true && successor->isNoThreadRight == true)
                    {
                        root = getNodeWhenNoSuccessors(root, parSuccessor, successor);
                    }
                    else
                    {
                        root = getNodeWhenOneSuccessors(root, parSuccessor, successor);
                    }

                    return root;
                }

                // один потомок
                Node* getNodeWhenOneSuccessors(Node* root, Node* par, Node* ptr)
                {

                    // ищем преемника
                    Node* findSuccessor(Node* ptr)
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

                    // ищем предшественника 
                    Node* findPred(Node* ptr)
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

                    // только левый потомок
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
                    var s = findSuccessor(ptr);
                    var p = findPred(ptr);

                    // есть правая ветка
                    if (ptr->isNoThreadLeft == false)
                    {
                        p->right = s;
                    }
                    // есть правая ветка
                    else
                    {
                        if (ptr->isNoThreadRight == false)
                        {
                            s->left = p;
                        }
                    }

                    return root;
                }

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
                            // указатель на левого потомка
                            ptr = ptr->left;
                        else
                            break;
                    }
                    else
                    {
                        if (ptr->isNoThreadRight == false)
                            // указатель на правого потомка
                            ptr = ptr->right;
                        else
                            break;
                    }
                }

                if (isFoundElement == false)
                {
                    Console.WriteLine("Нет искомого элемента");
                }
                //  есть ва потомка
                else if (ptr->isNoThreadLeft == false && ptr->isNoThreadRight == false)
                {
                    rootNode = getNodeWhenTwoSuccessors(rootNode, ptr);
                }
                // есть левый потом
                else if (ptr->isNoThreadLeft == false)
                {
                    rootNode = getNodeWhenOneSuccessors(rootNode, par, ptr);
                }
                // есть правый потомок
                else if (ptr->isNoThreadRight == false)
                {
                    rootNode = getNodeWhenOneSuccessors(rootNode, par, ptr);
                }
                // потомков нет
                else
                {
                    rootNode = getNodeWhenNoSuccessors(rootNode, par, ptr);
                }

                return rootNode;
            }
        }

    static unsafe void Main(string[] args)
        {
            var tree = new Tree();

            var root = new Node { data = 15, isNoThreadLeft = true, isNoThreadRight = true };
            var node2 = new Node { data = 25, isNoThreadLeft = true, isNoThreadRight = true };
            var node1 = new Node { data = 5, isNoThreadLeft = true, isNoThreadRight = true };
            var node3 = new Node { data = 1, isNoThreadLeft = true, isNoThreadRight = true };
            var node4 = new Node { data = 11, isNoThreadLeft = true, isNoThreadRight = true };
            var node5 = new Node { data = 9, isNoThreadLeft = true, isNoThreadRight = true };
            var node6 = new Node { data = 12, isNoThreadLeft = true, isNoThreadRight = true };
            var node7 = new Node { data = 8, isNoThreadLeft = true, isNoThreadRight = true };
            
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

            Console.WriteLine("Обход первоначальный");
            tree.TreeTraversal(rootPointer);
            Console.WriteLine();

            Console.WriteLine("Удаляем элемент со значением " + 15);
            tree.RemoveNode(rootPointer, 15);

            Console.WriteLine("Обход после удаления");
            tree.TreeTraversal(rootPointer);

        }
    }
}
