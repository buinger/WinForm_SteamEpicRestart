namespace AutoRestartExe
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            proName_Input = new TextBox();
            proName = new Label();
            runTime = new Label();
            runTime_Input = new TextBox();
            waitTime = new Label();
            waitTime_Input = new TextBox();
            startButton = new Button();
            gameId_Input = new TextBox();
            gameIdToogle = new CheckBox();
            infoBox = new RichTextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // proName_Input
            // 
            proName_Input.Location = new Point(12, 75);
            proName_Input.Name = "proName_Input";
            proName_Input.Size = new Size(198, 38);
            proName_Input.TabIndex = 0;
            // 
            // proName
            // 
            proName.AutoSize = true;
            proName.Location = new Point(12, 22);
            proName.Name = "proName";
            proName.Size = new Size(198, 31);
            proName.TabIndex = 1;
            proName.Text = "进程名(无需后缀)";
            // 
            // runTime
            // 
            runTime.AutoSize = true;
            runTime.Location = new Point(229, 22);
            runTime.Name = "runTime";
            runTime.Size = new Size(150, 31);
            runTime.TabIndex = 3;
            runTime.Text = "运行时间(分)";
            // 
            // runTime_Input
            // 
            runTime_Input.Location = new Point(229, 75);
            runTime_Input.Name = "runTime_Input";
            runTime_Input.Size = new Size(198, 38);
            runTime_Input.TabIndex = 2;
            // 
            // waitTime
            // 
            waitTime.AutoSize = true;
            waitTime.Location = new Point(229, 132);
            waitTime.Name = "waitTime";
            waitTime.Size = new Size(150, 31);
            waitTime.TabIndex = 5;
            waitTime.Text = "休息时间(分)";
            // 
            // waitTime_Input
            // 
            waitTime_Input.Location = new Point(229, 185);
            waitTime_Input.Name = "waitTime_Input";
            waitTime_Input.Size = new Size(198, 38);
            waitTime_Input.TabIndex = 4;
            // 
            // startButton
            // 
            startButton.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            startButton.Location = new Point(454, 184);
            startButton.Name = "startButton";
            startButton.Size = new Size(216, 39);
            startButton.TabIndex = 6;
            startButton.Text = "开始监控";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButton_Click;
            // 
            // gameId_Input
            // 
            gameId_Input.Location = new Point(12, 184);
            gameId_Input.Name = "gameId_Input";
            gameId_Input.Size = new Size(198, 38);
            gameId_Input.TabIndex = 7;
            gameId_Input.TextChanged += textBox1_TextChanged;
            // 
            // gameIdToogle
            // 
            gameIdToogle.AutoSize = true;
            gameIdToogle.Location = new Point(12, 131);
            gameIdToogle.Name = "gameIdToogle";
            gameIdToogle.Size = new Size(191, 35);
            gameIdToogle.TabIndex = 11;
            gameIdToogle.Text = "Steam游戏ID";
            gameIdToogle.UseVisualStyleBackColor = true;
            gameIdToogle.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // infoBox
            // 
            infoBox.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            infoBox.Location = new Point(454, 22);
            infoBox.Name = "infoBox";
            infoBox.ReadOnly = true;
            infoBox.Size = new Size(216, 141);
            infoBox.TabIndex = 12;
            infoBox.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(468, 242);
            label1.Name = "label1";
            label1.Size = new Size(203, 31);
            label1.TabIndex = 13;
            label1.Text = "q群：756479074";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(683, 282);
            Controls.Add(label1);
            Controls.Add(infoBox);
            Controls.Add(gameIdToogle);
            Controls.Add(gameId_Input);
            Controls.Add(startButton);
            Controls.Add(waitTime);
            Controls.Add(waitTime_Input);
            Controls.Add(runTime);
            Controls.Add(runTime_Input);
            Controls.Add(proName);
            Controls.Add(proName_Input);
            Name = "Form1";
            Text = "HM watchdog";
            FormClosed += Form1_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox proName_Input;
        private Label proName;
        private Label runTime;
        private TextBox runTime_Input;
        private Label waitTime;
        private TextBox waitTime_Input;
        private Button startButton;
        private TextBox gameId_Input;
        private CheckBox gameIdToogle;
        private RichTextBox infoBox;
        private Label label1;
    }
}