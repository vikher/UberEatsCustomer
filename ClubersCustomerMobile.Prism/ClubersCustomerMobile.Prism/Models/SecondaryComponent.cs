using Prism.Mvvm;

namespace ClubersCustomerMobile.Prism.Models
{
    public class SecondaryComponent : BindableBase
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public object? Price { get; set; }
        public int Price1 { get; set; }
        public int ComponentTypeId { get; set; }
        public object? AvaliabilityId { get; set; }
        public bool Checked { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, () => RaisePropertyChanged(nameof(Total)));
        }

        public int Total => (Quantity * Price1);
    }
}
