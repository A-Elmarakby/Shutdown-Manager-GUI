<# :
@echo off
powershell -NoProfile -ExecutionPolicy Bypass -Command "iex ((Get-Content '%~f0') -join \"`n\")"
exit /b
#>

# Load Windows Forms and Drawing libraries
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

[System.Windows.Forms.Application]::EnableVisualStyles()

# Setup Main Window
$form = New-Object System.Windows.Forms.Form
$form.Text = "Shutdown Timer"
$form.Size = New-Object System.Drawing.Size(330, 230)
$form.StartPosition = "CenterScreen"
$form.FormBorderStyle = 'FixedDialog'
$form.MaximizeBox = $false
$form.BackColor = [System.Drawing.Color]::FromArgb(245, 245, 245)

$font = New-Object System.Drawing.Font("Segoe UI", 10)
$form.Font = $font

# Label Setup
$label = New-Object System.Windows.Forms.Label
$label.Text = "Enter shutdown time (in hours):"
$label.Location = New-Object System.Drawing.Point(20, 15)
$label.AutoSize = $true

# Text Box Setup
$textBox = New-Object System.Windows.Forms.TextBox
$textBox.Location = New-Object System.Drawing.Point(20, 45)
$textBox.Size = New-Object System.Drawing.Size(275, 25)
$textBox.Text = "1"

# Start Button (Blue)
$btnOK = New-Object System.Windows.Forms.Button
$btnOK.Text = "Start Timer"
$btnOK.Location = New-Object System.Drawing.Point(20, 85)
$btnOK.Size = New-Object System.Drawing.Size(130, 35)
$btnOK.BackColor = [System.Drawing.Color]::FromArgb(0, 120, 215)
$btnOK.ForeColor = [System.Drawing.Color]::White
$btnOK.FlatStyle = 'Flat'

# Cancel Button (Red)
$btnCancel = New-Object System.Windows.Forms.Button
$btnCancel.Text = "Cancel Timer"
$btnCancel.Location = New-Object System.Drawing.Point(165, 85)
$btnCancel.Size = New-Object System.Drawing.Size(130, 35)
$btnCancel.BackColor = [System.Drawing.Color]::FromArgb(220, 53, 69)
$btnCancel.ForeColor = [System.Drawing.Color]::White
$btnCancel.FlatStyle = 'Flat'

# Info / LinkedIn Button (LinkedIn Blue)
$btnInfo = New-Object System.Windows.Forms.Button
$btnInfo.Text = "Info (LinkedIn)"
$btnInfo.Location = New-Object System.Drawing.Point(20, 130)
$btnInfo.Size = New-Object System.Drawing.Size(275, 35)
$btnInfo.BackColor = [System.Drawing.Color]::FromArgb(10, 102, 194)
$btnInfo.ForeColor = [System.Drawing.Color]::White
$btnInfo.FlatStyle = 'Flat'

# Start Button Actions
$btnOK.Add_Click({
    $hours = $textBox.Text
    try {
        $seconds = [math]::Round([double]$hours * 3600)
        & shutdown -s -t $seconds
        # Play Custom Success Sound (Rising tones)
        [console]::beep(1000, 200)
        [console]::beep(1500, 200)
        [System.Windows.Forms.MessageBox]::Show("The computer will shut down in $hours hour(s).", "A-Elmarakby", 0, [System.Windows.Forms.MessageBoxIcon]::Information)
        $form.Close()
    } catch {
        # Play Custom Error Sound (Low buzz)
        [console]::beep(400, 300)
        [console]::beep(400, 300)
        [System.Windows.Forms.MessageBox]::Show("Please enter numbers only.", "Error", 0, [System.Windows.Forms.MessageBoxIcon]::Error)
    }
})

# Cancel Button Actions
$btnCancel.Add_Click({
    & shutdown -a
    # Play Custom Cancel Sound (Falling tones)
    [console]::beep(1500, 200)
    [console]::beep(1000, 200)
    [System.Windows.Forms.MessageBox]::Show("Shutdown timer has been cancelled.", "A-Elmarakby", 0, [System.Windows.Forms.MessageBoxIcon]::Information)
    $form.Close()
})

# Info Button Actions
$btnInfo.Add_Click({
    # Play a Happy/Pleasing Sound (Musical chord arpeggio)
    [console]::beep(523, 150)
    [console]::beep(659, 150)
    [console]::beep(784, 150)
    [console]::beep(1046, 200)
    # Open LinkedIn Profile
    Start-Process "https://www.linkedin.com/in/a-elmarakby"
})

# Add controls to the form and display it
$form.Controls.Add($label)
$form.Controls.Add($textBox)
$form.Controls.Add($btnOK)
$form.Controls.Add($btnCancel)
$form.Controls.Add($btnInfo)

$form.ShowDialog() | Out-Null
