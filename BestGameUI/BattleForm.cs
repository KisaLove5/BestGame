using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using StrategyGame;
using StrategyGame.Formations;
using StrategyGame.GameCommands;

namespace BestGameUI
{
    public partial class BattleForm : Form
    {
        private Image bg;
        private Game game;
        private Army army1;
        private Army army2;

        private const int UnitPanelWidth = 120;
        private const int UnitPanelHeight = 140;

        private Dictionary<int, Panel> unitPanelsArmy1 = new();
        private Dictionary<int, Panel> unitPanelsArmy2 = new();

        // ─────────── поля формы ───────────────────────────────────────────────────
        private Label victoryLabel;             // надпись «Победа!»
        private System.Windows.Forms.Timer wiggleTimer;  // таймер анимации
        private int wigglePhase = 0;            // счётчик тиков
        private bool battleEnded = false;       // чтобы знать, что бой уже окончен
        private List<Panel> wiggleTargets = new();
        private Dictionary<Panel, int> wiggleBaseLeft = new();
        private readonly TurnInvoker invoker = new();
        private TextBox logBox;
        private Button btnLine;
        private Button btnThreeLine;
        private Button btnWall;
        // ───────────────────────────────────────────────────────────────────────────

        public BattleForm(Army a1, Army a2)
        {
            InitializeComponent();

            // ***--- ДОБАВЛЕНО: делаем вертикальный стек строк армии ***
            flowArmy1.FlowDirection = FlowDirection.TopDown;
            flowArmy2.FlowDirection = FlowDirection.TopDown;
            flowArmy1.WrapContents = true;
            flowArmy2.WrapContents = true;

            tableLayoutMain.ColumnCount = 1;
            tableLayoutMain.ColumnStyles.Clear();
            tableLayoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            StyleCommandButtons();

            // ── панель справа -----------------------------------------------------
            var logPanel = new Panel
            {
                Size = new Size(380, 150),
                Location = new Point(this.ClientSize.Width - 380, this.ClientSize.Height - 150),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            logBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9f)
            };

            logPanel.Controls.Add(logBox);
            Controls.Add(logPanel);
            logPanel.BringToFront();

            var btnUndo = MakeMenuButton("Undo");
            btnUndo.Click += (_, __) =>
            {
                invoker.Undo(game);
                InitializeUnitPanels();
            };

            var btnRedo = MakeMenuButton("Redo");
            btnRedo.Click += (_, __) =>
            {
                var log = invoker.Redo(game);
                if (!string.IsNullOrEmpty(log))
                    logBox.AppendText(log + Environment.NewLine);

                InitializeUnitPanels();
            };

            panelCommands.Controls.Add(btnUndo);
            panelCommands.Controls.Add(btnRedo);

            // Кнопки смены построения
            btnLine = MakeMenuButton("Линия");
            btnThreeLine = MakeMenuButton("3 линии");
            btnWall = MakeMenuButton("Стенка");

            btnLine.Click += (_, __) =>
            {
                var f = new LineFormation();
                army1.SetFormation(f);
                army2.SetFormation(f);      // ★ обе армии
                InitializeUnitPanels();
            };

            btnThreeLine.Click += (_, __) =>
            {
                var f = new ThreeLineFormation();
                army1.SetFormation(f);
                army2.SetFormation(f);      // ★
                InitializeUnitPanels();
            };

            btnWall.Click += (_, __) =>
            {
                var f = new ColumnWallFormation();
                army1.SetFormation(f);
                army2.SetFormation(f);      // ★
                InitializeUnitPanels();
            };

            panelCommands.Controls.Add(btnLine);
            panelCommands.Controls.Add(btnThreeLine);
            panelCommands.Controls.Add(btnWall);

            // Настройка таблицы армий
            tableLayoutArmies.RowStyles.Clear();
            tableLayoutArmies.RowCount = 2;
            tableLayoutArmies.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutArmies.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            tableLayoutArmies.SetRow(panelCommands, 0);
            tableLayoutArmies.SetRow(tableInner, 1);

            labelPressEnter.Visible = false;

            flowArmy1.AutoSize = true;
            flowArmy2.AutoSize = true;
            flowArmy1.WrapContents = false;
            flowArmy2.WrapContents = false;

            CenterFlowPanel(panelArmy1, flowArmy1);
            CenterFlowPanel(panelArmy2, flowArmy2);

            this.Load += BattleForm_Load;
            this.KeyPreview = true;

            army1 = a1;
            army2 = a2;
            game = new Game();
            game.SetupBattle(army1, army2);

            victoryLabel = new Label
            {
                Visible = false,
                AutoSize = true,
                Font = new Font("Segoe UI", 48f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(victoryLabel);

            wiggleTimer = new System.Windows.Forms.Timer { Interval = 30 };
            wiggleTimer.Tick += WiggleTimer_Tick;
        }

        private Button MakeMenuButton(string text)
        {
            var b = new Button { Text = text };
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            b.ForeColor = Color.White;
            b.BackColor = Color.FromArgb(70, 70, 70);
            b.Margin = new Padding(10, 5, 10, 5);
            b.Padding = new Padding(15, 5, 15, 5);
            b.MinimumSize = new Size(160, 50);
            b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(90, 90, 90);
            b.MouseLeave += (_, __) => b.BackColor = Color.FromArgb(70, 70, 70);
            return b;
        }

        private void StyleCommandButtons()
        {
            foreach (var ctrl in panelCommands.Controls)
            {
                if (ctrl is not Button b) continue;

                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
                b.ForeColor = Color.White;
                b.BackColor = Color.FromArgb(70, 70, 70);
                b.Margin = new Padding(10, 5, 10, 5);
                b.Padding = new Padding(15, 5, 15, 5);
                b.MinimumSize = new Size(160, 50);

                b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(90, 90, 90);
                b.MouseLeave += (_, __) => b.BackColor = Color.FromArgb(70, 70, 70);
            }
        }
        private FlowLayoutPanel CreateLineFlow(
        IEnumerable<Unit> units,
        bool flipImage,
        Dictionary<Guid, Panel> dict)
        {
            var line = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0, 2, 0, 2),
                FlowDirection = flipImage ? FlowDirection.RightToLeft
                                          : FlowDirection.LeftToRight
            };

            if (!units.Any())
            {
                // Пустышка для выравнивания
                line.Height = UnitPanelHeight;
                line.Width = 1;
                line.Visible = false;
                return line;
            }

            foreach (var u in units)
            {
                var p = CreateUnitPanel(u, flipImage);
                dict[u.UnitId] = p;
                line.Controls.Add(p);
            }
            return line;
        }

        private void BattleForm_Load(object sender, EventArgs e)
        {
            ApplyBackgroundAndTransparency();
            InitializeUnitPanels();
            ActionLogger.StartNewBattle();
            ActionLogger.Log("Бой начался!");
        }

        private void ApplyBackgroundAndTransparency()
        {
            string imagesDir = Path.Combine(Application.StartupPath, "Graphics", "Back");
            string bgFile = Path.Combine(imagesDir, "BattleBack.png");
            bg = Image.FromFile(bgFile);

            tableLayoutMain.BackgroundImage = bg;
            tableLayoutMain.BackgroundImageLayout = ImageLayout.Stretch;

            Control[] transparentControls = {
                tableLayoutArmies,
                tableInner,
                panelCommands,
                panelArmy1,
                panelArmy2,
                flowArmy1,
                flowArmy2
            };

            foreach (var c in transparentControls)
                c.BackColor = Color.Transparent;
        }

        private void RenderArmy(
            FlowLayoutPanel host,
            IReadOnlyList<IReadOnlyList<Unit>> lines,
            Dictionary<int, Panel> dict,
            bool flipImage)
        {
            foreach (var lineUnits in lines)
            {
                var lineFlow = new FlowLayoutPanel
                {
                    AutoSize = true,
                    WrapContents = false,
                    Margin = new Padding(0, 2, 0, 2),
                    FlowDirection = flipImage
                        ? FlowDirection.RightToLeft
                        : FlowDirection.LeftToRight
                };

                foreach (var unit in lineUnits)
                {
                    var panel = CreateUnitPanel(unit, flipImage);
                    dict[unit.UnitId] = panel;
                    lineFlow.Controls.Add(panel);
                }

                host.Controls.Add(lineFlow);
            }
        }

        private void SyncArmies()
        {
            army1 = game.Army1;
            army2 = game.Army2;
        }

        private void InitializeUnitPanels()
        {
            SyncArmies();

            flowArmy1.SuspendLayout();
            flowArmy2.SuspendLayout();

            flowArmy1.Controls.Clear();
            flowArmy2.Controls.Clear();
            unitPanelsArmy1.Clear();
            unitPanelsArmy2.Clear();

            var lines1 = army1.GetLines();
            var lines2 = army2.GetLines();
            int max = Math.Max(lines1.Count, lines2.Count);

            for (int i = 0; i < max; i++)
            {
                var l1 = (i < lines1.Count) ? lines1[i] : Array.Empty<Unit>();
                var l2 = (i < lines2.Count) ? lines2[i] : Array.Empty<Unit>();

                flowArmy1.Controls.Add(
                    CreateLineFlow(l1, flipImage: true, unitPanelsArmy1));
                flowArmy2.Controls.Add(
                    CreateLineFlow(l2, flipImage: false, unitPanelsArmy2));
            }

            flowArmy1.ResumeLayout();
            flowArmy2.ResumeLayout();

            CenterFlowPanel(panelArmy1, flowArmy1);
            CenterFlowPanel(panelArmy2, flowArmy2);
        }

        private void UpdateUnitPanels()
        {
            SyncArmies();
            UpdateArmyPanels(army1, unitPanelsArmy1, flowArmy1, flipImage: true);
            UpdateArmyPanels(army2, unitPanelsArmy2, flowArmy2, flipImage: false);
            CenterFlowPanel(panelArmy1, flowArmy1);
            CenterFlowPanel(panelArmy2, flowArmy2);
        }

        private void AnimateProgressBar(ProgressBar progressBar, int targetValue)
        {
            int startValue = progressBar.Value;
            int step = (targetValue > startValue) ? 1 : -1;
            if (startValue == targetValue) return;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 15 };
            timer.Tick += (s, e) =>
            {
                if (progressBar.IsDisposed)
                {
                    timer.Stop();
                    return;
                }

                progressBar.Value = Math.Max(progressBar.Minimum, Math.Min(progressBar.Value + step, progressBar.Maximum));

                if (progressBar.Value == targetValue)
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }

        private void FlashPanel(Panel panel)
        {
            Color originalColor = panel.BackColor;
            panel.BackColor = Color.Red;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 100 };
            int ticks = 0;

            timer.Tick += (s, e) =>
            {
                ticks++;
                if (ticks >= 2)
                {
                    timer.Stop();
                    panel.BackColor = originalColor;
                }
            };
            timer.Start();
        }

        private void UpdateArmyPanels(Army army, Dictionary<int, Panel> unitPanels, FlowLayoutPanel flowPanel, bool flipImage)
        {
            var aliveUnits = army.GetAllUnits();
            flowPanel.SuspendLayout();
            var unitsToRemove = new List<int>();

            foreach (var pair in unitPanels)
            {
                int unitId = pair.Key;
                var panel = pair.Value;
                var aliveUnit = aliveUnits.Find(u => u.UnitId == unitId);

                if (aliveUnit == null)
                {
                    flowPanel.Controls.Remove(panel);
                    unitsToRemove.Add(unitId);
                    continue;
                }

                foreach (Control control in panel.Controls)
                {
                    if (control is ProgressBar hpBar)
                    {
                        AnimateProgressBar(hpBar, aliveUnit.Health);
                    }
                }
            }

            foreach (var unitId in unitsToRemove)
                unitPanels.Remove(unitId);

            for (int i = 0; i < aliveUnits.Count; i++)
            {
                var unit = aliveUnits[i];
                int unitId = unit.UnitId;

                if (!unitPanels.ContainsKey(unitId))
                {
                    var panel = CreateUnitPanel(unit, flipImage);
                    unitPanels[unitId] = panel;
                    flowPanel.Controls.Add(panel);
                }

                var desiredIndex = i;
                var currentIndex = flowPanel.Controls.IndexOf(unitPanels[unitId]);

                if (currentIndex != desiredIndex && currentIndex >= 0)
                    flowPanel.Controls.SetChildIndex(unitPanels[unitId], desiredIndex);
            }

            flowPanel.ResumeLayout();
            flowPanel.Invalidate();
        }

        private Panel CreateUnitPanel(Unit unit, bool flipImage)
        {
            Panel container = new Panel
            {
                Width = UnitPanelWidth,
                Height = UnitPanelHeight,
                Margin = new Padding(5)
            };

            Panel imagePanel = new Panel
            {
                Dock = DockStyle.Top,
                Width = UnitPanelWidth,
                Height = UnitPanelHeight - 20
            };

            string unitName = unit.DisplayName;
            string path = Path.Combine(Application.StartupPath, "Graphics/Units", unitName.ToLower() + ".png");

            if (File.Exists(path))
            {
                Image img = Image.FromFile(path);
                if (flipImage)
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);

                imagePanel.BackgroundImage = img;
                imagePanel.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                imagePanel.BackColor = Color.Gray;
            }

            ProgressBar hpBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Height = 20,
                Minimum = 0,
                Maximum = (unit.MaxHealth > 0) ? unit.MaxHealth : 1,
                Value = Math.Max(0, Math.Min(unit.Health, unit.MaxHealth))
            };

            container.Controls.Add(imagePanel);
            container.Controls.Add(hpBar);
            return container;
        }

        private void BattleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                invoker.Undo(game);
                InitializeUnitPanels();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                invoker.Redo(game);
                InitializeUnitPanels();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (!battleEnded) NextTurn();
                else Close();
            }
        }

        private void NextTurn()
        {
            if (game.IsBattleOver()) return;

            var cmd = new TurnCommand();
            invoker.DoTurn(cmd, game);
            logBox.AppendText(cmd.TurnLog + Environment.NewLine);
            UpdateUnitPanels();

            if (game.IsBattleOver())
                ShowVictoryScreen();
        }

        private void ShowVictoryScreen()
        {
            if (battleEnded) return;
            battleEnded = true;

            string result = game.GetFinalResult();
            victoryLabel.Text = result;
            victoryLabel.Visible = true;
            victoryLabel.BringToFront();
            victoryLabel.Left = (ClientSize.Width - victoryLabel.Width) / 2;
            victoryLabel.Top = (ClientSize.Height - victoryLabel.Height) / 4;

            IEnumerable<Panel> winners = result.StartsWith("Армия1") ? unitPanelsArmy1.Values : unitPanelsArmy2.Values;

            wiggleTargets = winners.ToList();
            wiggleBaseLeft.Clear();
            foreach (var p in wiggleTargets)
                wiggleBaseLeft[p] = p.Left;

            wigglePhase = 0;
            wiggleTimer.Start();
        }

        private void WiggleTimer_Tick(object? sender, EventArgs e)
        {
            wigglePhase++;
            double angle = wigglePhase * Math.PI / 15;
            int amplitude = 6;

            foreach (var p in wiggleTargets)
            {
                if (p.IsDisposed || !wiggleBaseLeft.ContainsKey(p)) continue;
                int baseLeft = wiggleBaseLeft[p];
                int offset = (int)(Math.Sin(angle) * amplitude);
                p.Left = baseLeft + offset;
            }
        }

        private void BattleForm_Resize(object sender, EventArgs e)
        {
            CenterFlowPanel(panelArmy1, flowArmy1);
            CenterFlowPanel(panelArmy2, flowArmy2);
        }

        private void PanelArmy_Resize(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                if (panel == panelArmy1)
                    CenterFlowPanel(panelArmy1, flowArmy1);
                else if (panel == panelArmy2)
                    CenterFlowPanel(panelArmy2, flowArmy2);
            }
        }

        private void CenterFlowPanel(Panel container, FlowLayoutPanel flow)
        {
            flow.Left = (container.Width - flow.Width) / 2;
            if (flow.Left < 0) flow.Left = 0;

            flow.Top = (container.Height - flow.Height) / 2;
            if (flow.Top < 0) flow.Top = 0;
        }

        private void btnNextTurn_Click(object sender, EventArgs e)
        {
            NextTurn();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Файл сохранения (*.json)|*.json",
                DefaultExt = "json",
                AddExtension = true
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                game.Save(sfd.FileName);
                ActionLogger.Log($"Сохранение выполнено: {Path.GetFileName(sfd.FileName)}");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Файл сохранения (*.json)|*.json"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var factory = new UnitFactory();
                game = Game.Load(ofd.FileName, factory);
                army1 = game.Army1;
                army2 = game.Army2;

                InitializeUnitPanels();
                ActionLogger.StartNewBattle();
                ActionLogger.Log($"Загружен бой из {Path.GetFileName(ofd.FileName)}");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Перехватываем Enter после окончания боя
            if (keyData == Keys.Enter && battleEnded)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void labelPressEnter_Click(object sender, EventArgs e) { }
    }
}