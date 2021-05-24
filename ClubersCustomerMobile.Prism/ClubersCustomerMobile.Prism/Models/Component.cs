using Prism.Mvvm;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Component : BindableBase
    {
        public string subsectionName { get; set; }
        public int Id { get; set; }
        public double Price { get; set; }
        public bool Checked { get; set; }
        public string? Description { get; set; }
        public int ComponentTypeId { get; set; }
        public int AvaliabilityId { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, () => RaisePropertyChanged(nameof(Total)));
        }
        public double Total => (Quantity * Price);
    }
}
