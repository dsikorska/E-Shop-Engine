using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Domain.Enumerables
{
    public enum PaymentMethod
    {
        Card,

        [Display(Name = "On Delivery")]
        OnDelivery,

        [Display(Name = "Fast Transfer")]
        FastTransfer
    }
}
