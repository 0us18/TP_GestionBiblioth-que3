using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Application.Services;

public class ActivityService
{
    private readonly IUnitOfWork _unitOfWork;

    public ActivityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
    {
        return await _unitOfWork.Activities.GetAllAsync();
    }

    public async Task<Activity?> GetActivityByIdAsync(int id)
    {
        return await _unitOfWork.Activities.GetActivityWithParticipantsAsync(id);
    }

    public async Task CreateActivityAsync(Activity activity)
    {
        if (activity.ActivityDate <= DateTime.Now)
            throw new ArgumentException("La date de l'activité doit être dans le futur.");

        if (activity.MaxCapacity <= 0)
            throw new ArgumentException("La capacité doit être supérieure à 0.");

        await _unitOfWork.Activities.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateActivityAsync(Activity activity)
    {
        var existing = await _unitOfWork.Activities.GetByIdAsync(activity.ActivityId);
        if (existing == null)
            throw new ArgumentException("Activité introuvable.");

        if (activity.ActivityDate <= DateTime.Now)
            throw new ArgumentException("La date de l'activité doit être dans le futur.");

        if (activity.MaxCapacity <= 0)
            throw new ArgumentException("La capacité doit être supérieure à 0.");

        existing.Name = activity.Name;
        existing.Description = activity.Description;
        existing.Type = activity.Type;
        existing.ActivityDate = activity.ActivityDate;
        existing.MaxCapacity = activity.MaxCapacity;
        existing.OrganizerEmployeeId = activity.OrganizerEmployeeId;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteActivityAsync(int id)
    {
        var activity = await _unitOfWork.Activities.GetActivityWithParticipantsAsync(id);
        if (activity == null)
            throw new ArgumentException("Activité introuvable.");

        if (activity.Participations.Any())
            throw new InvalidOperationException("Impossible de supprimer une activité qui a des participations associées.");

        _unitOfWork.Activities.Remove(activity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RegisterParticipantAsync(int activityId, int userId)
    {
        var activity = await _unitOfWork.Activities.GetActivityWithParticipantsAsync(activityId);
        if (activity == null)
            throw new ArgumentException("Activité introuvable.");

        if (activity.Participations.Count >= activity.MaxCapacity)
            throw new InvalidOperationException("L'activité est complète.");

        if (activity.Participations.Any(p => p.UserId == userId))
            throw new InvalidOperationException("L'usager est déjà inscrit.");

        var participation = new ActivityParticipation
        {
            ActivityId = activityId,
            UserId = userId,
            RegistrationDate = DateTime.Now,
            AttendanceStatus = AttendanceStatus.Registered
        };
        activity.Participations.Add(participation);
        
        await _unitOfWork.SaveChangesAsync();
    }
}
