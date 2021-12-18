using System;

namespace TicketsBooking.Domain.Common
{
    public class EditableEntity
    {
        public EditableEntity()
        {
            Id = Guid.NewGuid().ToString().Replace("-",string.Empty);
        }

        public string Id { get; set; }
    }
}
