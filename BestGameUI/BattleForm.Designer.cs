namespace BestGameUI
{
    partial class BattleForm
    {
        private System.ComponentModel.IContainer components = null;

        // Главная таблица
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;

        // Внутренняя таблица для размещения "верхнего Label" + "панелей армий"
        private System.Windows.Forms.TableLayoutPanel tableLayoutArmies;

        // Эти панели-обёртки помогут нам выровнять армии по вертикали
        private System.Windows.Forms.Panel panelArmy1;
        private System.Windows.Forms.Panel panelArmy2;

        // Основные FlowLayoutPanel, в которых будут выводиться юниты
        private BestGameUI.DoubleBufferedFlowLayoutPanel flowArmy1;
        private BestGameUI.DoubleBufferedFlowLayoutPanel flowArmy2;

        // Label c «Нажмите Enter…»
        private System.Windows.Forms.Label labelPressEnter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tableLayoutMain = new TableLayoutPanel();
            tableLayoutArmies = new TableLayoutPanel();
            panelCommands = new FlowLayoutPanel();
            btnNextTurn = new Button();
            btnSave = new Button();
            btnLoad = new Button();
            labelPressEnter = new Label();
            tableInner = new TableLayoutPanel();
            panelArmy1 = new Panel();
            flowArmy1 = new DoubleBufferedFlowLayoutPanel();
            panelArmy2 = new Panel();
            flowArmy2 = new DoubleBufferedFlowLayoutPanel();
            tableLayoutMain.SuspendLayout();
            tableLayoutArmies.SuspendLayout();
            panelCommands.SuspendLayout();
            tableInner.SuspendLayout();
            panelArmy1.SuspendLayout();
            panelArmy2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutMain
            // 
            tableLayoutMain.ColumnCount = 2;
            tableLayoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutMain.Controls.Add(tableLayoutArmies, 0, 0);
            tableLayoutMain.Dock = DockStyle.Fill;
            tableLayoutMain.Location = new Point(0, 0);
            tableLayoutMain.Margin = new Padding(4, 5, 4, 5);
            tableLayoutMain.Name = "tableLayoutMain";
            tableLayoutMain.RowCount = 1;
            tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutMain.Size = new Size(1714, 1050);
            tableLayoutMain.TabIndex = 0;
            // 
            // tableLayoutArmies
            // 
            tableLayoutArmies.ColumnCount = 1;
            tableLayoutArmies.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutArmies.Controls.Add(panelCommands, 0, 0);
            tableLayoutArmies.Controls.Add(labelPressEnter, 0, 0);
            tableLayoutArmies.Controls.Add(tableInner, 0, 1);
            tableLayoutArmies.Dock = DockStyle.Fill;
            tableLayoutArmies.Location = new Point(4, 5);
            tableLayoutArmies.Margin = new Padding(4, 5, 4, 5);
            tableLayoutArmies.Name = "tableLayoutArmies";
            tableLayoutArmies.RowCount = 3;
            tableLayoutArmies.RowStyles.Add(new RowStyle(SizeType.Absolute, 67F));
            tableLayoutArmies.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutArmies.RowStyles.Add(new RowStyle(SizeType.Absolute, 916F));
            tableLayoutArmies.Size = new Size(1191, 1040);
            tableLayoutArmies.TabIndex = 0;
            // 
            // panelCommands
            // 
            panelCommands.AutoSize = true;
            panelCommands.Controls.Add(btnNextTurn);
            panelCommands.Controls.Add(btnSave);
            panelCommands.Controls.Add(btnLoad);
            panelCommands.Dock = DockStyle.Top;
            panelCommands.Location = new Point(4, 72);
            panelCommands.Margin = new Padding(4, 5, 4, 5);
            panelCommands.Name = "panelCommands";
            panelCommands.Size = new Size(1183, 47);
            panelCommands.TabIndex = 0;
            panelCommands.WrapContents = false;
            // 
            // btnNextTurn
            // 
            btnNextTurn.Location = new Point(4, 5);
            btnNextTurn.Margin = new Padding(4, 5, 4, 5);
            btnNextTurn.Name = "btnNextTurn";
            btnNextTurn.Size = new Size(107, 38);
            btnNextTurn.TabIndex = 0;
            btnNextTurn.Text = "Сделать ход";
            btnNextTurn.Click += btnNextTurn_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(119, 5);
            btnSave.Margin = new Padding(4, 5, 4, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(107, 38);
            btnSave.TabIndex = 1;
            btnSave.Text = "Сохранить";
            btnSave.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(234, 5);
            btnLoad.Margin = new Padding(4, 5, 4, 5);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(107, 38);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Загрузить";
            btnLoad.Click += btnLoad_Click;
            // 
            // labelPressEnter
            // 
            labelPressEnter.AutoSize = true;
            labelPressEnter.Dock = DockStyle.Fill;
            labelPressEnter.Font = new Font("Segoe UI", 12F);
            labelPressEnter.Location = new Point(4, 0);
            labelPressEnter.Margin = new Padding(4, 0, 4, 0);
            labelPressEnter.Name = "labelPressEnter";
            labelPressEnter.Size = new Size(1183, 67);
            labelPressEnter.TabIndex = 1;
            labelPressEnter.Text = "Нажмите Enter, чтобы сделать следующий ход";
            labelPressEnter.TextAlign = ContentAlignment.MiddleLeft;
            labelPressEnter.Click += labelPressEnter_Click;
            // 
            // tableInner
            // 
            tableInner.ColumnCount = 2;
            tableInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableInner.Controls.Add(panelArmy1, 0, 0);
            tableInner.Controls.Add(panelArmy2, 1, 0);
            tableInner.Dock = DockStyle.Fill;
            tableInner.Location = new Point(4, 129);
            tableInner.Margin = new Padding(4, 5, 4, 5);
            tableInner.Name = "tableInner";
            tableInner.RowCount = 1;
            tableInner.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableInner.Size = new Size(1183, 906);
            tableInner.TabIndex = 2;
            // 
            // panelArmy1
            // 
            panelArmy1.Controls.Add(flowArmy1);
            panelArmy1.Dock = DockStyle.Fill;
            panelArmy1.Location = new Point(4, 5);
            panelArmy1.Margin = new Padding(4, 5, 4, 5);
            panelArmy1.Name = "panelArmy1";
            panelArmy1.Size = new Size(583, 896);
            panelArmy1.TabIndex = 0;
            panelArmy1.Resize += PanelArmy_Resize;
            // 
            // flowArmy1
            // 
            flowArmy1.Anchor = AnchorStyles.None;
            flowArmy1.AutoSize = true;
            flowArmy1.FlowDirection = FlowDirection.RightToLeft;
            flowArmy1.Location = new Point(147, 365);
            flowArmy1.Margin = new Padding(4, 5, 4, 5);
            flowArmy1.Name = "flowArmy1";
            flowArmy1.Size = new Size(286, 167);
            flowArmy1.TabIndex = 0;
            // 
            // panelArmy2
            // 
            panelArmy2.Controls.Add(flowArmy2);
            panelArmy2.Dock = DockStyle.Fill;
            panelArmy2.Location = new Point(595, 5);
            panelArmy2.Margin = new Padding(4, 5, 4, 5);
            panelArmy2.Name = "panelArmy2";
            panelArmy2.Size = new Size(584, 896);
            panelArmy2.TabIndex = 1;
            panelArmy2.Resize += PanelArmy_Resize;
            // 
            // flowArmy2
            // 
            flowArmy2.Anchor = AnchorStyles.None;
            flowArmy2.AutoSize = true;
            flowArmy2.Location = new Point(150, 365);
            flowArmy2.Margin = new Padding(4, 5, 4, 5);
            flowArmy2.Name = "flowArmy2";
            flowArmy2.Size = new Size(286, 167);
            flowArmy2.TabIndex = 0;
            // 
            // BattleForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1714, 1050);
            Controls.Add(tableLayoutMain);
            KeyPreview = true;
            Margin = new Padding(4, 5, 4, 5);
            Name = "BattleForm";
            Text = "Бой";
            WindowState = FormWindowState.Maximized;
            KeyDown += BattleForm_KeyDown;
            Resize += BattleForm_Resize;
            tableLayoutMain.ResumeLayout(false);
            tableLayoutArmies.ResumeLayout(false);
            tableLayoutArmies.PerformLayout();
            panelCommands.ResumeLayout(false);
            tableInner.ResumeLayout(false);
            panelArmy1.ResumeLayout(false);
            panelArmy1.PerformLayout();
            panelArmy2.ResumeLayout(false);
            panelArmy2.PerformLayout();
            ResumeLayout(false);
        }

        private FlowLayoutPanel panelCommands;
        private Button btnNextTurn;
        private Button btnSave;
        private Button btnLoad;
        private TableLayoutPanel tableInner;
    }
}
