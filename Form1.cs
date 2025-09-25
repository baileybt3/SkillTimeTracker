using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SkillTimeTracker
{
    public partial class Form1 : Form
    {
        private Dictionary<string, TimeSpan> skillTimes = new();
        private Stopwatch stopwatch = new();
        private string currentSkill = "";
        private string saveFile = "skills.txt";
        private System.Windows.Forms.Timer uiTimer;   // Timer for live updates

        public Form1()
        {
            InitializeComponent();
            SetupListView();
            LoadSkills();
            SetupTimer();
        }

        // === Setup ListView (columns for Skill and Time) ===
        private void SetupListView()
        {
            listViewSkills.View = View.Details;
            listViewSkills.FullRowSelect = true;
            listViewSkills.GridLines = true;
            listViewSkills.Columns.Clear();
            listViewSkills.Columns.Add("Skill", 150);
            listViewSkills.Columns.Add("Time", 100);
        }

        // === Setup UI Timer ===
        private void SetupTimer()
        {
            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000; // 1 second
            uiTimer.Tick += UiTimer_Tick;
        }

        // Timer tick: update the running skill display
        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSkill) && stopwatch.IsRunning)
            {
                // Update UI with current running time
                var runningTime = skillTimes[currentSkill] + stopwatch.Elapsed;
                UpdateListView(currentSkill, runningTime);
            }
        }

        // === Add Skill ===
        private void btnAddSkill_Click(object sender, EventArgs e)
        {
            string skill = Microsoft.VisualBasic.Interaction.InputBox("Enter new skill:", "Add Skill");
            if (!string.IsNullOrWhiteSpace(skill) && !skillTimes.ContainsKey(skill))
            {
                skillTimes[skill] = TimeSpan.Zero;
                UpdateListView();
            }
        }

        // === Start Timer ===
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (listViewSkills.SelectedItems.Count == 0) return;

            currentSkill = listViewSkills.SelectedItems[0].Text;
            stopwatch.Restart();
            uiTimer.Start();
        }

        // === Stop Timer ===
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentSkill)) return;

            stopwatch.Stop();
            uiTimer.Stop();
            skillTimes[currentSkill] += stopwatch.Elapsed;
            UpdateListView();
            stopwatch.Reset();
            currentSkill = "";
        }

        // === Save & Exit ===
        private void btnSave_Click(object sender, EventArgs e)
        {
            using StreamWriter sw = new(saveFile);
            foreach (var kv in skillTimes)
            {
                sw.WriteLine($"{kv.Key}|{kv.Value}");
            }
            Application.Exit();
        }

        // === Load saved skills ===
        private void LoadSkills()
        {
            if (!File.Exists(saveFile)) return;

            foreach (var line in File.ReadAllLines(saveFile))
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && TimeSpan.TryParse(parts[1], out var time))
                {
                    skillTimes[parts[0]] = time;
                }
            }
            UpdateListView();
        }

        // === Refresh the entire ListView ===
        private void UpdateListView()
        {
            listViewSkills.Items.Clear();
            foreach (var kv in skillTimes)
            {
                var item = new ListViewItem(kv.Key);
                item.SubItems.Add(kv.Value.ToString(@"hh\:mm\:ss"));
                listViewSkills.Items.Add(item);
            }
        }

        // === Refresh only one skill row (for live ticking) ===
        private void UpdateListView(string skill, TimeSpan runningTime)
        {
            foreach (ListViewItem item in listViewSkills.Items)
            {
                if (item.Text == skill)
                {
                    item.SubItems[1].Text = runningTime.ToString(@"hh\:mm\:ss");
                }
            }
        }

        // === Remove Skill ===
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewSkills.SelectedItems.Count == 0) return;

            string skill = listViewSkills.SelectedItems[0].Text;

            var confirm = MessageBox.Show(
                $"Are you sure you want to remove '{skill}'?",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                skillTimes.Remove(skill);
                UpdateListView();
            }
        }
    }
}
