using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Users;

public partial class UserListForm : Form
{
    private readonly UserService _userService;
    private DataGridView _gridUsers = null!;
    private Button _btnAdd = null!;
    private Button _btnEdit = null!;
    private Button _btnDelete = null!;

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

        _btnAdd = new Button { Text = "Nouvel Usager", Location = new Point(20, 20), Width = 130 };
        _btnAdd.Click += (s, e) => OpenUserDetail(null);

        _btnEdit = new Button { Text = "Modifier", Location = new Point(160, 20), Width = 100 };
        _btnEdit.Click += (s, e) => EditSelectedUser();

        _btnDelete = new Button { Text = "Supprimer", Location = new Point(270, 20), Width = 100 };
        _btnDelete.Click += async (s, e) => await DeleteSelectedUser();

        _gridUsers = new DataGridView
        {
            Location = new Point(20, 60),
            Size = new Size(740, 380),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };
        _gridUsers.DoubleClick += (s, e) =>
        {
            if (_gridUsers.SelectedRows.Count > 0)
            {
                if (_gridUsers.SelectedRows[0].DataBoundItem is User user)
                    OpenUserDetail(user.UserId);
            }
        };

        this.Controls.Add(_btnAdd);
        this.Controls.Add(_btnEdit);
        this.Controls.Add(_btnDelete);
        this.Controls.Add(_gridUsers);
    }

    private async void LoadUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        _gridUsers.DataSource = users.ToList();
    }

    private int? GetSelectedUserId()
    {
        if (_gridUsers.SelectedRows.Count <= 0)
            return null;
        if (_gridUsers.SelectedRows[0].DataBoundItem is not User user)
            return null;
        return user.UserId;
    }

    private void OpenUserDetail(int? userId)
    {
        var form = Program.ServiceProvider.GetRequiredService<UserDetailForm>();
        form.UserId = userId;
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadUsers();
        }
    }

    private void EditSelectedUser()
    {
        var id = GetSelectedUserId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un usager.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        OpenUserDetail(id.Value);
    }

    private async Task DeleteSelectedUser()
    {
        var id = GetSelectedUserId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un usager.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show("Confirmer la suppression de l'usager sélectionné ?", "Confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            await _userService.DeleteUserAsync(id.Value);
            MessageBox.Show("Usager supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUsers();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
