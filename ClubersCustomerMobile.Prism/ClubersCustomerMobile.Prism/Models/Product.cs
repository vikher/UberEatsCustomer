using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Product : BindableBase
    {

        public ProductFile? ProductFile { get; set; }
        public List<MainComponent>? MainComponents { get; set; }
        public List<SecondaryComponent>? SecondaryComponents { get; set; }
        public List<ExtraComponent>? ExtraComponents { get; set; }
        public List<Section>? Sections { get; set; }
        public List<Subsection>? Subsections { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public double HomePrice { get; set; }
        public string? Comments { get; set; }
        public string? Description { get; set; }
        public double OnSitePrice { get; set; }
        public bool AvailableHome { get; set; }
        public bool AvailableOnSite { get; set; }
        public int PreparationTime { get; set; }
        public bool Available { get; set; }

        //public int Total => (Quantity * Price);
        public double Total => ((ExtraComponents?.Sum(x => x.Total) * Quantity) + (Quantity * HomePrice)) ?? (Quantity * HomePrice);

        //public double OnSiteTotal => (Quantity * OnSitePrice);
        public double OnSiteTotal => ((ExtraComponents?.Sum(x => x.Total) * Quantity) + (Quantity* OnSitePrice)) ?? (Quantity* OnSitePrice);

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, () => { RaisePropertyChanged(nameof(Total)); RaisePropertyChanged(nameof(OnSiteTotal)); });
        }
    }
}
