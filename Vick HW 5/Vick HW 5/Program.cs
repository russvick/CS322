// ---------------------------------------------------------------------------
// This program builds an expression tree for expressions with one operand.
// This program doesn't support multiple operand expressions,
// parenthesis or exponenets
//
// Author: Russ Vick
// ID: 11180466
// ---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Vick_HW_5
{
    //Class is used to collect expressions and assign variables determined by the user
    public class Expressions : ExpressionTree
    {
        //Main program.
        static void Main(string[] args)
        {
            //Variable initialization
            ExpressionTree bst = new ExpressionTree();
            string userOption = "", variable = "";
            double value = 0.0;

            //Beginning of program
            while (userOption != "0")
            {
                userOption = bst.Menu(bst.Exp);

                //Find the operator that is used

                switch (userOption)
                {
                    //Parses the expression based on arthmetic operators and also checks for constants and places 
                    //them in the dictionary at their actual value
                    case "1":
                        Console.Write("Enter an Expression: ");
                        bst.Exp = Console.ReadLine();
                        bst.cleanParse(bst.Exp);
                        bst.addParenthesis();
                        break;
                    case "2":
                        //Error Checking to ensure that the proper variable name is manipulated
                        bool marker = false;
                        while (marker == false)
                        {
                            //Assigns variable that will be changed
                            variable = bst.optionTwo();
                            foreach (string c in bst.ParsedExp)
                            {
                                //checks to make sure that the variable exists and that it is a variable that can be changed
                                if (variable == c && bst.isVariable(variable) == true)
                                {
                                    marker = true;
                                }
                            }
                        }
                        //Assigns a value to the variable and stores it into a dictionary.
                        while (true)
                        {
                            Console.Write("Enter a variable value: ");
                            string variableValue = Console.ReadLine();
                            if (Double.TryParse(variableValue, out value) == true)
                            {
                                value = Convert.ToDouble(variableValue);
                                break;
                            }
                        }
                        //Stores the variable(key) and the variable value(value)
                        bst.M_variables[variable] = value;
                        break;
                    case "3":
                        //Build expression tree and evaluate 
                        bst.Evalu();
                        //double pr = bst.Evaluate(bst.M_variables);

                        // Console.WriteLine(pr.ToString());
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public interface Node { };
    //Class dedicated to constants added to a node
    public class constNode : Node { public double val;}
    public class varNode : Node { public string valName;}
    public class opNode : Node
    {
        public char op;
        public Node Left, Right;
        public opNode parent;
    }

    //Class is responsible for creating and evaluating the expression tree
    public class ExpressionTree : Node
    {
        private Node root;
        private double m_eval;
        private string oper;
        private string currExpression;
        private string[] parsedExp;
        private Dictionary<string, double> m_variables;
        private string evalstring;

        public double Eval
        {
            get { return m_eval; }
            set { m_eval = value; }
        }

        public string evalString
        {
            get { return evalstring; }
            set { evalstring = value; }
        }

        public Dictionary<string, double> M_variables
        {
            get { return m_variables; }
            set { m_variables = value; }
        }
        public string[] ParsedExp
        {
            get { return parsedExp; }
            set { parsedExp = value; }
        }

        //Chops off parenthesis and adds to a parsed array containing all variables and constants. Also assigns all variables to 0
        public void cleanParse(string s)
        {
            m_variables = new Dictionary<string, double>();
            string subtext = "";
            int index = 0;
            parsedExp = s.Split('*', '/', '+', '-');
            foreach (string pe in parsedExp)
            {
                for (int i = 0; i < pe.Length; i++)
                {
                    if (Char.IsLetterOrDigit(pe[i]) == true)
                    {
                        subtext += pe[i];
                    }
                }
                if (isVariable(subtext) == true)
                {
                    m_variables[subtext] = 0.0;
                }
                parsedExp[index] = subtext;
                index++;
                //reset subtext value;
                subtext = "";
            }
        }

        public string Exp
        {
            get { return currExpression; }
            set { currExpression = value; }
        }

        public string Oper
        {
            get { return oper; }
            set { oper = value; }
        }

        public Node Root
        {
            get { return root; }
            set { root = value; }
        }

        public ExpressionTree()
        {
            root = null;
        }        
        //Prompts user for an number based on menu options
        public string Menu(string expres)
        {
            string option = "";
            //Menu Options
            Console.WriteLine("Menu (current expression = " + expres + ")");
            Console.WriteLine("1 = Enter a new expression");
            Console.WriteLine("2 = Set variable value");
            Console.WriteLine("3 = Evaluate tree");
            Console.WriteLine("0 = Quit");

            return option = Console.ReadLine();
        }

        //Prompts user for an expression
        public void optionOne()
        {
            Console.WriteLine("Enter an Expression");
            currExpression = Console.ReadLine();
            cleanParse(currExpression);
        }

        //Prompts user for a variable that will later be assigned to a value in main
        public string optionTwo()
        {
            string variable = "";
            Console.Write("Enter a variable name: ");
            return variable = Console.ReadLine();
        }

        public void addParenthesis()
        {

            foreach (char c in new char[] { '*', '/', '+', '-' })
            {
                int counter = 0;
                for (int i = 0; i < currExpression.Length; i++)
                {
                    if (currExpression[i] == '(') { counter++; }
                    else if (currExpression[i] == ')') { counter--; }
                    //We found where we want to insert
                    else if (currExpression[i] == c && counter != 0)
                    {
                        int validInsert = i + 1;

                        //Iterate through string until we find another operator and add at that position
                        while ((validInsert < currExpression.Length) && (currExpression[validInsert] != '*' && currExpression[validInsert] != '/'
                            && currExpression[validInsert] != '+' && currExpression[validInsert] != '-'))
                        {
                            validInsert++;
                            //If we run into another parenthesis before operator break out                            
                        }
                        if (currExpression[validInsert - 1] == ')' || validInsert == currExpression.Length) { break; }
                        currExpression = currExpression.Insert(validInsert, ")");

                        int secondCounter = 0;
                        //Iterate through s from right to left looking for matching parenthesis
                        for (int j = validInsert; j >= 0; j--)
                        {
                            if (currExpression[j] == ')') { secondCounter++; }
                            else if (currExpression[j] == '(') { secondCounter--; }
                            //Insert open parenthesis after a closed expression or at the end of the expression
                            if ((secondCounter == 0 && currExpression[j] == '(') || j == 0)
                            {
                                currExpression = currExpression.Insert(j + 1, "(");
                                i++;
                                break;
                            }
                        }
                    }
                }

            }
        }


        //Checks a expression varaible to find if it is an constant or not
        public bool isConstant(string variable)
        {
            int value = 0;
            if (Int32.TryParse(variable, out value) == true) { return true; }
            else { return false; }
        }

        //Checks a expression variable to find if it is a variable
        public bool isVariable(string variable)
        {
            int value = 0;
            if (Int32.TryParse(variable, out value) == false) { return true; }
            else { return false; }
        }

        //Responsible for building the expression tree
        public void createTree()
        {
            root = Compile(currExpression);
            setParentNode(root);
        }
        private static Node Compile(string s)
        {
            if (string.IsNullOrEmpty(s)) { return null; }
            if ('(' == s[0])
            {
                int counter = 1;
                //Iterate through each character in the string (s) and count parenthesis
                for (int i = 1; i < s.Length; i++)
                {
                    //start parenthesis counter
                    if ('(' == s[i]) { counter++; }
                    else if (')' == s[i])
                    {
                        counter--;
                        //Enter if we match parenthesis and recursively call the substring within the parenthesis
                        if (counter == 0)
                        {
                            if (i != s.Length - 1) { break; }
                            else { return Compile(s.Substring(1, s.Length - 2)); }
                        }
                    }
                }
            }
            char[] ops = { '+', '-', '*', '/' };
            foreach (char op in ops)
            {
                Node n = Compile(s, op);
                if (n != null) { return n; }
            }

            //return a constant or var node 
            double num;
            if (double.TryParse(s, out num))
            {
                return new constNode() { val = num };
            }
            else
            {
                return new varNode() { valName = s };
            }
        }
        private static Node Compile(string exp, char op)
        {
            int counter = 0;
            //Iterate through expression from right to left
            for (int i = exp.Length - 1; i >= 0; i--)
            {
 
                if (')' == exp[i]) { counter++; }
                else if ('(' == exp[i]) { counter--; }
                if (counter == 0 && op == exp[i])
                {
                    opNode on = new opNode() { op = exp[i] };

                    on.Left = Compile(exp.Substring(0, i));
                    on.Right = Compile(exp.Substring(i + 1));

                    return on;
                }
            }
            //Throw exeception if we don't find a matching parenthesis
            if (counter != 0) { throw new Exception(); }
            return null;
        }
        //Sets all parent operand nodes in tree
        public void setParentNode(Node n)
        {
            opNode op = (opNode)n;
            //Recursively look through left subtree and attach a parent node to each node after root
            if(op.Left.GetType() == typeof(opNode))
            {
                opNode temp = (opNode)op.Left;
                temp.parent = (opNode)op;
                setParentNode(op.Left);
            }
            else if (op.Right.GetType() == typeof(opNode))
            {
                opNode temp = (opNode)op.Right;
                temp.parent = (opNode)op;
                setParentNode(op.Right);
            }
            
        }
        //Builds a node depending on the specifics of the string value passed
        private Node buildNonOpNode(string exp)
        {
            double num;
            if (double.TryParse(exp, out num))
            {
                return new constNode() { val = num };
            }
            return new varNode() { valName = exp };
        }
        public double Evalu()
        {
            //Create tree builds tree through the use of the two compile functions and also calls the set parent function
            createTree();
            //PostOrder does the actual evaluation of the nodes in the tree
            postOrder(root);
            Console.WriteLine(m_eval);
            return m_eval;
        }

        public double lval = 0.0, rval = 0.0;

        public void postOrder(Node n)
        {
            //Function passes in root as n for first initial function call
            opNode op = (opNode)n;
            
            if (m_eval != 0)
            {
                double temp = evalSubTree(op);
                if (op.op == '+') { m_eval += temp; }
                else if (op.op == '-') { m_eval -= temp; }
                else if (op.op == '*') { m_eval *= temp; }
                else if (op.op == '/') { m_eval /= temp; }
                if (op.parent != null)
                {
                    postOrder(op.parent);
                }
                else { return; }
                
            }
            else
            {
                //If we reach a varNode or constNode for a left child we have reached the end of our tree and we can now start to carry out computations
                if (op.Left.GetType() != typeof(opNode) && op.Right.GetType() != typeof(opNode))
                {
                    m_eval = evalSubTree(op);
                    //after evaluation pass in the parent of the current node to begin moving back up the tree
                    postOrder(op.parent);
                }
                //traverse left and right until we reach the end 
                if (op.Left.GetType() == typeof(opNode) && m_eval == 0)
                {
                    postOrder(op.Left);
                }
                if (op.Right.GetType() == typeof(opNode))
                {
                    postOrder(op.Right);
                }
            }               
        }

        //Helper funton used to get the value of the var or const node with relation to the operand node
        public double evalSubTree(opNode op)
        {
            double temp = 0.0;
            //If left and right node is constant carry out computation
            if ((op.Left.GetType() == typeof(constNode) == true) && (op.Right.GetType() == typeof(constNode)))
            {
                constNode lConstant = (constNode)op.Left;
                constNode rConstant = (constNode)op.Right;
                double lvalue = lConstant.val;
                double rvalue = rConstant.val;
                temp = calculateChildren(op.op, lvalue, rvalue);
            }
            //If left node is var and right node is constant carry out computation
            else if ((op.Left.GetType() == typeof(varNode) == true) && (op.Right.GetType() == typeof(constNode)))
            {
                varNode lConstant = (varNode)op.Left;
                constNode rConstant = (constNode)op.Right;
                double lvalue = m_variables[lConstant.valName];
                double rvalue = rConstant.val;
                temp = calculateChildren(op.op, lvalue, rvalue);
            }
            //If left and right are var nodes look up values in dictionary and compute
            else if ((op.Left.GetType() == typeof(varNode) == true) && (op.Right.GetType() == typeof(varNode)))
            {
                varNode lConstant = (varNode)op.Left;
                varNode rConstant = (varNode)op.Right;
                double lvalue = m_variables[lConstant.valName];
                double rvalue = m_variables[rConstant.valName];
                temp = calculateChildren(op.op, lvalue, rvalue);
            }
            //if left is constant node and right is var carry out computations and neccessary steps
            else if ((op.Left.GetType() == typeof(constNode) == true) && (op.Right.GetType() == typeof(varNode)))
            {
                constNode lConstant = (constNode)op.Left;
                varNode rConstant = (varNode)op.Right;
                double lvalue = lConstant.val;
                double rvalue = m_variables[rConstant.valName];
                temp = calculateChildren(op.op, lvalue, rvalue);
            }
            //get right child if its a varNode 
            else if((op.Left.GetType() == typeof(opNode) == true) && (op.Right.GetType() == typeof(varNode)))
            {
                varNode rVar = (varNode)op.Right;
                double rvalue = m_variables[rVar.valName];
                return temp;
            }
            //get right child if its a constNode
            else if((op.Left.GetType() == typeof(opNode) == true) && (op.Right.GetType() == typeof(constNode)))
            {
                constNode rVar = (constNode)op.Right;
                double rvalue = rVar.val;
                temp = rvalue;
            }  
            //Left child is a varNode while right is an opNode
            else if((op.Left.GetType() == typeof(varNode)) && (op.Right.GetType() == typeof(opNode)))
            {
                varNode lvar = (varNode)op.Left;
                double lvalue = m_variables[lvar.valName];
                temp = lvalue;
            }
            //left child
            else if((op.Left.GetType() == typeof(constNode)) && (op.Right.GetType() == typeof(opNode)))
            {
                constNode lvar = (constNode)op.Left;
                double lvalue = lvar.val;
                temp = lvalue;
            }
            return temp;
        }
        //Helper function to carry out calculations on children nodes by passing in the left and right values with the operand.
        public double calculateChildren(char op, double left, double right)
        {
            if (op == '+') { return left + right; }
            else if (op == '-') { return left - right; }
            else if (op == '*') { return left * right; }
            else { return left / right; }
        }
    }
}
