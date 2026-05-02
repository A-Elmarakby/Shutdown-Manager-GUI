using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

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
            // Set up the main window
            this.Text = "Shutdown Timer";
            this.Size = new Size(330, 230);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Set dark background color
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Font = new Font("Segoe UI", 10);

            // Set up the text label
            label = new Label();
            label.Text = "Enter shutdown time (in hours):";
            label.Location = new Point(20, 15);
            label.AutoSize = true;
            label.ForeColor = Color.White;

            // Set up the text box
            textBox = new TextBox();
            textBox.Location = new Point(20, 45);
            textBox.Size = new Size(275, 25);
            textBox.Text = "1";
            textBox.BackColor = Color.FromArgb(50, 50, 50);
            textBox.ForeColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;

            // Set up Start button (Cyan color)
            btnOK = new Button();
            btnOK.Text = "Start Timer";
            btnOK.Location = new Point(20, 85);
            btnOK.Size = new Size(130, 35);
            btnOK.BackColor = Color.FromArgb(0, 123, 167); 
            btnOK.ForeColor = Color.White;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += new EventHandler(BtnOK_Click);

            // Set up Cancel button (Magenta color)
            btnCancel = new Button();
            btnCancel.Text = "Cancel Timer";
            btnCancel.Location = new Point(165, 85);
            btnCancel.Size = new Size(130, 35);
            btnCancel.BackColor = Color.FromArgb(178, 0, 89); 
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Set up Info button (Dark grey color)
            btnInfo = new Button();
            btnInfo.Text = "Info (LinkedIn)";
            btnInfo.Location = new Point(20, 130);
            btnInfo.Size = new Size(275, 35);
            btnInfo.BackColor = Color.FromArgb(70, 70, 70);
            btnInfo.ForeColor = Color.White;
            btnInfo.FlatStyle = FlatStyle.Flat;
            btnInfo.FlatAppearance.BorderSize = 0;
            btnInfo.Click += new EventHandler(BtnInfo_Click);

            // Add all parts to the window
            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
            this.Controls.Add(btnInfo);

            // Run silent install when the program starts
            this.Load += new EventHandler(MainForm_Load);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PerformSilentInstall();
        }

        private void PerformSilentInstall()
        {
            string registryKeyPath = @"Software\Elmarakby\ShutdownTimer";
            string hidePromptFlag = "DontShowPrompt";

            try
            {
                // 1. Get safe folder path
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string myAppFolder = Path.Combine(appDataFolder, "ShutdownTimerGUI");
                string safeExePath = Path.Combine(myAppFolder, "ShutdownTimer.exe");
                string currentExePath = Application.ExecutablePath;

                // Make the folder if it is not there
                if (!Directory.Exists(myAppFolder))
                {
                    Directory.CreateDirectory(myAppFolder);
                }

                // Copy the program and make a shortcut if running from outside the safe folder
                if (currentExePath.ToLower() != safeExePath.ToLower())
                {
                    File.Copy(currentExePath, safeExePath, true);

                    string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string shortcutPath = Path.Combine(desktopFolder, "Shutdown Timer.lnk");

                    // PowerShell code to make the shortcut with the app icon
                    string psCommand = $"$wshell = New-Object -ComObject WScript.Shell; $shortcut = $wshell.CreateShortcut('{shortcutPath}'); $shortcut.TargetPath = '{safeExePath}'; $shortcut.WorkingDirectory = '{myAppFolder}'; $shortcut.IconLocation = '{safeExePath},0'; $shortcut.Save()";

                    ProcessStartInfo psInfo = new ProcessStartInfo("powershell.exe", "-NoProfile -ExecutionPolicy Bypass -Command \"" + psCommand + "\"");
                    psInfo.CreateNoWindow = true;
                    psInfo.UseShellExecute = false;
                    
                    var process = Process.Start(psInfo);
                    process.WaitForExit(); 
                }

                // 2. Check if user checked "Don't show this again" before
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath))
                {
                    if (key != null && key.GetValue(hidePromptFlag) != null && key.GetValue(hidePromptFlag).ToString() == "1")
                    {
                        return; // Stop here, do not show the message
                    }
                }

                // 3. Show the message to the user
                ShowSuggestionPrompt();
            }
            catch (Exception ex)
            {
                // Print error but do not stop the app
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        private void ShowSuggestionPrompt()
        {
            string registryKeyPath = @"Software\Elmarakby\ShutdownTimer";
            string hidePromptFlag = "DontShowPrompt";

            // Make a small window for the message
            Form prompt = new Form()
            {
                Width = 380,
                Height = 280, // تم زيادة ارتفاع النافذة
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "App Ready",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White
            };

            // Simple A1 English text
            Label textLabel = new Label()
            {
                Left = 20,
                Top = 20,
                Width = 340,
                Height = 130, // تم زيادة مساحة النص ليظهر بالكامل
                Text = "Hello, my friend! 👋\n\nFor quicker access, you can pin the app to your taskbar:\nRight-click the Desktop shortcut and choose 'Pin to taskbar'.\n\nYou can safely delete the downloaded file.",
                Font = new Font("Segoe UI", 10)
            };

            // Checkbox to hide message next time
            CheckBox chkDontShow = new CheckBox()
            {
                Left = 20,
                Top = 150, // تم تنزيل المربع لتحت
                Width = 300,
                Text = "Don't show this again",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray
            };

            // OK button (Cyan color)
            Button confirmation = new Button()
            {
                Text = "OK",
                Left = 130,
                Top = 185, // تم تنزيل الزرار لتحت
                Width = 100,
                Height = 35,
                BackColor = Color.FromArgb(0, 123, 167), 
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            confirmation.FlatAppearance.BorderSize = 0;

            // When OK button is clicked
            confirmation.Click += (sender, e) =>
            {
                if (chkDontShow.Checked)
                {
                    // Save user choice in Registry
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(registryKeyPath))
                    {
                        key.SetValue(hidePromptFlag, "1");
                    }
                }
                prompt.Close();
            };

            // Add parts to the small window
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(chkDontShow);
            prompt.Controls.Add(confirmation);

            // Show the small window
            prompt.ShowDialog();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                double hours = Convert.ToDouble(textBox.Text);
                int seconds = (int)Math.Round(hours * 3600);

                // Run Windows shutdown command
                ProcessStartInfo psi = new ProcessStartInfo("shutdown", "-s -f -t " + seconds);
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);

                // Play success sound
                Console.Beep(1000, 200);
                Console.Beep(1500, 200);

                MessageBox.Show("The computer will shut down in " + hours + " hour(s).", "A-Elmarakby", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch
            {
                // Play error sound
                Console.Beep(400, 300);
                Console.Beep(400, 300);
                MessageBox.Show("Please enter numbers only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Cancel Windows shutdown
            ProcessStartInfo psi = new ProcessStartInfo("shutdown", "-a");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);

            // Play cancel sound
            Console.Beep(1500, 200);
            Console.Beep(1000, 200);

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

            // Open LinkedIn link
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