﻿/***
    BSD 2-Clause License

    Copyright (c) 2017, Mahmoud M. AbdelRahman
 *  arch.mahmoud.ouf111@gmail.com
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this
      list of conditions and the following disclaimer.

    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
    AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
    FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
    SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
    CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
    OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
    OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 ***/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;



namespace GH_CPython
{

    public partial class PythonShell : Form
    {

      
        //styles
        TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
        TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
        MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));
        TextStyle OrangeStyle = new TextStyle(Brushes.Orange, null, FontStyle.Regular);

        SaveFileDialog saveas = new SaveFileDialog();
        

        public string currentFileName = string.Empty;


        //public List<string> PsinputData = new List<string>();
        public PythonShell()
        {
            InitializeComponent();
        }

        public string output { get; set; }

        public string theshell { get; set; }

        private void PythonCanvas_Load(object sender, EventArgs e)
        {
            
        }

        private void InitStylesPriority()
        {
            //add this style explicitly for drawing under other styles
            PythonCanvas.AddStyle(SameWordsStyle);
        }

        private void PythonCanvas_TextChanged(object sender, TextChangedEventArgs e)
        {
            CSharpSyntaxHighlight(e);
        }

        string f = "\"\"\"";
        private void CSharpSyntaxHighlight(TextChangedEventArgs e)
        {
            PythonCanvas.LeftBracket = '(';
            PythonCanvas.RightBracket = ')';
            PythonCanvas.LeftBracket2 = '\x0';
            PythonCanvas.RightBracket2 = '\x0';
            //clear style of changed range
            e.ChangedRange.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, GreenStyle, BrownStyle, OrangeStyle);

            e.ChangedRange.SetStyle(MagentaStyle, @"(?<=def )(.*)(?=\()");
            e.ChangedRange.SetStyle(GrayStyle, @"(\[desc\])(.*?)(\[\/desc\])",RegexOptions.Singleline);

            //comment highlighting
            e.ChangedRange.SetStyle(GreenStyle, @"#.*$", RegexOptions.Multiline);

            //attribute highlighting
            e.ChangedRange.SetStyle(GrayStyle, @"^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline);
            //class name highlighting
            e.ChangedRange.SetStyle(BoldStyle, @"\b(class|struct|enum|interface)\s+(?<range>\w+?)\b");
            //keyword highlighting
            e.ChangedRange.SetStyle(BlueStyle, @"\b(elif|except|print|import|and|not|from |in |do |if|import *|def|abstract|as|self|bool|byte|case|catch|char|checked|class|const|pass|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while|add|alias|ascending|descending|dynamic|from|get|global|group|into|join|let|orderby|partial|remove|select|set|value|var|where|yield)\b|#region\b|#endregion\b");
            e.ChangedRange.SetStyle(MagentaStyle, @"\b(True|False|None)");

            //clear folding markers
            e.ChangedRange.ClearFoldingMarkers();
            
            //set folding markers
            e.ChangedRange.SetFoldingMarkers("{", "}");//allow to collapse brackets block
            //e.ChangedRange.SetFoldingMarkers(@"#region\b", @"#endregion\b");//allow to collapse #region blocks
            e.ChangedRange.SetFoldingMarkers(@"'''", @"'''");//allow to collapse comment block
            e.ChangedRange.SetFoldingMarkers(f, f,RegexOptions.Singleline|RegexOptions.RightToLeft);//allow to collapse comment block
        }

        string sorry = "Sorry!\nThis plugin is under developement \nThis feature is still in progress, stay tuned\n If you wish to contribute to the developemnt of this component, you are most welcome: \n        https://github.com/MahmoudAbdelRahman/GH_CPython";
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(currentFileName != string.Empty)
            {
                File.WriteAllText(currentFileName, PythonCanvas.Text);
            }else
            {
                saveas.Filter = "Python Files (*.py)|*.py| Text File (*.txt)|*.txt ";  //Text Files (*.txt)|*.txt|All Files (*.*)|*.*
                
                if(saveas.ShowDialog()== System.Windows.Forms.DialogResult.OK)
                {
                    currentFileName = saveas.FileName;
                    File.WriteAllText(currentFileName, PythonCanvas.Text);
                }
            }
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveas.Filter = "Python Files (*.py)|*.py| Text File (*.txt)|*.txt ";  //Text Files (*.txt)|*.txt|All Files (*.*)|*.*

            if (saveas.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentFileName = saveas.FileName;
                File.WriteAllText(currentFileName, PythonCanvas.Text);
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult mm = MessageBox.Show("Are you sure that you would like to reset this file? \nThis will erease everything!","Reset!", MessageBoxButtons.YesNo);
            if (mm == System.Windows.Forms.DialogResult.Yes)
            {
                PythonCanvas.Text = string.Empty;
            }
        }

        private void chooseInterpreterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(sorry);
            locatePython locPy = new locatePython();
            locPy.ShowDialog();
        }

        private void pipInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(sorry);
        }

        private void pythonShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(sorry);
        }

        private void openPythonFileItem_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Python file|*.py";
            openFileDialog1.Title = "Open Python File";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFileName = openFileDialog1.FileName;
                // Assign the cursor in the Stream to the Form's Cursor property.
                string openedfile = File.ReadAllText(openFileDialog1.FileName).Replace("\t", "    ");
                PythonCanvas.Text = openedfile;
            }
        }


       
    }


}
