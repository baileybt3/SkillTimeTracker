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

            // Enable drag/drop reordering
            listViewSkills.AllowDrop = true;
            listViewSkills.ItemDrag += ListViewSkills_ItemDrag;
            listViewSkills.DragEnter += ListViewSkills_DragEnter;
            listViewSkills.DragDrop += ListViewSkills_DragDrop;
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

        // === Start Skill ===
        private void StartSkill(string skill)
        {
            // Commit previous skill time before switching
            if (!string.IsNullOrEmpty(currentSkill) && stopwatch.IsRunning)
            {
                skillTimes[currentSkill] += stopwatch.Elapsed;
                stopwatch.Reset();
            }

            currentSkill = skill;
            stopwatch.Start();
            uiTimer.Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (listViewSkills.SelectedItems.Count == 0) return;
            string selectedSkill = listViewSkills.SelectedItems[0].Text;
            StartSkill(selectedSkill);
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

        // === Save ===
        private void btnSave_Click(object sender, EventArgs e)
        {
            using StreamWriter sw = new(saveFile);
            foreach (var kv in skillTimes)
            {
                sw.WriteLine($"{kv.Key}|{kv.Value.Ticks}");
            }

            MessageBox.Show("Skills saved successfully!", "Save",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // === Load saved skills ===
        private void LoadSkills()
        {
            if (!File.Exists(saveFile)) return;

            foreach (var line in File.ReadAllLines(saveFile))
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && long.TryParse(parts[1], out long ticks))
                {
                    skillTimes[parts[0]] = new TimeSpan(ticks);
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

        // === Drag/Drop Reordering ===
        private void ListViewSkills_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listViewSkills.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ListViewSkills_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
                e.Effect = DragDropEffects.Move;
        }

        private void ListViewSkills_DragDrop(object sender, DragEventArgs e)
        {
            Point cp = listViewSkills.PointToClient(new Point(e.X, e.Y));
            ListViewItem target = listViewSkills.GetItemAt(cp.X, cp.Y);
            ListViewItem dragged = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            if (target == null || dragged == null || target == dragged) return;

            int targetIndex = target.Index;
            listViewSkills.Items.Remove(dragged);
            listViewSkills.Items.Insert(targetIndex, dragged);

            // Re-sync dictionary with new order
            ReorderDictionary();
        }

        private void ReorderDictionary()
        {
            var newDict = new Dictionary<string, TimeSpan>();
            foreach (ListViewItem item in listViewSkills.Items)
            {
                if (skillTimes.TryGetValue(item.Text, out var t))
                    newDict[item.Text] = t;
            }
            skillTimes = newDict;
        }
    }
}
