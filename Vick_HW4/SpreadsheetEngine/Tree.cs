using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    class ExpressionTree
    {
        private Node m_root;
        public String m_expression; //store input expression
        //dict use to store and look up the variable value
        public Dictionary<string, double> m_dict = new Dictionary<string, double>();

        //Paramaterized constructor for taking an expression
        public void ExpTree(string exp)
        {
            m_expression = exp;
            m_root = Compile(exp);
        }

        //Default constructor
        public void ExpTree()
        {
            m_expression = "";
        }
        //Dictionary property
        public void SetDict(Dictionary<string, double> dict)
        {
            m_dict = dict;
        }

        //Used to build expression tree
        public void BuildTree(string exp)
        {
            m_expression = exp;
            m_root = Compile(exp);
        }

        private Node Compile(string s)
        {
            //Make sure string is exists else return null
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            //Chop off parenthesis
            if ('(' == s[0])
            {
                int counter = 1;
                for (int i = 1; i < s.Length; i++)
                {
                    if ('(' == s[i]) { counter++; }
                    else if (')' == s[i])
                    {
                       counter--;
                        if (counter == 0)
                        {
                            if (i != s.Length - 1){ break; }
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

            //If there is no operator in the expression, 
            //then s must be a variable or a constant.
            //Return appropriate nodes.
            double num;
            if (double.TryParse(s, out num))
            {
                return new constNode(){val = num};
            }
            else
            {
                //m_dict[s] = 0;
                return new varNode(){valName = s};
            }
        }

        private Node Compile(string s, char op)
        {
            int count = 0, parenCounter = 0;

            count = -1;
            for (int i = s.Length - 1; i != -1; i += count)
            {
                //Miss matching parenthesis
                if ('(' == s[i] && 0 == parenCounter){throw new Exception();}

                if (')' == s[i]){ parenCounter++; }

                else if ('(' == s[i]){ parenCounter--; }

                //same thing as before, if the parenthesis don't close, ignore what is inside
                if (parenCounter != 0){ continue; }

                if (op == s[i])
                {
                    opNode opNo = new opNode(s[i]);
                    opNo.Left = Compile(s.Substring(0, i));
                    opNo.Right = Compile(s.Substring(i + 1));
                    return opNo;
                }
            }
            

            //this makes sure that all parenthesis are a matching pair
            if (0 != parenCounter)
            {
                throw new Exception();
            }
            //if the operator is not found, return null
            return null;
        }

        public void SetVariable(string name, double value)
        {
            m_dict[name] = value;
        }

        private double Evaluate(Node n)
        {
            //check for constant
            constNode constnode = n as constNode;

            if (null != constnode)
            {
                return constnode.val;
            }

            //check for variable
            varNode varnode = n as varNode;
            if (null != varnode)
            {
                //If we perform undo on a reference we erase we need to make sure we update the cell
                if(m_dict.ContainsKey(varnode.valName))
                {
                    return m_dict[varnode.valName];
                }
                else
                {
                    return 0;
                }
                
            }

            opNode opnode = n as opNode;
            //check for operator
            if (null != opnode)
            {
                //evaluate
                switch (opnode.op)
                {
                    case '+':
                        return Evaluate(opnode.Left) + Evaluate(opnode.Right);
                    case '-':
                        return Evaluate(opnode.Left) - Evaluate(opnode.Right);
                    case '*':
                        return Evaluate(opnode.Left) * Evaluate(opnode.Right);
                    case '/':
                        return Evaluate(opnode.Left) / Evaluate(opnode.Right);
                    default:
                        throw new Exception();
                }
            }
            throw new Exception();
        }

        public double Evaluate()
        {
            return Evaluate(m_root);
        }
    }
    
}
