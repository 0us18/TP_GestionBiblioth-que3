using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Activities;

public partial class ActivityListForm : Form
{
    private readonly ActivityService _activityService;
    private DataGridView _gridActivities = null!;
    private Button _btnEdit = null!;
    private Button _btnDelete = null!;

    public ActivityListForm(ActivityService activityService)
    {
        _activityService = activityService;
        InitializeComponent();
        LoadActivities();
    }

    private void InitializeComponent()
    {
        this.Text = "Gestion des Activités";
        this.Size = new Size(900, 500);

        var btnNew = new Button { Text = "Nouvelle Activité", Location = new Point(20, 20), Width = 150 };
        btnNew.Click += (s, e) => OpenActivityDetail(null);

        _btnEdit = new Button { Text = "Modifier", Location = new Point(180, 20), Width = 100 };
        _btnEdit.Click += (s, e) => EditSelectedActivity();

        _btnDelete = new Button { Text = "Supprimer", Location = new Point(290, 20), Width = 100 };
        _btnDelete.Click += async (s, e) => await DeleteSelectedActivity();

        this.Controls.Add(btnNew);
        this.Controls.Add(_btnEdit);
        this.Controls.Add(_btnDelete);

        _gridActivities = new DataGridView
        {
            Location = new Point(20, 60),
            Size = new Size(840, 380),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };
        _gridActivities.DoubleClick += (s, e) =>
        {
            if (_gridActivities.SelectedRows.Count > 0)
            {
                var activity = (Activity)_gridActivities.SelectedRows[0].DataBoundItem;
                OpenActivityDetail(activity.ActivityId);
            }
        };

        this.Controls.Add(_gridActivities);
    }

    private async void LoadActivities()
    {
        var activities = await _activityService.GetAllActivitiesAsync();
        _gridActivities.DataSource = activities.ToList();
    }

    private void OpenActivityDetail(int? activityId)
    {
        var form = Program.ServiceProvider.GetRequiredService<ActivityDetailForm>();
        form.ActivityId = activityId;
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadActivities();
        }
    }

    private int? GetSelectedActivityId()
    {
        if (_gridActivities.SelectedRows.Count <= 0)
            return null;

        if (_gridActivities.SelectedRows[0].DataBoundItem is not Activity activity)
            return null;

        return activity.ActivityId;
    }

    private void EditSelectedActivity()
    {
        var id = GetSelectedActivityId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner une activité.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        OpenActivityDetail(id.Value);
    }

    private async Task DeleteSelectedActivity()
    {
        var id = GetSelectedActivityId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner une activité.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show("Confirmer la suppression de l'activité sélectionnée ?", "Confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            await _activityService.DeleteActivityAsync(id.Value);
            MessageBox.Show("Activité supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadActivities();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
