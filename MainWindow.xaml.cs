using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace DiffPatchWpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Reproteq Diff Patch v1.0");
            OutputBlock.Text = "Reproteq Diff Patch v1.1  Author:TT 2021" + "\r\n";
            
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();  // current dir
            if (currentDirectory.Contains(" ")) 
            { MessageBox.Show("Error . To run this program the path does not have to contain spaces or other strange characters.please run this program from another path example C:\\Users\\yourUser\\Desktop\\DiffPatchWpf\\DiffPatchWpf.exe", "Alert"); }
           
        }

        //--------------------- btn open file1 -----------------------------------
        private void btnFile1_Click(object sender, RoutedEventArgs e)
        {
            //------------------------ <btn open file1()> -------------------
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Bin Files|*.bin|Ori Files|*.ori|All Files|*.*";
            fileDialog.DefaultExt = ".bin";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();

            if (dialogOK == true)
            {
                string sFilenames = "";
                // -------------@loop : Filenames ----------------------
                foreach (string sFilename in fileDialog.FileNames)
                {
                    //collect string
                    sFilenames += ";" + sFilename;
                }
                sFilenames = sFilenames.Substring(1); // delete first
                //  -----------------@loop : Filenames --------------
                tboxFile1.Text = sFilenames;

                OutputBlock.Text += "Loaded File1 >>> " + sFilenames + "\r\n";
            }
            //---------------------------- </ btn open file1 > ----------
        }
        // ------------------ end btn1 ---------------------------------------------



        //------------------- btn open file2 --------------------------------------
        private void btnFile2_Click(object sender, RoutedEventArgs e)
        {
            //------------------------ <btn open file2()> -------------------
            OpenFileDialog fileDialog2 = new OpenFileDialog();
            fileDialog2.Multiselect = true;
            fileDialog2.Filter = "All Files|*.*|Patch Files|*.ips |Bin Files|*.bin";
            fileDialog2.DefaultExt = ".bin";
            Nullable<bool> dialogOK2 = fileDialog2.ShowDialog();

            if (dialogOK2 == true)
            {
                string sFilenames2 = "";
                // -------------@loop : Filenames ----------------------
                foreach (string sFilename2 in fileDialog2.FileNames)
                {
                    //collect string
                    sFilenames2 += ";" + sFilename2;
                }
                sFilenames2 = sFilenames2.Substring(1); // delete first
                //  -----------------@loop : Filenames --------------
                tboxFile2.Text = sFilenames2;

                OutputBlock.Text += "Loaded File2 >>> " + sFilenames2 + "\r\n";
            }
            //---------------------------- </ btn open file2 > ----------
        }
        // ------------------ end btn2 ----------------------------------------


        //-----------------btnDiff------------------------------------------
        private void btnDiff_Click(object sender, RoutedEventArgs e)
        {
            OutputBlock.Text = "Run Diff Algo" + "\r\n";
            string quote = "\"";
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();  // current dir
            OutputBlock.Text += "Path " + currentDirectory + "\r\n"; // output view
            //var exePathDiff = currentDirectory + @"\hdiffz.exe";  // path to diff.exe    
            var exePathDiff = currentDirectory + @"\hdiffz.exe";  // path to diff.exe  
            OutputBlock.Text += "hdiffz " + exePathDiff + "\r\n"; // output viewer
            string varfile1 = tboxFile1.Text;// get tbox file1 path
            string filename1 = Path.GetFileName(varfile1);
            string strfile1 = Path.GetFileNameWithoutExtension(filename1);
            string file1pat = Path.GetDirectoryName(varfile1);
            //var patchname = currentDirectory + @"\"+ strfile1 + "-patch.ips";   // output file patchname
            var patchname = file1pat + @"\" + strfile1 + "-patch.ips";   // output file patchname            
            string varfile2 = tboxFile2.Text; // get tbox file2 path
            System.Diagnostics.Process process1;
            process1 = new System.Diagnostics.Process();
            process1.StartInfo.FileName = "cmd.exe";
            string Diffcommand = "/k start " + exePathDiff  + " -f -m-0 -C-no -s-16m " + quote + varfile1 + quote + " " + quote + varfile2 + quote+ " " + quote + patchname + quote + " & exit";
            Debug.WriteLine(Diffcommand);
            //string Diffcommand = "/k start " + exePathDiff + " -f -m-0 -C-no -s-16m " + varfile1 + " " + varfile2 + " " + patchname + " & exit";
            process1.StartInfo.Arguments = Diffcommand;
            OutputBlock.Text += "Cmd " + Diffcommand + "\r\n";
            OutputBlock.Text += "Start Diff ..." + "\r\n";
            process1.Start();              
            process1.WaitForExit(2);
            process1.Close();
            OutputBlock.Text += "End Diff Succes Ok!!  patch.ips is created" + "\r\n";
            OutputBlock.Text += "Here is Patch file " + patchname + "\r\n";
            OutputBlock.Text += "Thanks for use this tool " + "\r\n";
            MessageBox.Show("Done File Saved in " + "\r\n" + patchname , "Diff Ok!");
        }
        //-----------------end btnDiff -------------------------------------


        //-----------------btnPatch ---------------------------------------
        private void btnPatch_Click(object sender, RoutedEventArgs e)
        {
            OutputBlock.Text = "Run Patch Algo" + "\r\n";
            string quote = "\"";
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();  // current dir
            OutputBlock.Text += "Path " + currentDirectory + "\r\n"; // output view
            var exePathPatch = currentDirectory + @"\hpatchz.exe"; // path to patcher.exe         
            OutputBlock.Text += "hpatchz " + exePathPatch + "\r\n"; // output viewer
            string varfile1 = tboxFile1.Text;// get tbox file1 path
            string filename1 = Path.GetFileName(varfile1);
            string strfile1 = Path.GetFileNameWithoutExtension(filename1);
            string varfile2 = tboxFile2.Text; // get tbox file2 path
            string file2pat = Path.GetDirectoryName(varfile2);
            //var patchedfilename = currentDirectory + @"\" + strfile1 + "-patched.bin";   // output file patchnamefile
            var patchedfilename = file2pat + @"\" + strfile1 + "-patched.bin";   // output file patchnamefile
            System.Diagnostics.Process process2;
            process2 = new System.Diagnostics.Process();
            process2.StartInfo.FileName = "cmd.exe";            
            string Patchcommand = "/k start " + exePathPatch + " " + quote + varfile1 + quote + " " + quote + varfile2 + quote + " " + quote + patchedfilename + quote + " & exit";
            //string Patchcommand = "/k start " + exePathPatch + " " + varfile1 + " " + varfile2 + " " + patchedfilename + " & exit";
            process2.StartInfo.Arguments = Patchcommand;
            Debug.WriteLine(Patchcommand);
            process2.Start();
            OutputBlock.Text += "Cmd " + Patchcommand + "\r\n";
            OutputBlock.Text += "Start Patching ..." + "\r\n";
            process2.WaitForExit(2);
            process2.Close();
            OutputBlock.Text += "End Patch Succes Ok!!  patched.bin is created" + "\r\n";
            OutputBlock.Text += "Here is Patched file " + patchedfilename + "\r\n";
            OutputBlock.Text += "Thanks for use this tool " + "\r\n";
            MessageBox.Show("Done File Saved in " + "\r\n" + patchedfilename, "Patched OK!");
        }
        // ------------------ end btnPatch --------------------------------

    }
    //---------------------- end main

}
// ------------ end namespace
