namespace WurmAssistant.Granger
{
    partial class FormHorseManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxHerd = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRenameHerd = new System.Windows.Forms.Button();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.buttonDeleteHerd = new System.Windows.Forms.Button();
            this.buttonMergeHerds = new System.Windows.Forms.Button();
            this.buttonNewHerd = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.contextMenuStripHerd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHorseManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.editTraitsManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveThisHorseToAnotherHerdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteThisHorseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSetWeights = new System.Windows.Forms.Button();
            this.buttonChangeNewHorseDestination = new System.Windows.Forms.Button();
            this.textBoxNewHorseDestination = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonChangeAHSkill = new System.Windows.Forms.Button();
            this.numericUpDownAHSkill = new System.Windows.Forms.NumericUpDown();
            this.labelAHSkill = new System.Windows.Forms.Label();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.checkBoxDontExcludeInbreed = new System.Windows.Forms.CheckBox();
            this.checkBoxNegativeExclude = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludePotential = new System.Windows.Forms.CheckBox();
            this.checkBoxPairingMissingTraits = new System.Windows.Forms.CheckBox();
            this.checkBoxAdviseAvailable = new System.Windows.Forms.CheckBox();
            this.checkBoxLiveAdvisor = new System.Windows.Forms.CheckBox();
            this.labelInspectedHorse = new System.Windows.Forms.Label();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.listViewNFHerd = new ClassLibrary1.ListViewNF();
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewNFTraits = new ClassLibrary1.ListViewNF();
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonExport = new System.Windows.Forms.Button();
            this.checkBoxDisableAdvisor = new System.Windows.Forms.CheckBox();
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.checkBoxEpicCurve = new System.Windows.Forms.CheckBox();
            this.contextMenuStripHerd.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAHSkill)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxHerd
            // 
            this.comboBoxHerd.DropDownHeight = 150;
            this.comboBoxHerd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHerd.FormattingEnabled = true;
            this.comboBoxHerd.IntegralHeight = false;
            this.comboBoxHerd.Location = new System.Drawing.Point(12, 129);
            this.comboBoxHerd.MaxDropDownItems = 12;
            this.comboBoxHerd.Name = "comboBoxHerd";
            this.comboBoxHerd.Size = new System.Drawing.Size(196, 24);
            this.comboBoxHerd.TabIndex = 0;
            this.comboBoxHerd.SelectedIndexChanged += new System.EventHandler(this.comboBoxHerd_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current herd:";
            // 
            // buttonRenameHerd
            // 
            this.buttonRenameHerd.Location = new System.Drawing.Point(288, 128);
            this.buttonRenameHerd.Name = "buttonRenameHerd";
            this.buttonRenameHerd.Size = new System.Drawing.Size(77, 25);
            this.buttonRenameHerd.TabIndex = 2;
            this.buttonRenameHerd.Text = "Rename";
            this.buttonRenameHerd.UseVisualStyleBackColor = true;
            this.buttonRenameHerd.Click += new System.EventHandler(this.buttonRenameHerd_Click);
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.Location = new System.Drawing.Point(6, 24);
            this.checkBoxEnable.Name = "checkBoxEnable";
            this.checkBoxEnable.Size = new System.Drawing.Size(159, 38);
            this.checkBoxEnable.TabIndex = 4;
            this.checkBoxEnable.Text = "Enable capturing\r\ndata from game logs";
            this.checkBoxEnable.UseVisualStyleBackColor = true;
            this.checkBoxEnable.CheckedChanged += new System.EventHandler(this.checkBoxEnable_CheckedChanged);
            // 
            // buttonDeleteHerd
            // 
            this.buttonDeleteHerd.Location = new System.Drawing.Point(445, 128);
            this.buttonDeleteHerd.Name = "buttonDeleteHerd";
            this.buttonDeleteHerd.Size = new System.Drawing.Size(68, 25);
            this.buttonDeleteHerd.TabIndex = 6;
            this.buttonDeleteHerd.Text = "Delete";
            this.buttonDeleteHerd.UseVisualStyleBackColor = true;
            this.buttonDeleteHerd.Click += new System.EventHandler(this.buttonDeleteHerd_Click);
            // 
            // buttonMergeHerds
            // 
            this.buttonMergeHerds.Location = new System.Drawing.Point(371, 128);
            this.buttonMergeHerds.Name = "buttonMergeHerds";
            this.buttonMergeHerds.Size = new System.Drawing.Size(68, 25);
            this.buttonMergeHerds.TabIndex = 7;
            this.buttonMergeHerds.Text = "Merge...";
            this.buttonMergeHerds.UseVisualStyleBackColor = true;
            this.buttonMergeHerds.Click += new System.EventHandler(this.buttonMergeHerds_Click);
            // 
            // buttonNewHerd
            // 
            this.buttonNewHerd.Location = new System.Drawing.Point(214, 128);
            this.buttonNewHerd.Name = "buttonNewHerd";
            this.buttonNewHerd.Size = new System.Drawing.Size(68, 25);
            this.buttonNewHerd.TabIndex = 8;
            this.buttonNewHerd.Text = "New...";
            this.buttonNewHerd.UseVisualStyleBackColor = true;
            this.buttonNewHerd.Click += new System.EventHandler(this.buttonNewHerd_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(1344, 13);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(99, 52);
            this.buttonHelp.TabIndex = 9;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // contextMenuStripHerd
            // 
            this.contextMenuStripHerd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHorseManuallyToolStripMenuItem,
            this.toolStripSeparator2,
            this.editTraitsManuallyToolStripMenuItem,
            this.moveThisHorseToAnotherHerdToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteThisHorseToolStripMenuItem});
            this.contextMenuStripHerd.Name = "contextMenuStripHerd";
            this.contextMenuStripHerd.Size = new System.Drawing.Size(290, 112);
            // 
            // addHorseManuallyToolStripMenuItem
            // 
            this.addHorseManuallyToolStripMenuItem.Name = "addHorseManuallyToolStripMenuItem";
            this.addHorseManuallyToolStripMenuItem.Size = new System.Drawing.Size(289, 24);
            this.addHorseManuallyToolStripMenuItem.Text = "Add new horse manually";
            this.addHorseManuallyToolStripMenuItem.Click += new System.EventHandler(this.addHorseManuallyToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(286, 6);
            // 
            // editTraitsManuallyToolStripMenuItem
            // 
            this.editTraitsManuallyToolStripMenuItem.Name = "editTraitsManuallyToolStripMenuItem";
            this.editTraitsManuallyToolStripMenuItem.Size = new System.Drawing.Size(289, 24);
            this.editTraitsManuallyToolStripMenuItem.Text = "Edit this horse";
            this.editTraitsManuallyToolStripMenuItem.Click += new System.EventHandler(this.editTraitsManuallyToolStripMenuItem_Click);
            // 
            // moveThisHorseToAnotherHerdToolStripMenuItem
            // 
            this.moveThisHorseToAnotherHerdToolStripMenuItem.Name = "moveThisHorseToAnotherHerdToolStripMenuItem";
            this.moveThisHorseToAnotherHerdToolStripMenuItem.Size = new System.Drawing.Size(289, 24);
            this.moveThisHorseToAnotherHerdToolStripMenuItem.Text = "Move this horse to another herd";
            this.moveThisHorseToAnotherHerdToolStripMenuItem.Click += new System.EventHandler(this.moveThisHorseToAnotherHerdToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(286, 6);
            // 
            // deleteThisHorseToolStripMenuItem
            // 
            this.deleteThisHorseToolStripMenuItem.Name = "deleteThisHorseToolStripMenuItem";
            this.deleteThisHorseToolStripMenuItem.Size = new System.Drawing.Size(289, 24);
            this.deleteThisHorseToolStripMenuItem.Text = "Delete this horse";
            this.deleteThisHorseToolStripMenuItem.Click += new System.EventHandler(this.deleteThisHorseToolStripMenuItem_Click);
            // 
            // buttonSetWeights
            // 
            this.buttonSetWeights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetWeights.Location = new System.Drawing.Point(1247, 127);
            this.buttonSetWeights.Name = "buttonSetWeights";
            this.buttonSetWeights.Size = new System.Drawing.Size(213, 25);
            this.buttonSetWeights.TabIndex = 13;
            this.buttonSetWeights.Text = "Set weights (value) for traits";
            this.buttonSetWeights.UseVisualStyleBackColor = true;
            this.buttonSetWeights.Click += new System.EventHandler(this.buttonSetWeights_Click);
            // 
            // buttonChangeNewHorseDestination
            // 
            this.buttonChangeNewHorseDestination.Location = new System.Drawing.Point(389, 42);
            this.buttonChangeNewHorseDestination.Name = "buttonChangeNewHorseDestination";
            this.buttonChangeNewHorseDestination.Size = new System.Drawing.Size(91, 25);
            this.buttonChangeNewHorseDestination.TabIndex = 16;
            this.buttonChangeNewHorseDestination.Text = "Change...";
            this.buttonChangeNewHorseDestination.UseVisualStyleBackColor = true;
            this.buttonChangeNewHorseDestination.Click += new System.EventHandler(this.buttonChangeNewHorseDestination_Click);
            // 
            // textBoxNewHorseDestination
            // 
            this.textBoxNewHorseDestination.Location = new System.Drawing.Point(181, 43);
            this.textBoxNewHorseDestination.Name = "textBoxNewHorseDestination";
            this.textBoxNewHorseDestination.ReadOnly = true;
            this.textBoxNewHorseDestination.Size = new System.Drawing.Size(202, 22);
            this.textBoxNewHorseDestination.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "New horses will be added to this herd:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxEpicCurve);
            this.groupBox1.Controls.Add(this.buttonChangeAHSkill);
            this.groupBox1.Controls.Add(this.numericUpDownAHSkill);
            this.groupBox1.Controls.Add(this.labelAHSkill);
            this.groupBox1.Controls.Add(this.buttonDebug);
            this.groupBox1.Controls.Add(this.checkBoxDontExcludeInbreed);
            this.groupBox1.Controls.Add(this.checkBoxNegativeExclude);
            this.groupBox1.Controls.Add(this.checkBoxIncludePotential);
            this.groupBox1.Controls.Add(this.checkBoxPairingMissingTraits);
            this.groupBox1.Controls.Add(this.checkBoxAdviseAvailable);
            this.groupBox1.Controls.Add(this.checkBoxLiveAdvisor);
            this.groupBox1.Controls.Add(this.checkBoxEnable);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonChangeNewHorseDestination);
            this.groupBox1.Controls.Add(this.textBoxNewHorseDestination);
            this.groupBox1.Controls.Add(this.buttonHelp);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1449, 98);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Granger options";
            // 
            // buttonChangeAHSkill
            // 
            this.buttonChangeAHSkill.Enabled = false;
            this.buttonChangeAHSkill.Location = new System.Drawing.Point(1038, 35);
            this.buttonChangeAHSkill.Name = "buttonChangeAHSkill";
            this.buttonChangeAHSkill.Size = new System.Drawing.Size(78, 25);
            this.buttonChangeAHSkill.TabIndex = 32;
            this.buttonChangeAHSkill.Text = "Change";
            this.buttonChangeAHSkill.UseVisualStyleBackColor = true;
            this.buttonChangeAHSkill.Click += new System.EventHandler(this.buttonChangeAHSkill_Click);
            // 
            // numericUpDownAHSkill
            // 
            this.numericUpDownAHSkill.Location = new System.Drawing.Point(956, 37);
            this.numericUpDownAHSkill.Name = "numericUpDownAHSkill";
            this.numericUpDownAHSkill.ReadOnly = true;
            this.numericUpDownAHSkill.Size = new System.Drawing.Size(76, 22);
            this.numericUpDownAHSkill.TabIndex = 33;
            this.numericUpDownAHSkill.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDownAHSkill_KeyDown);
            this.numericUpDownAHSkill.Validated += new System.EventHandler(this.numericUpDownAHSkill_Validated);
            // 
            // labelAHSkill
            // 
            this.labelAHSkill.AutoSize = true;
            this.labelAHSkill.Location = new System.Drawing.Point(953, 16);
            this.labelAHSkill.Name = "labelAHSkill";
            this.labelAHSkill.Size = new System.Drawing.Size(79, 17);
            this.labelAHSkill.TabIndex = 30;
            this.labelAHSkill.Text = "AH skill for:";
            // 
            // buttonDebug
            // 
            this.buttonDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDebug.Location = new System.Drawing.Point(1344, 66);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(99, 26);
            this.buttonDebug.TabIndex = 29;
            this.buttonDebug.Text = "Debug";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
            // 
            // checkBoxDontExcludeInbreed
            // 
            this.checkBoxDontExcludeInbreed.AutoSize = true;
            this.checkBoxDontExcludeInbreed.Location = new System.Drawing.Point(696, 68);
            this.checkBoxDontExcludeInbreed.Name = "checkBoxDontExcludeInbreed";
            this.checkBoxDontExcludeInbreed.Size = new System.Drawing.Size(195, 21);
            this.checkBoxDontExcludeInbreed.TabIndex = 25;
            this.checkBoxDontExcludeInbreed.Text = "Do not exclude inbreeding";
            this.checkBoxDontExcludeInbreed.UseVisualStyleBackColor = true;
            this.checkBoxDontExcludeInbreed.CheckedChanged += new System.EventHandler(this.checkBoxDontExcludeInbreed_CheckedChanged);
            // 
            // checkBoxNegativeExclude
            // 
            this.checkBoxNegativeExclude.AutoSize = true;
            this.checkBoxNegativeExclude.Location = new System.Drawing.Point(696, 41);
            this.checkBoxNegativeExclude.Name = "checkBoxNegativeExclude";
            this.checkBoxNegativeExclude.Size = new System.Drawing.Size(210, 21);
            this.checkBoxNegativeExclude.TabIndex = 24;
            this.checkBoxNegativeExclude.Text = "Exclude if any negative traits";
            this.checkBoxNegativeExclude.UseVisualStyleBackColor = true;
            this.checkBoxNegativeExclude.CheckedChanged += new System.EventHandler(this.checkBoxNegativeExclude_CheckedChanged);
            // 
            // checkBoxIncludePotential
            // 
            this.checkBoxIncludePotential.AutoSize = true;
            this.checkBoxIncludePotential.Location = new System.Drawing.Point(514, 68);
            this.checkBoxIncludePotential.Name = "checkBoxIncludePotential";
            this.checkBoxIncludePotential.Size = new System.Drawing.Size(171, 21);
            this.checkBoxIncludePotential.TabIndex = 23;
            this.checkBoxIncludePotential.Text = "Include potential value";
            this.checkBoxIncludePotential.UseVisualStyleBackColor = true;
            this.checkBoxIncludePotential.CheckedChanged += new System.EventHandler(this.checkBoxIncludePotential_CheckedChanged);
            // 
            // checkBoxPairingMissingTraits
            // 
            this.checkBoxPairingMissingTraits.AutoSize = true;
            this.checkBoxPairingMissingTraits.Location = new System.Drawing.Point(696, 15);
            this.checkBoxPairingMissingTraits.Name = "checkBoxPairingMissingTraits";
            this.checkBoxPairingMissingTraits.Size = new System.Drawing.Size(223, 21);
            this.checkBoxPairingMissingTraits.TabIndex = 22;
            this.checkBoxPairingMissingTraits.Text = "Prefer pairing for missing traits";
            this.checkBoxPairingMissingTraits.UseVisualStyleBackColor = true;
            this.checkBoxPairingMissingTraits.CheckedChanged += new System.EventHandler(this.checkBoxPairingMissingTraits_CheckedChanged);
            // 
            // checkBoxAdviseAvailable
            // 
            this.checkBoxAdviseAvailable.AutoSize = true;
            this.checkBoxAdviseAvailable.Location = new System.Drawing.Point(514, 41);
            this.checkBoxAdviseAvailable.Name = "checkBoxAdviseAvailable";
            this.checkBoxAdviseAvailable.Size = new System.Drawing.Size(162, 21);
            this.checkBoxAdviseAvailable.TabIndex = 21;
            this.checkBoxAdviseAvailable.Text = "Advise only available";
            this.checkBoxAdviseAvailable.UseVisualStyleBackColor = true;
            this.checkBoxAdviseAvailable.CheckedChanged += new System.EventHandler(this.checkBoxAdviseAvailable_CheckedChanged);
            // 
            // checkBoxLiveAdvisor
            // 
            this.checkBoxLiveAdvisor.AutoSize = true;
            this.checkBoxLiveAdvisor.Enabled = false;
            this.checkBoxLiveAdvisor.Location = new System.Drawing.Point(514, 15);
            this.checkBoxLiveAdvisor.Name = "checkBoxLiveAdvisor";
            this.checkBoxLiveAdvisor.Size = new System.Drawing.Size(106, 21);
            this.checkBoxLiveAdvisor.TabIndex = 19;
            this.checkBoxLiveAdvisor.Text = "Live advisor";
            this.checkBoxLiveAdvisor.UseVisualStyleBackColor = true;
            this.checkBoxLiveAdvisor.CheckedChanged += new System.EventHandler(this.checkBoxLiveAdvisor_CheckedChanged);
            // 
            // labelInspectedHorse
            // 
            this.labelInspectedHorse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInspectedHorse.AutoSize = true;
            this.labelInspectedHorse.Location = new System.Drawing.Point(1135, 135);
            this.labelInspectedHorse.Name = "labelInspectedHorse";
            this.labelInspectedHorse.Size = new System.Drawing.Size(0, 17);
            this.labelInspectedHorse.TabIndex = 23;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(953, 101);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(122, 25);
            this.buttonRefresh.TabIndex = 24;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Visible = false;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 1000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // listViewNFHerd
            // 
            this.listViewNFHerd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewNFHerd.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23});
            this.listViewNFHerd.ContextMenuStrip = this.contextMenuStripHerd;
            this.listViewNFHerd.FullRowSelect = true;
            this.listViewNFHerd.GridLines = true;
            this.listViewNFHerd.HideSelection = false;
            this.listViewNFHerd.Location = new System.Drawing.Point(12, 156);
            this.listViewNFHerd.MultiSelect = false;
            this.listViewNFHerd.Name = "listViewNFHerd";
            this.listViewNFHerd.Size = new System.Drawing.Size(1063, 532);
            this.listViewNFHerd.TabIndex = 25;
            this.listViewNFHerd.UseCompatibleStateImageBehavior = false;
            this.listViewNFHerd.View = System.Windows.Forms.View.Details;
            this.listViewNFHerd.VirtualMode = true;
            this.listViewNFHerd.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewNFHerd_ColumnClick);
            this.listViewNFHerd.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewNFHerd_RetrieveVirtualItem);
            this.listViewNFHerd.SelectedIndexChanged += new System.EventHandler(this.listViewNFHerd_SelectedIndexChanged);
            this.listViewNFHerd.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewNFHerd_MouseDoubleClick);
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Name";
            this.columnHeader14.Width = 118;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Gender";
            this.columnHeader15.Width = 64;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Father";
            this.columnHeader16.Width = 87;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Mother";
            this.columnHeader17.Width = 95;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Value";
            this.columnHeader18.Width = 53;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Potential";
            this.columnHeader19.Width = 68;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Breeded";
            this.columnHeader20.Width = 94;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Groomed";
            this.columnHeader21.Width = 96;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Deliver";
            this.columnHeader22.Width = 97;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Comments";
            this.columnHeader23.Width = 257;
            // 
            // listViewNFTraits
            // 
            this.listViewNFTraits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewNFTraits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26});
            this.listViewNFTraits.GridLines = true;
            this.listViewNFTraits.Location = new System.Drawing.Point(1081, 156);
            this.listViewNFTraits.Name = "listViewNFTraits";
            this.listViewNFTraits.Size = new System.Drawing.Size(379, 532);
            this.listViewNFTraits.TabIndex = 26;
            this.listViewNFTraits.UseCompatibleStateImageBehavior = false;
            this.listViewNFTraits.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "Trait";
            this.columnHeader24.Width = 190;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Status";
            this.columnHeader25.Width = 70;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "Value";
            // 
            // buttonExport
            // 
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new System.Drawing.Point(953, 127);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(122, 25);
            this.buttonExport.TabIndex = 27;
            this.buttonExport.Text = "Export...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Visible = false;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // checkBoxDisableAdvisor
            // 
            this.checkBoxDisableAdvisor.AutoSize = true;
            this.checkBoxDisableAdvisor.Location = new System.Drawing.Point(536, 131);
            this.checkBoxDisableAdvisor.Name = "checkBoxDisableAdvisor";
            this.checkBoxDisableAdvisor.Size = new System.Drawing.Size(127, 21);
            this.checkBoxDisableAdvisor.TabIndex = 28;
            this.checkBoxDisableAdvisor.Text = "Disable advisor";
            this.checkBoxDisableAdvisor.UseVisualStyleBackColor = true;
            this.checkBoxDisableAdvisor.CheckedChanged += new System.EventHandler(this.checkBoxDisableAdvisor_CheckedChanged);
            // 
            // buttonDeleteAll
            // 
            this.buttonDeleteAll.Location = new System.Drawing.Point(839, 128);
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Size = new System.Drawing.Size(108, 24);
            this.buttonDeleteAll.TabIndex = 34;
            this.buttonDeleteAll.Text = "Delete ALL";
            this.buttonDeleteAll.UseVisualStyleBackColor = true;
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // checkBoxEpicCurve
            // 
            this.checkBoxEpicCurve.AutoSize = true;
            this.checkBoxEpicCurve.Location = new System.Drawing.Point(956, 65);
            this.checkBoxEpicCurve.Name = "checkBoxEpicCurve";
            this.checkBoxEpicCurve.Size = new System.Drawing.Size(180, 21);
            this.checkBoxEpicCurve.TabIndex = 34;
            this.checkBoxEpicCurve.Text = "Epic skill-curve override";
            this.checkBoxEpicCurve.UseVisualStyleBackColor = true;
            this.checkBoxEpicCurve.CheckedChanged += new System.EventHandler(this.checkBoxEpicCurve_CheckedChanged);
            // 
            // FormHorseManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1473, 700);
            this.Controls.Add(this.buttonDeleteAll);
            this.Controls.Add(this.checkBoxDisableAdvisor);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.listViewNFTraits);
            this.Controls.Add(this.listViewNFHerd);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.labelInspectedHorse);
            this.Controls.Add(this.buttonSetWeights);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonNewHerd);
            this.Controls.Add(this.buttonMergeHerds);
            this.Controls.Add(this.buttonDeleteHerd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRenameHerd);
            this.Controls.Add(this.comboBoxHerd);
            this.Name = "FormHorseManager";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Granger (beta version)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHorseManager_FormClosing);
            this.Load += new System.EventHandler(this.FormHorseManager_Load);
            this.contextMenuStripHerd.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAHSkill)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxHerd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonRenameHerd;
        private System.Windows.Forms.CheckBox checkBoxEnable;
        private System.Windows.Forms.Button buttonDeleteHerd;
        private System.Windows.Forms.Button buttonMergeHerds;
        private System.Windows.Forms.Button buttonNewHerd;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripHerd;
        private System.Windows.Forms.Button buttonSetWeights;
        private System.Windows.Forms.ToolStripMenuItem addHorseManuallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editTraitsManuallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveThisHorseToAnotherHerdToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteThisHorseToolStripMenuItem;
        private System.Windows.Forms.Button buttonChangeNewHorseDestination;
        private System.Windows.Forms.TextBox textBoxNewHorseDestination;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelInspectedHorse;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Timer timerRefresh;
        private ClassLibrary1.ListViewNF listViewNFHerd;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private ClassLibrary1.ListViewNF listViewNFTraits;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader26;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.CheckBox checkBoxAdviseAvailable;
        private System.Windows.Forms.CheckBox checkBoxLiveAdvisor;
        private System.Windows.Forms.CheckBox checkBoxPairingMissingTraits;
        private System.Windows.Forms.CheckBox checkBoxIncludePotential;
        private System.Windows.Forms.CheckBox checkBoxNegativeExclude;
        private System.Windows.Forms.CheckBox checkBoxDontExcludeInbreed;
        private System.Windows.Forms.CheckBox checkBoxDisableAdvisor;
        private System.Windows.Forms.Button buttonDebug;
        private System.Windows.Forms.Button buttonChangeAHSkill;
        private System.Windows.Forms.NumericUpDown numericUpDownAHSkill;
        private System.Windows.Forms.Label labelAHSkill;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.CheckBox checkBoxEpicCurve;
    }
}