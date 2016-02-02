using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    //base class
    public abstract class Node {}

    //inheriting operand node class
    public class opNode : Node
    {
        public char op;
        public Node Left, Right;
        public opNode parent;

        //assign operand to member variable (op)
        public opNode(char ch){ op = ch; }
    }

    //inheriting variable node class
    public class varNode : Node { public string valName;}

    //inheriting constant node class
    public class constNode : Node {public double val;}   
}
