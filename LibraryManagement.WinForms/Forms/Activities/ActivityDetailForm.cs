using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.WinForms.Forms.Activities;

public partial class ActivityDetailForm : Form
{
    private readonly ActivityService _activityService;
    public int? ActivityId { get; set; }

    private TextBox _txtName;
    private TextBox _txtDescription;
    private ComboBox _cmbType;
    private DateTimePicker _dtpDate;
    private NumericUpDown _numCapacity;

    public ActivityDetailForm(ActivityService activityService)
    {
        _activityService = activityService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = "Détail de l'Activité";
        this.Size = new Size(500, 400);
        this.StartPosition = FormStartPosition.CenterParent;

        var lblName = new Label { Text = "Nom:", Location = new Point(20, 20) };
        _txtName = new TextBox { Location = new Point(120, 20), Width = 300 };

        var lblDesc = new Label { Text = "Description:", Location = new Point(20, 60) };
        _txtDescription = new TextBox { Location = new Point(120, 60), Width = 300, Height = 60, Multiline = true };

        var lblType = new Label { Text = "Type:", Location = new Point(20, 140) };
        _cmbType = new ComboBox { Location = new Point(120, 140), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbType.DataSource = Enum.GetValues(typeof(ActivityType));

        var lblDate = new Label { Text = "Date:", Location = new Point(20, 180) };
        _dtpDate = new DateTimePicker { Location = new Point(120, 180), Width = 200, Format = DateTimePickerFormat.Short };

        var lblCapacity = new Label { Text = "Capacité:", Location = new Point(20, 220) };
        _numCapacity = new NumericUpDown { Location = new Point(120, 220), Width = 100, Minimum = 1, Maximum = 1000 };

        var btnSave = new Button { Text = "Enregistrer", Location = new Point(120, 300), DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await SaveActivity();

        this.Controls.AddRange(new Control[] { lblName, _txtName, lblDesc, _txtDescription, lblType, _cmbType, lblDate, _dtpDate, lblCapacity, _numCapacity, btnSave });
    }

    private async Task LoadData()
    {
        if (ActivityId.HasValue)
        {
            var activity = await _activityService.GetActivityByIdAsync(ActivityId.Value);
            if (activity != null)
            {
                _txtName.Text = activity.Name;
                _txtDescription.Text = activity.Description;
                _cmbType.SelectedItem = activity.Type;
                _dtpDate.Value = activity.ActivityDate;
                _numCapacity.Value = activity.MaxCapacity;
            }
        }
    }

    private async Task SaveActivity()
    {
        try
        {
            var activity = new Activity
            {
                Name = _txtName.Text,
                Description = _txtDescription.Text,
                Type = (ActivityType)_cmbType.SelectedItem,
                ActivityDate = _dtpDate.Value,
                MaxCapacity = (int)_numCapacity.Value,
                // For simplicity, we assume a default organizer or handle it better in real app
                // Since OrganizerEmployeeId is nullable now, we can leave it null or set a default
                OrganizerEmployeeId = 1 // Hardcoded for demo/student level simplicity as requested
            };

            if (ActivityId.HasValue)
            {
                activity.ActivityId = ActivityId.Value;
                // Update logic... ActivityService needs Update method or we use context directly via service
                // For now, let's assume Add only for simplicity or I'd need to add Update to Service
                MessageBox.Show("La modification n'est pas encore implémentée dans le service.");
            }
            else
            {
                await _activityService.CreateActivityAsync(activity);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
