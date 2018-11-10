using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Domain.Enumerables
{
    public enum OrderStatus
    {
        [Display(Name = "Waiting for payment")]
        WaitingForPayment,

        Processing,

        Delivery,

        Pending,
    }
}
