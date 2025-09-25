namespace SkillTimeTracker
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
            components = new System.ComponentModel.Container();
            btnAddSkill = new Button();
            btnStart = new Button();
            btnStop = new Button();
            btnSave = new Button();
            listViewSkills = new ListView();
            timer1 = new System.Windows.Forms.Timer(components);
            btnRemove = new Button();
            SuspendLayout();
            // 
            // btnAddSkill
            // 
            btnAddSkill.Location = new Point(358, 67);
            btnAddSkill.Name = "btnAddSkill";
            btnAddSkill.Size = new Size(106, 73);
            btnAddSkill.TabIndex = 2;
            btnAddSkill.Text = "Add Skill";
            btnAddSkill.UseVisualStyleBackColor = true;
            btnAddSkill.Click += btnAddSkill_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(358, 294);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(106, 73);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start Timer";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(549, 294);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(118, 73);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop Timer";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(448, 177);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(122, 73);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // listViewSkills
            // 
            listViewSkills.Location = new Point(12, 12);
            listViewSkills.Name = "listViewSkills";
            listViewSkills.Size = new Size(274, 426);
            listViewSkills.TabIndex = 6;
            listViewSkills.UseCompatibleStateImageBehavior = false;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(549, 67);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(118, 64);
            btnRemove.TabIndex = 7;
            btnRemove.Text = "Remove Skill";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRemove);
            Controls.Add(listViewSkills);
            Controls.Add(btnSave);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(btnAddSkill);
            Name = "Form1";
            Text = "Skill Time Tracker";
            ResumeLayout(false);
        }

        #endregion
        private Button btnAddSkill;
        private Button btnStart;
        private Button btnStop;
        private Button btnSave;
        private ListView listViewSkills;
        private System.Windows.Forms.Timer timer1;
        private Button btnRemove;
    }
}
