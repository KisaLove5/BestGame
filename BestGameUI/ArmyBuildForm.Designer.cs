namespace BestGameUI
{
    partial class ArmyBuildForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonSwordsman;
        private System.Windows.Forms.Button buttonSpearman;
        private System.Windows.Forms.Button buttonArcher;
        private System.Windows.Forms.Button buttonMage;
        private System.Windows.Forms.Button buttonHealer;
        private System.Windows.Forms.Button buttonWall;
        private System.Windows.Forms.Button buttonWeaponBearer; // ✅ добавил кнопку оруженосца
        private System.Windows.Forms.Button buttonReady;
        private System.Windows.Forms.Label labelMoney;

        private BestGameUI.DoubleBufferedFlowLayoutPanel flowArmy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonSwordsman = new System.Windows.Forms.Button();
            this.buttonSpearman = new System.Windows.Forms.Button();
            this.buttonArcher = new System.Windows.Forms.Button();
            this.buttonMage = new System.Windows.Forms.Button();
            this.buttonHealer = new System.Windows.Forms.Button();
            this.buttonWall = new System.Windows.Forms.Button();
            this.buttonWeaponBearer = new System.Windows.Forms.Button(); // ✅ добавил
            this.buttonReady = new System.Windows.Forms.Button();
            this.labelMoney = new System.Windows.Forms.Label();
            this.flowArmy = new BestGameUI.DoubleBufferedFlowLayoutPanel();

            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            // tableLayoutPanel1
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.CreateLeftPanel(), 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowArmy, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1200, 600);

            // flowArmy
            this.flowArmy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowArmy.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowArmy.AutoScroll = true;

            // ArmyBuildForm
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ArmyBuildForm";
            this.Text = "Сбор армии";
        }

        /// <summary>
        /// Создаём панель для кнопок и инфо.
        /// </summary>
        private System.Windows.Forms.Panel CreateLeftPanel()
        {
            var panelLeft = new System.Windows.Forms.Panel();
            panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;

            labelInfo.AutoSize = true;
            labelInfo.Font = new System.Drawing.Font("Segoe UI", 14F);
            labelInfo.Text = "Сбор армии";
            labelInfo.Location = new System.Drawing.Point(10, 10);

            labelMoney.AutoSize = true;
            labelMoney.Font = new System.Drawing.Font("Segoe UI", 14F);
            labelMoney.Text = "Монет: 0";
            labelMoney.Location = new System.Drawing.Point(10, 60);

            int top = 120;
            int step = 50;

            buttonSwordsman.Text = "Swordsman (50)";
            buttonSwordsman.Size = new System.Drawing.Size(150, 40);
            buttonSwordsman.Location = new System.Drawing.Point(10, top);

            buttonSpearman.Text = "Spearman (60)";
            buttonSpearman.Size = new System.Drawing.Size(150, 40);
            buttonSpearman.Location = new System.Drawing.Point(10, top += step);

            buttonArcher.Text = "Archer (40)";
            buttonArcher.Size = new System.Drawing.Size(150, 40);
            buttonArcher.Location = new System.Drawing.Point(10, top += step);

            buttonMage.Text = "Mage (100)";
            buttonMage.Size = new System.Drawing.Size(150, 40);
            buttonMage.Location = new System.Drawing.Point(10, top += step);

            buttonHealer.Text = "Healer (80)";
            buttonHealer.Size = new System.Drawing.Size(150, 40);
            buttonHealer.Location = new System.Drawing.Point(10, top += step);

            buttonWall.Text = "Wall (20)";
            buttonWall.Size = new System.Drawing.Size(150, 40);
            buttonWall.Location = new System.Drawing.Point(10, top += step);

            buttonWeaponBearer.Text = "WeaponBearer (70)";
            buttonWeaponBearer.Size = new System.Drawing.Size(150, 40);
            buttonWeaponBearer.Location = new System.Drawing.Point(10, top += step); // ✅ добавил

            buttonReady.Text = "Готово";
            buttonReady.Size = new System.Drawing.Size(150, 40);
            buttonReady.Location = new System.Drawing.Point(10, top += step);

            panelLeft.Controls.Add(labelInfo);
            panelLeft.Controls.Add(labelMoney);
            panelLeft.Controls.Add(buttonSwordsman);
            panelLeft.Controls.Add(buttonSpearman);
            panelLeft.Controls.Add(buttonArcher);
            panelLeft.Controls.Add(buttonMage);
            panelLeft.Controls.Add(buttonHealer);
            panelLeft.Controls.Add(buttonWall);
            panelLeft.Controls.Add(buttonWeaponBearer); // ✅ добавил
            panelLeft.Controls.Add(buttonReady);

            return panelLeft;
        }
    }
}
