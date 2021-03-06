﻿using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

using MyPad.Dialogs;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

//using LuaInterface;

namespace MyPad
{
    public partial class MainForm : Form
    {
        /// <remarks>
        /// ContextMenuStrip replaces ContextMenu. You can associate a ContextMenuStrip with any control, 
        /// and a right mouse click automatically displays the shortcut menu.
        /// </remarks>
        ContextMenuStrip contextMenuStrip1;

        void InitializeTabContextMenu ()
        {
            contextMenuStrip1 = new ContextMenuStrip ();
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            this.ContextMenuStrip = contextMenuStrip1;
        }

        private void contextMenuStrip1_Opening (object sender, CancelEventArgs e)
        {
            Point p = this.tabControl1.PointToClient (Cursor.Position);
            for (int i = 0; i < this.tabControl1.TabCount; i++) {
                Rectangle r = this.tabControl1.GetTabRect (i);
                if (r.Contains (p)) {
                    this.tabControl1.SelectedIndex = i; // i is the index of tab under cursor
                    contextMenuStrip1.Items.Clear ();
                                        contextMenuStrip1.Items.Add ("Закрыть", null, closeToolStripMenuItem_Click);
                    if (tabControl1.SelectedTab as EditorTabPage != null) {
                        contextMenuStrip1.Items.Add ("-");
                                                contextMenuStrip1.Items.Add ("Имя и путь файла скопировать", null, filepathnameToolStripMenuItem_Click);
                                                contextMenuStrip1.Items.Add ("Директорию файла скопировать", null, foldernameToolStripMenuItem_Click);
                    }
                    return;
                }
            }
            e.Cancel = true;
        }

        /*private void tabControl1_MouseClick (object sender, MouseEventArgs e)
        {
            return;
            if (e.Button == MouseButtons.Right) {
                //Point pt = new Point(e.X, e.Y);
                Point pt = Cursor.Position;
                Point p = this.tabControl1.PointToClient (pt);
                for (int i = 0; i < this.tabControl1.TabCount; i++) {
                    Rectangle r = this.tabControl1.GetTabRect (i);
                    if (r.Contains (p)) {
                        this.tabControl1.SelectedIndex = i; // i is the index of tab under cursor
                        var menu = new ContextMenu ();
                        menu.MenuItems.Add ("Закрыть", closeToolStripMenuItem_Click);
                        if (tabControl1.SelectedTab as EditorTabPage != null) {
                            menu.MenuItems.Add ("-");
                            menu.MenuItems.Add ("Имя и путь файла скопировать", filepathnameToolStripMenuItem_Click);
                            menu.MenuItems.Add ("Директорию файла скопировать", foldernameToolStripMenuItem_Click);
                        }
                        menu.Show (this.tabControl1, p);
                        SetupActiveTab ();
                        return;
                    }
                }
                //e.Cancel = true;
            }
        }*/

        private void filepathnameToolStripMenuItem_Click (object sender, EventArgs e)
        {
            TabPage tb = tabControl1.SelectedTab;
            if (tb as EditorTabPage == null) {
                return;
            }
            EditorTabPage etb = tb as EditorTabPage;
            Globals.TextClipboard.CopyTextToClipboard (etb.GetFileFullPathAndName (), false);
        }

        private void foldernameToolStripMenuItem_Click (object sender, EventArgs e)
        {
            TabPage tb = tabControl1.SelectedTab;
            if (tb as EditorTabPage == null) {
                return;
            }
            EditorTabPage etb = tb as EditorTabPage;

            var fileFullPathAndName = etb.GetFileFullPathAndName ();
            FileInfo fi = new FileInfo (fileFullPathAndName);
            if (fileFullPathAndName.Contains (Path.DirectorySeparatorChar)) {
                Globals.TextClipboard.CopyTextToClipboard (fi.DirectoryName, false);
            } else {
                Globals.TextClipboard.CopyTextToClipboard ("." + Path.DirectorySeparatorChar, false);
            }
        }
    }
}

