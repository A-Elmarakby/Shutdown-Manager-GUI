using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ShutdownTimerGUI
{
    public class MainForm : Form
    {
        private Label label;
        private TextBox textBox;
        private Button btnOK;
        private Button btnCancel;
        private Button btnInfo;

        public MainForm()
        {
            // Setup Main Window
            this.Text = "Shutdown Timer";
            this.Size = new Size(330, 230);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Font = new Font("Segoe UI", 10);

            // Label Setup
            label = new Label();
            label.Text = "Enter shutdown time (in hours):";
            label.Location = new Point(20, 15);
            label.AutoSize = true;

            // Text Box Setup
            textBox = new TextBox();
            textBox.Location = new Point(20, 45);
            textBox.Size = new Size(275, 25);
            textBox.Text = "1";

            // Start Button
            btnOK = new Button();
            btnOK.Text = "Start Timer";
            btnOK.Location = new Point(20, 85);
            btnOK.Size = new Size(130, 35);
            btnOK.BackColor = Color.FromArgb(0, 120, 215);
            btnOK.ForeColor = Color.White;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Click += new EventHandler(BtnOK_Click);

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel Timer";
            btnCancel.Location = new Point(165, 85);
            btnCancel.Size = new Size(130, 35);
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Info Button
            btnInfo = new Button();
            btnInfo.Text = "Info (LinkedIn)";
            btnInfo.Location = new Point(20, 130);
            btnInfo.Size = new Size(275, 35);
            btnInfo.BackColor = Color.FromArgb(10, 102, 194);
            btnInfo.ForeColor = Color.White;
            btnInfo.FlatStyle = FlatStyle.Flat;
            btnInfo.Click += new EventHandler(BtnInfo_Click);

            // Add elements to the form
            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
            this.Controls.Add(btnInfo);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                double hours = Convert.ToDouble(textBox.Text);
                int seconds = (int)Math.Round(hours * 3600);

                // Execute shutdown command with -f to force close applications
                ProcessStartInfo psi = new ProcessStartInfo("shutdown", "-s -f -t " + seconds);
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);

                // Play success sound
                Console.Beep(1000, 200);
                Console.Beep(1500, 200);

                // Show success message
                MessageBox.Show("The computer will shut down in " + hours + " hour(s).", "A-Elmarakby", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch
            {
                // Play error sound
                Console.Beep(400, 300);
                Console.Beep(400, 300);

                // Show error message
                MessageBox.Show("Please enter numbers only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Cancel the shutdown timer
            ProcessStartInfo psi = new ProcessStartInfo("shutdown", "-a");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);

            // Play cancel sound
            Console.Beep(1500, 200);
            Console.Beep(1000, 200);

            // Show cancel message
            MessageBox.Show("Shutdown timer has been cancelled.", "A-Elmarakby", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            // Play info sound
            Console.Beep(523, 150);
            Console.Beep(659, 150);
            Console.Beep(784, 150);
            Console.Beep(1046, 200);

            // Open LinkedIn profile
            ProcessStartInfo psi = new ProcessStartInfo("https://www.linkedin.com/in/a-elmarakby");
            psi.UseShellExecute = true;
            Process.Start(psi);
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}