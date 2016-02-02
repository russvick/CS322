using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace SpreadsheetEngine
{
    public abstract class Cell : INotifyPropertyChanged
    {
        protected string m_text;
        protected string m_val;
        private int rowIndex;
        private int colIndex;
        //-1 is the int for white
        public int BGcolor = -1;

        //Property for BG color
        public int GetBGColor()
        {
            return BGcolor;
        }
        public void SetBGColor(int color)
        {
            BGcolor = color;
            OnPropertyChanged(BGcolor.ToString());
        }
        //Lets us know if the aspects of a specific cell have changed
        public bool hasChanged()
        {
            if (BGcolor != -1 || this.Value != "")
                return true;
            else
                return false;
        }

        public Cell() { }

        //Parametarized constructor used to set row and column index
        public Cell(int row, int col)
        {
            this.m_text = "";
            this.m_val = "";
            rowIndex = row;
            colIndex = col;
        }

        //Property that evaluates the value of a cell
        public string Value
        {
            get
            {
                return m_val;
            }
        }

        //Read-only property to return the specified row index
        public int RowIndex
        {
            get { return rowIndex; }
        }
        //Read-only property to return the specified column index
        public int ColumnIndex
        {
            get { return colIndex; }
        }

        //String text property to represent the actual text typed into a given cell
        public string Text
        {
            get { return m_text; }
            set
            {
                //If the text being set is the exact same text then ignore it 
                if (m_text == value) { return; }

                //text is different therefore update cell and fire property changed event
                else
                {
                    m_text = value;
                    OnPropertyChanged(value);
                }
            }
        }

        public string ReturnName()
        {
            return (char)(colIndex + 65) + (rowIndex + 1).ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    /// 
    /// SpreadSheet Class used to create, set, and fetch data from a cell
    /// 
    public class SpreadSheet
    {
        public spreadSheetCell[,] grid;
        public int rows = 0, cols = 0;
        public Dictionary<string, string> m_Dict = new Dictionary<string, string>();
        public event PropertyChangedEventHandler CellPropertyChanged;

        //////////////////////////////////////////////////////////////////////
        //                     Undo Redo Collection                         //
        //////////////////////////////////////////////////////////////////////
        public class UndoRedo
        {

            public Stack<UndoRedoCollection> Undos = new Stack<UndoRedoCollection>();
            public Stack<UndoRedoCollection> Redos = new Stack<UndoRedoCollection>();

            //Used to clear the undo/redo stacks
            public void Clear()
            {
                Undos.Clear();
                Redos.Clear();
            }

            //pushing to the undo stack
            public void UndoPush(UndoRedoCollection toAdd)
            {
                Undos.Push(toAdd);
            }

            //Executing the undos
            public void performUndo()
            {
                UndoRedoCollection undoAction = Undos.Pop();

                foreach (UndoRedoCollection individualUndoRedo in undoAction.GetCollection())
                {
                    if (individualUndoRedo.m_text == "text")
                    {
                        //save current text
                        string temp = individualUndoRedo.m_cell.Text;
                        //reset to original text
                        individualUndoRedo.m_cell.Text = individualUndoRedo.OrigText;
                        //update original text to what we just replaced
                        individualUndoRedo.OrigText = temp;
                    }
                    else
                    {
                        //Used for undoing color changes.
                        //variable (int c) represents the color of the cell to be changed
                        int c = individualUndoRedo.m_cell.GetBGColor();

                        individualUndoRedo.m_cell.SetBGColor(individualUndoRedo.OrigColor);
                        individualUndoRedo.OrigColor = c;
                    }
                }
                //the last undo must be pushed onto the redo stack for proper functionality
                Redos.Push(undoAction);

            }

            public void performRedo()
            {
                UndoRedoCollection redoCollect;

                try { redoCollect = Redos.Pop(); }
                catch (InvalidOperationException) { return; }

                foreach (UndoRedoCollection undo_redo_Collection in redoCollect.GetCollection())
                {
                    //if the undo redo collection is of a type string
                    if (undo_redo_Collection.m_text == "text")
                    {
                        string tempS = undo_redo_Collection.m_cell.Text;
                        undo_redo_Collection.m_cell.Text = undo_redo_Collection.OrigText;
                        undo_redo_Collection.OrigText = tempS;
                    }
                    //if undo redo collection is a color
                    else
                    {
                        int tColor = undo_redo_Collection.m_cell.BGcolor;
                        undo_redo_Collection.m_cell.BGcolor = undo_redo_Collection.OrigColor;
                        undo_redo_Collection.OrigColor = tColor;
                    }
                }
                Undos.Push(redoCollect);
            }


            //Checkts to see if the undo stack is empty
            public bool UndoNotEmpty()
            {
                if (Undos.Count != 0) { return true; }
                else { return false; }
            }

            //Checcks is the redo stack is empty or not
            public bool RedoNotEmpty()
            {
                if (Redos.Count != 0) { return true; }
                else { return false; }
            }

            interface IUndoRedoCmd { IUndoRedoCmd Execute(); }

            public class UndoRedoCollection : IUndoRedoCmd
            {
                public int OrigColor = 0;
                public string OrigText = "";
                public Cell m_cell;
                public String m_text = "";

                IUndoRedoCmd temp = null;
                List<UndoRedoCollection> collect = new List<UndoRedoCollection>();

                IUndoRedoCmd IUndoRedoCmd.Execute() { return temp; }

                public UndoRedoCollection(string Input, Cell InputCell, string OldData)
                {
                    //copy data to our native cell
                    m_cell = InputCell;

                    //copy data to our native variables
                    m_text = Input;
                    OrigText = OldData;
                }

                public UndoRedoCollection(string Input, Cell InputCell, int OldColor)
                {
                    //copy data to our native cell
                    m_cell = InputCell;


                    //copy data to native variables
                    m_text = Input;
                    OrigColor = OldColor;
                }

                public UndoRedoCollection()
                { }

                public void addCollection(UndoRedoCollection input) { collect.Add(input); }

                public List<UndoRedoCollection> GetCollection() { return collect; }
            }
        }
        public void Clear()
        {
            foreach (spreadSheetCell cell in grid)
            {
                //set cell color to white and clear text (resort to default cell properties)
                cell.SetBGColor(-1);
                cell.Text = "";
            }
        }

        //Paramtarized constructed used to initialize cells in the spreadshee
        public SpreadSheet(int numRows, int numColumns)
        {
            rows = numRows;
            cols = numColumns;
            grid = new spreadSheetCell[rows, cols];

            //Initialize an array of cells
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    spreadSheetCell nCell = spreadSheetCell.createCell(i, j);
                    grid[i, j] = nCell;
                    nCell.PropertyChanged += handler;
                }
            }
        }

        public int RowCount
        {
            get { return rows; }
        }

        protected void OnCellPropertyChanged(Cell c, string text)
        {
            PropertyChangedEventHandler handler = CellPropertyChanged;

            if (handler != null)
            {
                handler(c, new PropertyChangedEventArgs(text));
            }
        }

        private void handler(object sender, PropertyChangedEventArgs e)
        {
            spreadSheetCell c = sender as spreadSheetCell;
            reloadSheet();
            setValue(c);
            reloadSheet();
            OnCellPropertyChanged(sender as Cell, e.PropertyName);
        }

        public int ColumnCount
        {
            get { return cols; }
        }

        //Return the cell at a given column and row specified by the user
        public Cell GetCell(int row, int col)
        {
            if ((row >= 50 || row < 0) && (col >= 26 || col < 0)) { return null; }

            return grid[row, col];
        }

        //Overloaded fuction similar to Cell GetCell, however this determines the location of a cell based on
        //a string parameter
        public spreadSheetCell GetCell(string s)
        {
            //Determine the A-Z value
            int col = s[0] - 65;

            //Determine the row
            int row = int.Parse(s.Substring(1)) - 1;

            return grid[row, col];
        }

        public void setValue(spreadSheetCell cell)
        {
            //we dont want to actually change the cell's text property, so we use toDelimit to store a copy
            string toDelimit = cell.Text;

            bool InvalidName = false;
            bool SelfRef = false;
            bool CircleRef = false;

            //if the cell's text starts with an "=" then we will need to perform operations on the cell
            if (toDelimit.StartsWith("="))
            {
                InvalidName = false;
                //this is an empty string that will store the new expression to be passed to the tree
                string newExpression = "";

                //This is going to help us grab all the important elements of the cell such as the constants and the variables
                var importantElements = Regex.Matches(toDelimit, @"\d+|\w+|\S").OfType<Match>().Select(m => m.Value);

                //string s will either be an operator, parenthesis, or cell name
                foreach (string s in importantElements)
                {
                    //if a letter is the first character in the string, then it is a cell name
                    if (s[0] > 64 && s[0] < 91)
                    {
                        //The following checks for a letter in the substring following the first letter
                        try
                        {
                            Int32.Parse(s.Substring(1));
                        }
                        catch (FormatException)
                        {
                            InvalidName = true;
                        }
                        //Check to make sure that the cell is within the range specific (1-50)
                        if (InvalidName == false)
                        {
                            if (Int32.Parse(s.Substring(1)) < 51 && Int32.Parse(s.Substring(1)) > 0)
                            {
                                InvalidName = false;
                            }
                            else
                                InvalidName = true;
                        }

                        if (cell.ReturnName() == s)
                        {
                            SelfRef = true;
                            break;
                        }

                        if (InvalidName == false && GetCell(s).Value != "")
                        {
                            //Check circular reference of cell about to be added
                            if (circleRef(cell.ReturnName(), s, GetCell(s).Text))
                            {
                                CircleRef = true;
                                break;
                            }

                            //If we get to this point we know we aren't adding a circular ref
                            newExpression += GetCell(s).Value;
                        }
                        //Default value, simply add to new expression
                        else if (InvalidName == false && GetCell(s).Value == "")
                        {
                            newExpression += "0";
                        }
                    }
                    else
                    {
                        newExpression += s;
                    }
                }

                if (CircleRef == false && SelfRef == false && InvalidName == false)
                {
                    ExpressionTree Exp = new ExpressionTree();
                    Exp.BuildTree(newExpression.Substring(1));
                    cell.Value = Exp.Evaluate().ToString();
                }

            }
            else
                if (InvalidName == false && SelfRef == false)
                    cell.Value = toDelimit;

            if (CircleRef)
            {
                cell.Value = "!(circular reference)";
            }
            if (InvalidName)
            {
                cell.Value = "!(bad cell name)";
            }
            if (SelfRef)
            {
                cell.Value = "!(self reference)";
            }
        }
        //Returns the names of cells referenced
        public string[] GetReferences(string name)
        {
            string[] delimits = new string[] { "=", "+", "-", "/", "*", "(", ")" };
            //Store variables and constants 
            string[] refNames = GetCell(name).Text.Split(delimits, StringSplitOptions.RemoveEmptyEntries);
            List<string> cellNames = new List<string>();

            foreach (string s in refNames)
            {
                if (s[0] > 64 && s[0] < 91)
                {
                    cellNames.Add(s);
                }
            }
            return cellNames.ToArray<string>();
        }

        //This method checks for cicular references by searching a cells contents and its references contents
        public bool circleRef(string originalCell, string currentCell, string expression)
        {
            string[] CellRefs = GetReferences(currentCell);
            string[] empty = new string[] { };

            if (CellRefs.Length == 0)
            {
                return false;
            }
            else
            {
                foreach (string s in CellRefs)
                {

                    if (s[0] > 64 && s[0] < 91)
                    {
                        if (GetCell(s).Value == "")
                            return false;

                        if (GetReferences(GetCell(s).ReturnName()) == empty)
                        {
                            return false;
                        }
                        if (s == originalCell)
                        {
                            return true;
                        }
                        //We got to a cell that does reference on other cells. So now we check to see if we have a cicular reference
                        string[] refs = GetReferences(GetCell(s).ReturnName());
                        if (refs != empty)
                        {
                            foreach (string cRef in refs)
                            {
                                //check the reference of the cell in question
                                string[] deepReference = GetReferences(cRef);
                                if (deepReference != empty)
                                {
                                    foreach (string deepCellRef in deepReference)
                                    {
                                        if(deepCellRef == originalCell)
                                        {
                                            return true;
                                        }
                                    }
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void reloadSheet()
        {
            foreach (spreadSheetCell c in grid)
            {
                setValue(c);
            }
        }
        public void setSpreadSheetCell(int row, int col, string s) { grid[row, col].Text = s; }

        public class spreadSheetCell : Cell
        {
            public spreadSheetCell() { }
            public spreadSheetCell(int row, int col) : base(row, col) { }

            public static spreadSheetCell createCell(int row, int col)
            {
                return new spreadSheetCell(row, col);
            }

            public string Value
            {
                set { m_val = value; }
                get { return m_val; }
            }
        }

        public bool Load(Stream xmlStream)
        {
            XDocument document = null;

            try { document = XDocument.Load(xmlStream); }
            catch (Exception) { return false; }

            if (null == document) { return false; }

            //Clear sheet before loading document
            Clear();

            XElement root = document.Root;

            Load(root);
            if ("Spreadsheet" != root.Name) { return false; }

            return true;
        }

        public void Load(XElement cellAttribute)
        {
            if ("Spreadsheet" != cellAttribute.Name) { return; }

            foreach (XElement child in cellAttribute.Elements("Cell"))
            {
                spreadSheetCell cell = GetCell(child.Attribute("Name").Value);

                if (null != cell)
                {
                    var text = child.Element("Text");
                    var color = child.Element("BGColor");

                    if (text != null) { cell.Text = text.Value; }

                    if (color.Value.Equals("-1")) { continue; }

                    else { cell.SetBGColor(int.Parse(color.Value)); }
                }
            }
        }

        public bool Save(Stream xmlStream)
        {
            XmlWriter writer = XmlWriter.Create(xmlStream);

            //If the stream is null then save the stream and close it. 
            if (writer != null)
            {
                Save(writer);
                writer.Close();
                return true;
            }
            return false;
        }

        public void Save(XmlWriter writer)
        {
            //makes spreasheet tag
            writer.WriteStartElement("Spreadsheet");

            var myCells = from spreadSheetCell cell in grid where cell.hasChanged() select cell;

            foreach (spreadSheetCell saveCells in myCells)
            {
                writer.WriteStartElement("Cell");
                writer.WriteAttributeString("Name", saveCells.ReturnName());
                writer.WriteElementString("Text", saveCells.Text);
                writer.WriteElementString("BGColor", saveCells.GetBGColor().ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
