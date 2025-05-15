using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StrategyGame;

namespace BestGameUI
{
    public partial class ArmyBuildForm : Form
    {
        private string playerName;
        private int money;
        private Army army;
        private UnitFactory factory;

        public Army ResultArmy => army;

        public ArmyBuildForm(string playerName, int startMoney)
        {
            InitializeComponent(); // создаёт tableLayoutPanel, leftPanel, flowArmy
            this.playerName = playerName;
            this.money = startMoney;
            this.army = new Army();
            this.factory = new UnitFactory();

            labelInfo.Text = $"Сбор армии для {playerName}";
            labelMoney.Text = $"Монет: {money}";

            // Привязываем обработчики кнопок (можно было это сделать и в Designer)
            buttonSwordsman.Click += (s, e) => PurchaseUnit("swordsman", 50);
            buttonSpearman.Click += (s, e) => PurchaseUnit("spearman", 60);
            buttonArcher.Click += (s, e) => PurchaseUnit("archer", 40);
            buttonMage.Click += (s, e) => PurchaseUnit("mage", 100);
            buttonHealer.Click += (s, e) => PurchaseUnit("healer", 80);
            buttonWall.Click += (s, e) => PurchaseUnit("wall", 20);
            buttonWeaponBearer.Click += (s, e) => PurchaseUnit("weaponbearer", 70); // ✅ добавляем оруженосца
            buttonReady.Click += buttonReady_Click;
        }

        private void PurchaseUnit(string unitName, int cost)
        {
            if (money >= cost)
            {
                var newUnit = factory.CreateUnit(unitName);
                if (newUnit != null)
                {
                    army.AddUnit(newUnit);
                    money -= cost;
                    labelMoney.Text = $"Монет: {money}";

                    RefreshArmyUI();
                }
            }
            else
            {
                MessageBox.Show("Недостаточно монет!");
            }
        }

        private void buttonReady_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshArmyUI()
        {
            flowArmy.SuspendLayout();
            flowArmy.Visible = false;

            flowArmy.Controls.Clear();

            var units = army.GetAllUnits();
            foreach (var unit in units)
            {
                var panel = CreateUnitPanel(unit);
                flowArmy.Controls.Add(panel);
            }

            CenterFlowArmy(); // Центруем юниты
            flowArmy.Visible = true;
            flowArmy.ResumeLayout();
        }

        private void CenterFlowArmy()
        {
            flowArmy.Update(); // Обновляем размеры
            int totalWidth = 0;
            foreach (Control control in flowArmy.Controls)
            {
                totalWidth += control.Width + control.Margin.Horizontal;
            }

            if (flowArmy.Controls.Count > 0)
            {
                flowArmy.Padding = new Padding(Math.Max(0, (flowArmy.Width - totalWidth) / 2), 10, 0, 0);
            }
        }

        private Panel CreateUnitPanel(Unit unit)
        {
            Panel panel = new Panel
            {
                Width = 200,
                Height = 200,
                BackColor = Color.LightGray,
                Margin = new Padding(5)
            };

            string unitName = unit.DisplayName;
            string path = System.IO.Path.Combine(Application.StartupPath, "Graphics/Units", unitName.ToLower() + ".png");

            if (System.IO.File.Exists(path))
            {
                panel.BackgroundImage = Image.FromFile(path);
                panel.BackgroundImageLayout = ImageLayout.Stretch;
            }

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false,
                Height = 50
            };

            Size buttonSize = new Size(50, 50);
            Font buttonFont = new Font("Segoe UI", 9, FontStyle.Regular);

            Button btnRemove = new Button
            {
                Text = "Ø",
                Size = buttonSize,
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = buttonFont,
                Cursor = Cursors.Hand
            };
            btnRemove.Click += (s, e) => RemoveUnit(unit);

            Button btnUp = new Button
            {
                Text = "↑",
                Size = buttonSize,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = buttonFont,
                Cursor = Cursors.Hand
            };
            btnUp.Click += (s, e) => MoveUnitUp(unit);

            Button btnDown = new Button
            {
                Text = "↓",
                Size = buttonSize,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = buttonFont,
                Cursor = Cursors.Hand
            };
            btnDown.Click += (s, e) => MoveUnitDown(unit);

            buttonPanel.Controls.Add(btnRemove);
            buttonPanel.Controls.Add(btnUp);
            buttonPanel.Controls.Add(btnDown);

            panel.Controls.Add(buttonPanel);

            return panel;
        }

        private void RemoveUnit(Unit unit)
        {
            army.RemoveUnit(unit);
            money += unit.Cost;
            labelMoney.Text = $"Монет: {money}";
            RefreshArmyUI();
        }

        private void MoveUnitUp(Unit unit)
        {
            var list = army.GetAllUnits();
            int oldIndex = list.IndexOf(unit);
            if (oldIndex > 0)
            {
                army.MoveUnit(unit, oldIndex - 1, 1);
                RefreshArmyUI();
            }
        }

        private void MoveUnitDown(Unit unit)
        {
            var list = army.GetAllUnits();
            int oldIndex = list.IndexOf(unit);
            if (oldIndex < list.Count - 1 && oldIndex >= 0)
            {
                army.MoveUnit(unit, oldIndex + 1, 0);
                RefreshArmyUI();
            }
        }
    }
}
