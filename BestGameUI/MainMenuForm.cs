using System;
using System.Drawing;
using System.Windows.Forms;
using StrategyGame;

namespace BestGameUI
{
    public partial class MainMenuForm : Form
    {
        // --- поля -----------------------------------------------------------
        private Button btnDeathLog;   // <-- вместо CheckBox
        private Button btnAbilityLog; // <-- вместо CheckBox
        private Button buttonStart;
        private Button buttonLoad;
        private Button buttonRandom;

        private bool deathLogOn = false;
        private bool abilityLogOn = false;

        // --- конструктор ----------------------------------------------------
        public MainMenuForm()
        {
            InitializeComponent();
            BuildUi();
        }

        // --- построение интерфейса -----------------------------------------
        private void BuildUi()
        {
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(30, 30, 30);
            Font = new Font("Segoe UI", 14f);
            KeyPreview = true;
            KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 18f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 18f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f)); // нижний отступ
            Controls.Add(table);

            var labelTitle = new Label
            {
                Text = "🔥CATZ⚔️",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 48f, FontStyle.Bold)
            };
            table.Controls.Add(labelTitle, 0, 0);

            // --- две кнопки-тумблеры ----------------------------------------
            btnDeathLog = MakeToggleButton("Логировать смерти", ToggleDeath);
            btnAbilityLog = MakeToggleButton("Логировать способности", ToggleAbility);

            table.Controls.Add(btnDeathLog, 0, 1);
            table.Controls.Add(btnAbilityLog, 0, 2);

            // --- большие кнопки Start / Load --------------------------------
            buttonStart = MakeMenuButton("Начать новую игру");
            buttonStart.Click += ButtonStart_Click;
            table.Controls.Add(buttonStart, 0, 3);

            buttonLoad = MakeMenuButton("Загрузить игру");
            buttonLoad.Click += ButtonLoad_Click;
            table.Controls.Add(buttonLoad, 0, 4);

            buttonRandom = MakeMenuButton("Случайная битва");
            buttonRandom.Click += ButtonRandom_Click;
            table.Controls.Add(buttonRandom, 0, 5);   // <-- учтите, что RowCount станет 7
        }

        // ---------- фабрики визуальных элементов ----------------------------
        private Button MakeMenuButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 28f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatAppearance = { BorderSize = 0 }
            };
            btn.MouseEnter += (_, __) => btn.BackColor = Color.FromArgb(90, 90, 90);
            btn.MouseLeave += (_, __) =>
                btn.BackColor = btn.Tag as string == "hoverLock"
                                ? Color.FromArgb(40, 120, 40)   // активное зелёное
                                : Color.FromArgb(70, 70, 70);   // обычное серое
            return btn;
        }

        private Button MakeToggleButton(string text, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 20f),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatAppearance = { BorderSize = 0 }
            };
            btn.Click += onClick;
            btn.MouseEnter += (_, __) => btn.BackColor = Color.FromArgb(90, 90, 90);
            btn.MouseLeave += (_, __) =>
                btn.BackColor = btn.Tag as string == "hoverLock"
                                ? Color.FromArgb(40, 120, 40)
                                : Color.FromArgb(70, 70, 70);
            return btn;
        }

        // ---------- логика тумблеров ----------------------------------------
        private void ToggleDeath(object? sender, EventArgs e)
        {
            deathLogOn = !deathLogOn;
            GameSettings.EnableDeathLogging = deathLogOn;
            ApplyToggleVisual(btnDeathLog, deathLogOn);
        }

        private void ToggleAbility(object? sender, EventArgs e)
        {
            abilityLogOn = !abilityLogOn;
            GameSettings.EnableAbilityLogging = abilityLogOn;
            ApplyToggleVisual(btnAbilityLog, abilityLogOn);
        }

        private static void ApplyToggleVisual(Button btn, bool enabled)
        {
            btn.BackColor = enabled ? Color.FromArgb(40, 120, 40)
                                    : Color.FromArgb(70, 70, 70);
            btn.Tag = enabled ? "hoverLock" : null;
        }

        // ---------- обработчики Start / Load --------------------------------
        private void ButtonStart_Click(object? sender, EventArgs e)
        {
            var formArmy1 = new ArmyBuildForm("Player 1", 300);
            formArmy1.ShowDialog();
            var army1 = formArmy1.ResultArmy;
            if (army1 == null) return;

            var formArmy2 = new ArmyBuildForm("Player 2", 300);
            formArmy2.ShowDialog();
            var army2 = formArmy2.ResultArmy;
            if (army2 == null) return;

            using var battleForm = new BattleForm(army1, army2);
            battleForm.ShowDialog();
        }

        private void ButtonLoad_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Файлы сохранения (*.json)|*.json"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var factory = new UnitFactory();
                var game = Game.Load(ofd.FileName, factory);

                using var battleForm = new BattleForm(game.Army1, game.Army2);
                battleForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить игру:\r\n{ex.Message}",
                                "Ошибка загрузки",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void ButtonRandom_Click(object? sender, EventArgs e)
        {
            const int budget = 300;          // такой же, как в ArmyBuildForm

            var factory = new UnitFactory();
            var army1 = RandomArmyGenerator.Generate(budget, factory);
            var army2 = RandomArmyGenerator.Generate(budget, factory);

            using var battleForm = new BattleForm(army1, army2);
            battleForm.ShowDialog();
        }
    }
}
