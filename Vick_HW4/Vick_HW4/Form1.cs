/* Russ vick
 * ID 11180466
 * HW # 7
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
using System.IO;
using SpreadsheetEngine;

namespace Vick_HW4
{
    public partial class Form1 : Form
    {
        SpreadSheet.UndoRedo Undo_Redo = new SpreadSheet.UndoRedo();
        SpreadSheet currSheet;

        public Form1()
        {
            //create a new spreadsheet with the given parameters
            currSheet = new SpreadSheet(50, 26);

            //Subscribe the object to fire when a cell's property has been changed
            currSheet.CellPropertyChanged += currSheet_PropertyChanged;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BuildForm();
            reloadUndo();
            ReloadRedo();
        }

        //Used to set up spreadsheet to initialize the columns and rows
        public void BuildForm()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < 26; i++)
            {
                dataGridView1.Columns.Add(alphabet[i].ToString(), alphabet[i].ToString());
            }
            dataGridView1.Rows.Add(50);
            for (int i = 0; i < 50; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dataGridView1_CellBeginEdit_1(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (null == currSheet) { return; }

            else
            {
                DataGridViewCell gridCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                SpreadsheetEngine.Cell spreadCell = currSheet.GetCell(e.RowIndex, e.ColumnIndex);
                gridCell.Value = spreadCell.Text;
            }
        }


        private void dataGridView1_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            string originalCellText = "";
            if (null == currSheet) { return; }
            //Var (undoRedoPush) holds a collection of undo and redo commands

            DataGridViewCell UIcell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            SpreadSheet.UndoRedo.UndoRedoCollection undoRedoPush = new SpreadSheet.UndoRedo.UndoRedoCollection();
            SpreadsheetEngine.Cell dataCell = currSheet.GetCell(e.RowIndex, e.ColumnIndex);

            originalCellText = dataCell.Text;
            //save original text


            //Set text for the UI cell and trigger the event to update the cell value
            if (null == UIcell.Value) { dataCell.Text = string.Empty; }

            else { dataCell.Text = UIcell.Value.ToString(); }

            //Set the value for the UI cell to the cell value
            UIcell.Value = dataCell.Value;

            undoRedoPush.addCollection(new SpreadSheet.UndoRedo.UndoRedoCollection("text", dataCell, originalCellText));

            reloadEntireSheet();
            Undo_Redo.UndoPush(undoRedoPush);
            ReloadRedo();
            reloadUndo();
        }

        //Maybe one of the worst functions ever created...but it works =)
        public void reloadEntireSheet()
        {
            foreach (Cell x in currSheet.grid)
            {
                currSheet.reloadSheet();
            }

            //updating UI cells in the sheet since the data-layer cell-values have most-likely changed
            foreach (Cell x in currSheet.grid)
            {
                dataGridView1.Rows[x.RowIndex].Cells[x.ColumnIndex].Value = x.Value;
            }
        }

        private void changeColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog colorD = new ColorDialog();
            colorD.AllowFullOpen = true;
            colorD.AnyColor = true;
            colorD.ShowDialog();

            SpreadSheet.UndoRedo.UndoRedoCollection pushUndoRedo = new SpreadSheet.UndoRedo.UndoRedoCollection();

            //converting color to uint for storage in the data-layer cell
            int color = (int)colorD.Color.ToArgb();

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                SpreadsheetEngine.Cell dataCell = currSheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                //add to undo
                pushUndoRedo.addCollection(new SpreadSheet.UndoRedo.UndoRedoCollection
                    ("color", currSheet.GetCell(cell.RowIndex, cell.ColumnIndex),
                    currSheet.GetCell(cell.RowIndex, cell.ColumnIndex).GetBGColor()));

                dataCell.SetBGColor(color);
                BackColorChanged += currSheet_PropertyChanged;
            }
            Undo_Redo.UndoPush(pushUndoRedo);
            reloadUndo();
            ReloadRedo();
        }

        private void currSheet_PropertyChanged(object sender, EventArgs e)
        {
            SpreadSheet.spreadSheetCell c = sender as SpreadSheet.spreadSheetCell;

            if (c != null)
            {
                dataGridView1.Rows[c.RowIndex].Cells[c.ColumnIndex].Value = c.Value;
                Color bgColor = Color.FromArgb(c.GetBGColor());
                //dataGridView1.Rows[cell.ColumnIndex].Cells[cell.RowIndex].Style.BackColor = bg;
                dataGridView1.Rows[c.RowIndex].Cells[c.ColumnIndex].Style.BackColor = bgColor;
                //add cell to the dictionary/update its value within the dictionary of known values
                currSheet.m_Dict[c.ReturnName()] = c.Value;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo_Redo.performUndo();
            ReloadRedo();
            reloadUndo();

            reloadEntireSheet();

            TextChanged += currSheet_PropertyChanged;
            BackColorChanged += currSheet_PropertyChanged;
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo_Redo.performRedo();
            reloadUndo();
            ReloadRedo();

            reloadEntireSheet();

            TextChanged += currSheet_PropertyChanged;
            BackColorChanged += currSheet_PropertyChanged;
        }

        public void reloadUndo()
        {
            if (Undo_Redo.UndoNotEmpty()) { undoToolStripMenuItem.Enabled = true; }
            else { undoToolStripMenuItem.Enabled = false; }
        }

        public void ReloadRedo()
        {
            if (Undo_Redo.RedoNotEmpty()) { redoToolStripMenuItem.Enabled = true; }
            else { redoToolStripMenuItem.Enabled = false; }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "*.xml";
            saveFile.Filter = "XML file (*.xml) | *.xml";

            saveFile.RestoreDirectory = true;

            //Show the save dialog box to user and open the file to which the document will be saved
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Stream s = saveFile.OpenFile();
                currSheet.Save(s);
                s.Dispose();
                s.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.FileName = "*.xml";
            openFile.Filter = "XML file (*xml) |*.xml";
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Stream s = openFile.OpenFile();
                currSheet.Load(s);
                //updates all neccessary cells
                reloadEntireSheet();
                //clear stream and close
                s.Dispose();
                s.Close();
            }
        }
    }
}