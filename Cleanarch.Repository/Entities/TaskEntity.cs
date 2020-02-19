using System;

namespace Cleanarch.Repository.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsComplete { get; set; }

        public TaskEntity(string title, DateTimeOffset date, string description = null)
        {
            Title = title;
            Description = description;
            Date = date;
            IsComplete = false;
        }
    }
}
