using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Activities;

public partial class ActivityListForm : Form
{
    private readonly ActivityService _activityService;
    private DataGridView _gridActivities;

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
        this.Controls.Add(btnNew);

        // Grid
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
}
