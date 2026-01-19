using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Users;

public partial class UserListForm : Form
{
    private readonly UserService _userService;
    private DataGridView _gridUsers;

    public UserListForm(UserService userService)
    {
        _userService = userService;
        InitializeComponent();
        LoadUsers();
    }

    private void InitializeComponent()
    {
        this.Text = "Gestion des Usagers";
        this.Size = new Size(800, 500);

        // Grid
        _gridUsers = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };

        this.Controls.Add(_gridUsers);
    }

    private async void LoadUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        _gridUsers.DataSource = users.ToList();
    }
}
