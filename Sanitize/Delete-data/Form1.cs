using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ivi.Visa;
using Ivi.Visa.FormattedIO;

namespace Delete_data
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Hide();
            groupBox2.Hide();
            button3.Hide();
            label1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Show();
            listBox1.Show();
            IEnumerable<string> devices;
            try
            {
                
                devices = GlobalResourceManager.Find();
                var every2ndElement = devices.Where((p, index) => index % 2 == 0);
                foreach (string device in every2ndElement)
                {
                    listBox1.Items.Add(device);
                }
                this.listBox1.MouseClick += new MouseEventHandler(listBox1_MouseClick);
            }
            catch (VisaException ex)
            {
                MessageBox.Show("Инструментов не найдено");
            }
        }


        void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            groupBox2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string VISA_ADDRESS = listBox1.GetItemText(listBox1.SelectedItem);
            // Create a connection (session) to the instrument
            IMessageBasedSession session;
            session = GlobalResourceManager.Open(VISA_ADDRESS) as IMessageBasedSession;
      
            // Create a formatted I/O object which will help us format the data we want to send/receive to/from the instrument
            MessageBasedFormattedIO formattedIO = new MessageBasedFormattedIO(session);

            // For Serial and TCP/IP socket connections enable the read Termination Character, or read's will timeout
            if (session.ResourceName.Contains("ASRL") || session.ResourceName.Contains("SOCKET"))
                session.TerminationCharacterEnabled = true;

            formattedIO.WriteLine(":SYSTem:SECurity:ERASeall");
            formattedIO.WriteLine(":SYSTem:SECurity:SANitize");
            formattedIO.WriteLine(":SYSTem:FILesystem:STORage:FSDCard ON|OFF|1|0");
            formattedIO.WriteLine(":SYSTem:PRESet: PERSistent");
            formattedIO.WriteLine(":SYSTem:COMMunicate:LAN:DEFaults");            formattedIO.WriteLine(":CAL:IQ:DEF");                                        
            session.Dispose();
            MessageBox.Show("Очистка заврешена");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ac = listBox1.SelectedItem.ToString();
            GetVisaChecked(ac);
            button3.Show();
        }

        private void GetVisaChecked(string visa_addr)
        {
            IMessageBasedSession session;
            try
            {
                session = GlobalResourceManager.Open(visa_addr) as IMessageBasedSession;
                richTextBox1.Text = "Соединение установлено";
            }
            catch (NativeVisaException visaException)
            {
                richTextBox1.Text = "Соединение не установлено";
            }
        }
        
    }
}
