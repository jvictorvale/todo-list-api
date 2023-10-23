﻿using FluentValidation.Results;

namespace ToDo.Application.Notification;

public class Notificator : INotificator
{
    private bool _notFoundResource;
    private readonly List<string> _notifications = new();
    
    public void Handle(string message)
    {
        if (_notFoundResource)
            throw new InvalidOperationException("Cannot call Handle when there are NotFoundResouce!");
        _notifications.Add(message);
    }

    public void Handle(List<ValidationFailure> failures)
    {
        failures.ForEach(error => Handle(error.ErrorMessage));
    }

    public void HandleNotFoundResource()
    {
        if(HasNotification)
            throw new InvalidOperationException("Cannot call HandleNotFoundResource when there are notifications!");

        _notFoundResource = true;
    }

    public IEnumerable<string> GetNotifications() => _notifications;

    public bool HasNotification => _notifications.Any();
    public bool IsNotFoundResource => _notFoundResource;
}