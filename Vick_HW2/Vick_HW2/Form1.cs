using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vick_HW2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            //Unsorted 
            HashSet<int> set = new HashSet<int>();
            List<int> list = new List<int>();
            SortedSet<int> sortset = new SortedSet<int>();
            StringBuilder s = new StringBuilder();
            Random rand = new Random();

            //Add random numbers to a list (not ordered)
            for (int i = 0; i < 10000; i++) { list.Add(rand.Next(1, 20000)); }

            //Place unique numbers from the list previously generated into a hashset
            for (int i = 0; i < 10000; i++) { set.Add(list[i]); }

            //Sorts the list of random numbers through the use of a Sortedset
            for (int i = 0; i < 10000; i++) { sortset.Add(list[i]); }

            //count distinct characters in the list 
            int unique = (from i in list select i).Distinct().Count();

            //String formatting
            s.Append("1. Hashset method: " + set.Count().ToString() + " unique numbers." +
            "The time complexity of this code is O(nlog(n) + 2n), because the intialization of variables " +
            "is O(1) and the add function for the hashset and list is O(1). Therefore the algorithm executes" +
            "10000 times twice and we'll mark this up to be (2n)." +
            "This gives us a time complexity for the aglorithm to be O(2n). Then the add time complexity for the sortedset is nlogn which consequatnly"+
            "gives us O(nlog(n) + 2n)");
            s.Append("\r\n2. " + unique.ToString() + " unique numbers.");
            s.Append("\r\n3. " + sortset.Count().ToString() + " unique numbers");
            textBox1.Text = s.ToString();                  
        }
    }  
}
