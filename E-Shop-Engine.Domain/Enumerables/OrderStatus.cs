using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Domain.Enumerables
{
    public enum OrderStatus
    {
        [Display(Name = "In Progress")]
        InProgress,
        Completed,
    }
}
