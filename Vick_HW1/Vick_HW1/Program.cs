using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vick_HW1
{
    class BST
    {
        class Node
        {
            public int data;
            public Node left_node, right_node;

            public Node(int data)
            {
                this.data = data;
                left_node = null;
                right_node = null;
            }
        }
  
        class BinaryFunctions
        {
            public Node root;
            static int number_of_nodes;
            static int levels;
           

            public bool isLeaf(Node n)
            {
                if(n.right_node == null && n.left_node == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
         
            public void insert(Node current_node, int key)
            {   
                
                if (root == null)
                {
                    root = new Node(key);
                    number_of_nodes++;
                }
   
                //Program reached a leaf node so insert 
                else if (current_node.data == key) { return; }

                else if (current_node.data > key && current_node.left_node == null)
                { 
                    current_node.left_node = new Node(key);
                    number_of_nodes++;
                }
                else if (current_node.data < key && current_node.right_node == null) 
                {
                    current_node.right_node = new Node(key);
                    number_of_nodes++;
                }
                //root is not leaf so traverse till leaf is hit
                else
                {
                    //leaf node has not been reached so continue to search for proper path down the tree                  
                    if (current_node.data == key)
                        return;

                    else if (current_node.data > key)
                        insert(current_node.left_node, key);

                    else
                        insert(current_node.right_node, key);                                    
                }
            }
            //Does an in order traversal to report back the contents of the tree in accending order 
            public void inOrderTraversal(Node current)
            {
                //Traverse in the order of left current right
                if(current != null)
                {
                    inOrderTraversal(current.left_node);
                    Console.Write(current.data + " ");
                    inOrderTraversal(current.right_node);
                }
               
            }

            //Basic function to allow access to the number of nodes recorded in the function "public void insert(Node current_node, int key)"
            public int returnNodes()
            {                             
                return number_of_nodes;
            }
            public int countLevels(Node current)
            {
        
                if(current == null)
                {
                    return 0;
                }
                else
                {
                    //traverse down both sides of the current node and search for the greater depth
                    int rightdepth = countLevels(current.right_node);
                    int leftdepth = countLevels(current.left_node);

                    //find if the max depth from the left is greater from the right and proceed by returning the 
                    //greater of the two depths
                    if (leftdepth > rightdepth)
                        return leftdepth + 1;
                    else
                        return rightdepth + 1;
                  
                }
            }         
        }
    
        static void Main(string[] args)
        {
            BinaryFunctions tree = new BinaryFunctions();
            int sumdepth = 0;
            int numberOfNodes = 0;
            double minLevel = 0;

            Console.WriteLine("Enter a collection of numbers in the range [0,100], seperated by spaces:\n");
            string line = Console.ReadLine();

            string[] line_parsed = line.Split(new char[] {' '});
            for (int i = 0; i < line_parsed.Length; i++)
                tree.insert(tree.root, int.Parse(line_parsed[i]));

            Console.Write("Tree Contents: ");
            tree.inOrderTraversal(tree.root);
            numberOfNodes = tree.returnNodes();
            sumdepth = tree.countLevels(tree.root);
            minLevel = Math.Log(numberOfNodes, 2.0);
            Console.WriteLine();
            Console.WriteLine("Tree Statistics:");
            Console.WriteLine("  Number of nodes: " + numberOfNodes);          
            Console.WriteLine(" Number of levels: " + sumdepth);
            Console.WriteLine(" Minumum number of levels that a tree with " + numberOfNodes + " nodes could have = " + Math.Ceiling(minLevel));
            Console.WriteLine("Done!");
            
            
        }
    }
}
