using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace NetworkPort
{
    public partial class Form1 : Form
    {

        Socket socketreciver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket socketsender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5020);
        IPEndPoint remot = new IPEndPoint(IPAddress.Parse("192.168.1.2"), 5020);
        MemoryStream img_Msg = new MemoryStream();
      
        byte[] msg ;// = new byte[1024];
        byte[] msgReceive ;//= new byte[1024];
        string buffer = "";
        int Rec;
        //

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            MSGtextBox.Select();                             
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyDate)
        {
            if (keyDate == Keys.Enter)
            {
                SendButton_Click(null, null);
            }
            return base.ProcessCmdKey(ref msg, keyDate);
        }


         public void Receive()
        {
            ChatRichTextBox.Text += " " + buffer + " : He \n";
        }
        public void Send_Image()
        {
            try
            {
               // IPEndPoint remot = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 5020);
                socketsender.Connect(remot);
                string picture = "pictur";
                msg = new byte[1024];
                msg = Encoding.ASCII.GetBytes(picture);
                socketsender.Send(msg);
                pictureBox1.Image.Save(img_Msg, pictureBox1.Image.RawFormat);
                msg = new byte[img_Msg.Length];
                msg = img_Msg.GetBuffer();
                img_Msg.Close();
                socketsender.Send(msg);
                socketsender.Close();
            }
            catch (SocketException e)
            { MessageBox.Show(e.Message); }

        }
        public void Receive_Image()
        {
            try
            {
                //socket.Bind(iepany);
                //socket.Listen(-1);
                //socket.Accept();
                
                MemoryStream new_Image = new MemoryStream(msgReceive);
                pictureBox1.Image = Image.FromStream(new_Image);
                SaveIMGbutton.Visible = true;
                saveToolStripMenuItem.Visible = true;
            }
            catch (SocketException err)
            {
                MessageBox.Show(err.Message); 
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result != DialogResult.OK)
                MessageBox.Show("image con't be load because you didn't choose an image");
            else if (result == DialogResult.OK)
            {
                string current_Image = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(current_Image);
                SaveIMGbutton.Visible = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveIMGbutton_Click(null, null);
        }

        private void SendImageButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                openToolStripMenuItem_Click(null, null);
            Send_Image();
          

        }
        private void SaveIMGbutton_Click(object sender, EventArgs e)
        {
           
           if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }
       

        private void ReciveButton_Click(object sender, EventArgs e)
        {       
            EndPoint Remot = (EndPoint)ipep;    
            try
            {
                socketreciver.Bind(ipep);
                socketreciver.Listen(-1);
                socketreciver.Accept();
                while (true)
                {
                    msgReceive = new byte[1024];
                    Rec = socketreciver.Receive(msgReceive);                
                    buffer = Encoding.ASCII.GetString(msgReceive, 0, Rec);
                    
                    if (buffer == "picture")
                    {
                        Receive_Image();
                    }
                    else
                    {
                        Receive();
                    }

                }
            }
            catch (SocketException err)
            { MessageBox.Show(err.Message); }

        }

        private void SendButton_Click(object sender, EventArgs e)
        {

            ChatRichTextBox.Text += " Me :    " + MSGtextBox.Text + "\n";
            Send();
        }
        public void Send()
        {
            // ارسال رسالة
            try
            {
                socketsender.Connect(remot);
                msg = new byte[1024];
                msg = Encoding.ASCII.GetBytes(MSGtextBox.Text);
                MSGtextBox.Text="";
                MSGtextBox.Select();
                socketsender.Send(msg);
               
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image=null;
        }

    }
}