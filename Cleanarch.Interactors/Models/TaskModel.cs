using System;

// ReSharper disable InconsistentNaming

namespace Cleanarch.DomainLayer.Models
{
    public class TaskModel: UseCasePayload
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsComplete { get; set; }
        public TaskModel(string title, DateTimeOffset date, string description = null)
        {
            Title = title;
            Description = description;
            Date = date;
            IsComplete = false;
        }
    }
}
