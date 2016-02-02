/*
 * Russ Vick
 * ID 11180466
 * Created on: January 30, 2015
 * 
 * This code uses a winforms application to load, save, and generate
 * the first 50 and 100 Fibonacci numbers
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO;


namespace Vick_HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Generic load function
        private void loadText(TextReader text)
        {
            string sstr = text.ReadToEnd();
            textBox1.Text = sstr;
        }

        //Loads first 50 Fibonacci numbers to the text box
        private void loadFibonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            StringBuilder sstr = new StringBuilder();
            FibonacciTextReader fib50 = new FibonacciTextReader(50);
            for (int i = 0; i < 50; i ++)
            {
                //Pubiter helps the FibonacciTextReader to find out what line we need to deliever the fib number on
                fib50.pubiter = i;

                //Appends the fibonacci number into the string builder
                sstr.AppendLine((i + 1) + ": " + fib50.ReadLine());

            }
            using (StringReader read = new StringReader(sstr.ToString()))
            {
                loadText(read);
            }
        }
        
        //Loads the first 100 numbers of the Fibonacci sequence to the text box
        private void loadFibonaciiNumbersFirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Initialization
            StringBuilder sstr = new StringBuilder();
            FibonacciTextReader fib100 = new FibonacciTextReader(100);
            
            for (int i = 0; i < 100; i++)
            {
                //Pubiter helps the FibonacciTextReader to find out what line we need to deliever the fib number on
                fib100.pubiter = i;

                //Appends the fibonacci number into the string builder 
                sstr.AppendLine((i + 1) + ": " + fib100.ReadLine());
            }
            using (StringReader read = new StringReader(sstr.ToString()))
            {
                loadText(read);
            }
        }

        //Loads text from a file specified by the user
        private void loadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            //Sets filter and filter index for OpenfileDialog instance
            file.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            file.FilterIndex = 1;

            file.Multiselect = true;

            if(file.ShowDialog() == DialogResult.OK)
            {
                Stream fstream = file.OpenFile();

                using(StreamReader read = new StreamReader(fstream))
                {
                   loadText(read);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();

            //Sets filter and filter index for OpenfileDialog instance
            file.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            file.FilterIndex = 2;

            file.RestoreDirectory = true;

            if (file.ShowDialog() == DialogResult.OK)
            {
               using(StreamWriter sw = new StreamWriter (file.FileName))
               {
                   sw.Write(textBox1.Text);
               }
            }
        }       
    }

    //A class which controls calculating and returning a specified number of Fibonacci numbers
    public class FibonacciTextReader : TextReader
    {
        //Sets our max lines
        public int mLines = 0;

        //Pubiter keeps track of the line we are on during the fibonacci sequence
        public int pubiter = 0;

        public string fibSequence = "";
        StringBuilder sb = new StringBuilder();

        public FibonacciTextReader(int maxlines)
        {
            mLines = maxlines;         
        }

        //This method provides an override to readline which will 
        //calculate the next fibonacci number and return it as a string
        public override string ReadLine()
        {
            BigInteger num = 0;
            BigInteger first_num = 0, second_num = 1;
            
            for (int i = 0; i <= pubiter; i++)
            {
                
                if(pubiter == mLines)
                {
                    return null;
                }
                //Special case to deal with 0 and 1 of the fibonacci sequence setting the 
                //number returned to the iterator
                else if (i == 0 || i == 1)
                {
                    num = i;                    
                }
                else
                {                   
                    num = first_num + second_num;
                    first_num = second_num;
                    second_num = num;                    
                }
            }
            return num.ToString();
        }                     
    }
}
