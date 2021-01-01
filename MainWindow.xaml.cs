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
            OutputBlock.Text = "Reproteq Diff Patch v1.2  Author:TT 2021" + "\r\n";     

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
            string saltli = "\r\n";

            string varfile1 = tboxFile1.Text;// get tbox file1 path
            string filename1 = Path.GetFileName(varfile1);
            string strfile1 = Path.GetFileNameWithoutExtension(filename1);
            string file1pat = Path.GetDirectoryName(varfile1);
            var patchname = file1pat + @"\" + strfile1 + "-patch.txt";   // output file patchname            
            string varfile2 = tboxFile2.Text; // get tbox file2 path
            OutputBlock.Text += "Comparing ... " + saltli;            

            //------------- bytes ori
            byte[] Bytes_Ori;
            using (StreamReader sr = new StreamReader(varfile1))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(ms);
                    Bytes_Ori = ms.ToArray();
                }
            }
            //-------------end bytes ori

            //---------------bytes mod
            byte[] Bytes_Mod;
            using (StreamReader sr = new StreamReader(varfile2))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(ms);
                    Bytes_Mod = ms.ToArray();
                }
            }
            //---------------endbytes mod

            //------------------ outputblock
            for (int i = 0; i < Bytes_Ori.Length; i++)
            {
                if (Bytes_Ori[i] != Bytes_Mod[i])
                {
                    OutputBlock.Text += "Addr: 0x" + i.ToString("X2") + "  Ori: 0x" + Bytes_Ori[i].ToString("X2") + "  Mod: 0x" + Bytes_Mod[i].ToString("X2") + saltli;                
                }
            }
            //------------------end outputblock


            //----------------- output file
            using (var sw = new StreamWriter(patchname))
            {
                for (int i = 0; i < Bytes_Ori.Length; i++)
                {
                    if (Bytes_Ori[i] != Bytes_Mod[i])
                    {
                        //sw.WriteLine("0x" + i.ToString("X2") + " 0x"+ Bytes_Mod[i].ToString("X2"), i);
                        sw.WriteLine(i.ToString() + " 0x" + Bytes_Mod[i].ToString("X2"), i);
                        //sw.WriteLine(i.ToString() + " " + Bytes_Mod[i].ToString("X2"), i);
                    }
                }               
            }
            //---------------- end output file


            OutputBlock.Text += "End Patch Succes Ok!!  patch.txt is created" + saltli;
            OutputBlock.Text += "Here is Patch file " + patchname + saltli;
            OutputBlock.Text += "Thanks for use this tool created by TT 2021" + saltli;
            MessageBox.Show("Done File Saved in " + "\r\n" + patchname , " Pacht created Ok!");
        }
        //-----------------end btnDiff -------------------------------------



        //-----------------btnPatch ---------------------------------------
        private void btnPatch_Click(object sender, RoutedEventArgs e)
        {
            OutputBlock.Text = "Run Patch Algo" + "\r\n";
            string saltli = "\r\n";
            string varfile1 = tboxFile1.Text;// get tbox file1 path
            string filename1 = Path.GetFileName(varfile1);
            string strfile1 = Path.GetFileNameWithoutExtension(filename1);
            string varfile2 = tboxFile2.Text; // get tbox file2 path
            string file2pat = Path.GetDirectoryName(varfile2);          
            var patchedfilename = file2pat + @"\" + strfile1 + "-patched.bin";   // output file patchnamefile
            OutputBlock.Text += "Patching ... " + saltli;

            //------------- bytes ori2
           
            byte[] Bytes_Ori;
            using (StreamReader sr = new StreamReader(varfile1))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(ms);
                    Bytes_Ori = ms.ToArray();    
                }
            }
            //-------------end bytes ori




            //----------------- output file

            using (FileStream stream = new FileStream(patchedfilename, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                for (int i = 0; i < Bytes_Ori.Length; i++)
                {
                    writer.Write((byte)Bytes_Ori[i]);
                  //  OutputBlock.Text += "writing" + saltli;
                }
            }


            //---------------- end output file

            //-----------read file patch
            string[] lines;

            var list = new List<string>();
            var fileStream = new FileStream(varfile2, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                   // OutputBlock.Text += line + saltli;


                    string myString = line;
                    var strpart1 = myString.Substring(0, myString.IndexOf(' '));
                    var strpart2 = myString.Substring(myString.IndexOf(' ')); 
                    string hex = strpart2;
                    string hexTrim = String.Concat(hex.Where(c => !Char.IsWhiteSpace(c)));
                    int value = Convert.ToInt32(hexTrim, 16);
                    byte byteVal = Convert.ToByte(value);
                    OutputBlock.Text += "Addr"+  strpart1 + " Val" +strpart2+ saltli;
                    //------------------change values in positions
                    using (var stream = new FileStream(patchedfilename, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = Int32.Parse(strpart1);// long 
                        stream.WriteByte(byteVal); // byte
                    }

                    //------------------end change values in positions


                }               

                OutputBlock.Text += "End Patch Succes Ok!!  patched.bin is created" + saltli;
                OutputBlock.Text += "Here is Patched file " + patchedfilename + saltli;
                OutputBlock.Text += "Thanks for use this tool created by TT 2021" + saltli;

                OutputBlock.Text += "Acepted do";
                MessageBox.Show("Done File Saved in " + saltli + patchedfilename, "Patched OK!");

            }
            lines = list.ToArray();

            //---------------end read file patch


        }


        // ------------------ end btnPatch --------------------------------


        //-----------menu headeer
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string saltli = "\r\n";
            string wallet = "1Mmwhdw4mQzbuLbmPFdEF2uXMVi8X3kv68";           
            var response = MessageBox.Show("Donate BTC " + saltli + saltli + wallet + saltli + saltli + "Paypal reproteq@gmail.com" + saltli + saltli + "Copyright 2021 Reproteq® \r\n\r\nCopy adress btc to clipboard?", "", MessageBoxButton.YesNo);  if (response == MessageBoxResult.Yes) { Clipboard.SetText(wallet); }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //-----------end menu header




    }
    //---------------------- end main

}
// ------------ end namespace
