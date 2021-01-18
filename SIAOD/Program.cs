using System;
using System.Collections.Generic;
using System.Globalization;

namespace SIAOD
{
    //Ввести 10-15 целых чисел и построить из них с помощью указателей бинарное дерево поиска.
    //Обойти его прямым, симметричным и обратным способами.
    //Реализовать процедуры поиска, вставки и удаления элементов  бинарного дерева поиска.


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
            public Node* parent;
        }

        public unsafe class Tree
        {
            //указатель на вершину дерева

            public Node* Root { get; private set; }

            //конструктор
            public Tree()
            {
                Root = null;
            }

            public void Add(Node* node)
            {
                // дерево пустое
                //устанавливаем вершину
                if (Root == null)
                {
                    Root = node;
                    return;
                }

                var currentNode = Root;
                while (node->parent == null)
                {
                    // цикл по левой ветке
                    if (currentNode->data > node->data)
                    {
                        // если левая ветка пустая
                        if (currentNode->left == null)
                        {
                            currentNode->left = node;
                            node->parent = currentNode;
                        }
                        // переход к другой вершине
                        else
                        {
                            currentNode = currentNode->left;
                        }
                    }
                    // цикл по правой ветке
                    else
                    {
                        // если правая ветка пустая
                        if (currentNode->right == null)
                        {
                            currentNode->right = node;
                            node->parent = currentNode;
                        }
                        // переход к другой вершине
                        else
                        {
                            currentNode = currentNode->right;
                        }
                    }
                }
            }

            // высота дерева
            public int GetTreeHeight(Node* root)
            {
                if (root == null) return 0;

                // запускаем рекурсию
                var treeHeightLeft = GetTreeHeight(root->left);
                var treeHeightRight = GetTreeHeight(root->right);

                return treeHeightLeft > treeHeightRight ? treeHeightLeft + 1 : treeHeightRight + 1;
            }

            //вывод информации о строке по указанной высота
            private static void WriteRowInformation(Node* root, int height)
            {
                if (root == null) return;

                if (height == 1)
                {
                    Console.Write($"{root->data} ");
                }
                else if (height > 1)
                {
                    //идем в левое поддерево
                    WriteRowInformation(root->left, height - 1);
                    //идем в правое поддерево
                    WriteRowInformation(root->right, height - 1);
                }
            }

            // Прямой обход дерева
            public void PreOrderTraversal(Node* root)
            {
                for (var i = 0; i <= GetTreeHeight(root); i++)
                {
                    WriteRowInformation(root, i);
                    Console.WriteLine();
                }
            }

            // Обратный обход дерева
            public void PostOrderTraversal(Node* root)
            {
                for (var i = GetTreeHeight(root); i >= 1; i--)
                {
                    WriteRowInformation(root, i);
                    Console.WriteLine();
                }
            }

            // Симетричный обход дерева рекурсивно
            public void SymmetricTreeTraversal(Node* root)
            {
                if (root == null) return;

                //идем в левое поддерево
                SymmetricTreeTraversal(root->left);
                Console.Write(root->data + " ");
                //идем в правое поддерево
                SymmetricTreeTraversal(root->right);
            }

            public Node* SearchNode(int value)
            {
                var currentNode = Root;
                while (currentNode != null)
                {
                    //найден результат
                    if (currentNode->data == value)
                    {
                        return currentNode;
                    }

                    //идем в левое поддерево
                    if (currentNode->data > value)
                    {
                        currentNode = currentNode->left;
                    }
                    //идем в правое поддерево
                    else
                    {
                        currentNode = currentNode->right;
                    }
                }
                //не нашли
                return null;
            }



            public Node* Remove(Node* root, int data)
            {
                // поиск минимального через рекурсию
                static Node* getMinNode(Node* root)
                {
                    if (root->left == null)
                    {
                        return root;
                    }

                    return getMinNode(root->left);
                }
                
                if (root == null) return null;

                //идем в левое поддерево
                if (data < root->data)
                {
                    root->left = Remove(root->left, data);
                }
                else if (data > root->data)
                {
                    root->right = Remove(root->right, data);
                }
                else if (root->left != null && root->right != null)
                {
                    //искомый  элемент в корне текущего поддерева
                    root->data = getMinNode(root->right)->data;
                    root->right = Remove(root->right, root->data);
                }
                else
                {
                    if (root->left != null)
                    {
                        root = Root->left;
                    }
                    else if (root->right != null)
                    {
                        root = root->right;
                    }
                    else
                    {
                        root = null;
                    }
                }

                return root;
            }


        }

        static unsafe void Main(string[] args)
        {
            var node1 = new Node { data = 11 };
            var node2 = new Node { data = 8 };
            var node3 = new Node { data = 13 };
            var node4 = new Node { data = 6 };
            var node5 = new Node { data = 9 };
            var node6 = new Node { data = 12 };
            var node7 = new Node { data = 14 };
            var node8 = new Node { data = 15 };
            var node9 = new Node { data = 16 };
            var node10 = new Node { data = 6 };


            var tree = new Tree();
            tree.Add(&node1);
            tree.Add(&node2);
            tree.Add(&node3);
            tree.Add(&node4);
            tree.Add(&node5);
            tree.Add(&node6);
            tree.Add(&node7);
            tree.Add(&node8);
            tree.Add(&node9);
            tree.Add(&node10);

            //поиск
            Console.WriteLine("Поиск элемента по значению: " + 11);
            var result = tree.SearchNode(11);
            Console.WriteLine("Левый потомок: " + result->left->data);
            Console.WriteLine("Правый потомок: " + result->right->data);
            Console.WriteLine("-----------");

            //удаление
            Console.WriteLine();
            Console.WriteLine("Дерево:");
            tree.PreOrderTraversal(tree.Root);
            Console.WriteLine("Удаление элемента по значению: " + 16);
            tree.Remove(tree.Root, 16);
            Console.WriteLine("Дерево:");
            tree.PreOrderTraversal(tree.Root);
            Console.WriteLine("Полученная высота: " + tree.GetTreeHeight(tree.Root));
            Console.WriteLine("-----------");

            //обходы
            Console.WriteLine();
            Console.WriteLine("Прямой обход дерева");
            tree.PreOrderTraversal(tree.Root);
            Console.WriteLine("-----------");

            Console.WriteLine("Обратный обход дерева");
            tree.PostOrderTraversal(tree.Root);
            Console.WriteLine("-----------");

            Console.WriteLine("Симметричный обход дерева");
            tree.SymmetricTreeTraversal(tree.Root);
            Console.WriteLine();

        }
    }
}
