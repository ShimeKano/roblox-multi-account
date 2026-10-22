using System.Diagnostics;
using System.Threading;

namespace RobloxMultiClientLite;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

internal sealed class MainForm : Form
{
    private const string SingletonName = "ROBLOX_singletonEvent";
    private Mutex? _singletonMutex;
    private readonly Label _status = new();
    private readonly Label _processCount = new();
    private readonly Button _toggle = new();
    private readonly TextBox _gameUrl = new();
    private readonly Button _openGame = new();
    private readonly System.Windows.Forms.Timer _timer = new();

    public MainForm()
    {
        Text = "Roblox Multi-Client Lite";
        MinimumSize = new Size(480, 360);
        Size = new Size(540, 410);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            ColumnCount = 1,
            RowCount = 7,
            AutoSize = false
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(root);

        root.Controls.Add(new Label
        {
            Text = "Roblox Multi-Client Lite",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 8)
        });

        _status.AutoSize = true;
        _status.Text = "Multi-instance: OFF";
        _status.Margin = new Padding(0, 0, 0, 8);
        root.Controls.Add(_status);

        _toggle.Text = "Enable multi-instance";
        _toggle.AutoSize = true;
        _toggle.Padding = new Padding(8, 4, 8, 4);
        _toggle.Click += (_, _) => ToggleMultiInstance();
        root.Controls.Add(_toggle);

        _processCount.AutoSize = true;
        _processCount.Margin = new Padding(0, 12, 0, 8);
        root.Controls.Add(_processCount);

        root.Controls.Add(new Label
        {
            Text = "Game URL (optional):",
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 4)
        });

        var urlRow = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 8)
        };
        _gameUrl.Width = 320;
        _gameUrl.PlaceholderText = "https://www.roblox.com/games/...";
        _openGame.Text = "Open game page";
        _openGame.AutoSize = true;
        _openGame.Click += (_, _) => OpenGamePage();
        urlRow.Controls.Add(_gameUrl);
        urlRow.Controls.Add(_openGame);
        root.Controls.Add(urlRow);

        root.Controls.Add(new Label
        {
            Text = "Sign in manually in Roblox. This tool stores no credentials and cannot guarantee that the current Roblox build allows five separate clients. Use Roblox's normal UI to join the same experience/server.",
            AutoSize = true,
            MaximumSize = new Size(470, 0),
            ForeColor = SystemColors.GrayText,
            Margin = new Padding(0, 8, 0, 0)
        });

        _timer.Interval = 1500;
        _timer.Tick += (_, _) => RefreshProcessCount();
        _timer.Start();
        RefreshProcessCount();
        FormClosing += (_, _) => DisableMultiInstance();
    }

    private void ToggleMultiInstance()
    {
        if (_singletonMutex is not null)
        {
            DisableMultiInstance();
            return;
        }

        try
        {
            // MultiBloxy uses a named Windows Mutex rather than an event object.
            // Keep the mutex handle alive while multi-instance mode is enabled.
            // Roblox may still block or ignore this legacy compatibility technique.
            _singletonMutex = new Mutex(false, SingletonName);
            _status.Text = "Multi-instance: enabled (Roblox compatibility unverified)";
            _toggle.Text = "Disable multi-instance";
        }
        catch (Exception ex)
        {
            _status.Text = $"Could not enable mutex: {ex.Message}";
            _singletonMutex?.Dispose();
            _singletonMutex = null;
        }
    }

    private void DisableMultiInstance()
    {
        _singletonMutex?.Dispose();
        _singletonMutex = null;
        _status.Text = "Multi-instance: OFF";
        _toggle.Text = "Enable multi-instance";
    }

    private void OpenGamePage()
    {
        var value = _gameUrl.Text.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            Process.Start(new ProcessStartInfo("https://www.roblox.com/games") { UseShellExecute = true });
            return;
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp) ||
            (!uri.Host.Equals("www.roblox.com", StringComparison.OrdinalIgnoreCase) &&
             !uri.Host.Equals("roblox.com", StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Enter an official roblox.com game URL.", "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
    }

    private void RefreshProcessCount()
    {
        try
        {
            var count = Process.GetProcessesByName("RobloxPlayerBeta").Length;
            _processCount.Text = $"RobloxPlayerBeta processes detected: {count} (not a session/account check)";
        }
        catch
        {
            _processCount.Text = "Roblox process status unavailable.";
        }
    }
}
