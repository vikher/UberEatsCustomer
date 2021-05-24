using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Views;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OnSiteProductDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private Order _cartorder;
        private bool _isProceedEnabled;
        private double _totalPrice;
        private Product? _selectedProduct;
        private ObservableCollection<ExtraComponent>? _extraComponents;
        private ObservableCollection<SecondaryComponent>? _secondaryComponents;
        private DelegateCommand _addproductToCartCommand;
        private DelegateCommand _valueChangedCommand;
        private DelegateCommand _componentsCheckChangedCommand;
        private ObservableCollection<Subsection>? _subsections;
        private ObservableCollection<Component>? _components;
        private DelegateCommand _goBackCommand;
        private ObservableCollection<Grouping<string, Component>> _componentGrouped;

        public OnSiteProductDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "Opción del menú en sitio";
        }

        public DelegateCommand AddproductToCartCommand => _addproductToCartCommand ?? (_addproductToCartCommand = new DelegateCommand(AddproductToCartAsync));
        public DelegateCommand ValueChangedCommand => _valueChangedCommand ?? (_valueChangedCommand = new DelegateCommand(StepperValueChangedAsync));
        public DelegateCommand ComponentsCheckChangedCommand => _componentsCheckChangedCommand ?? (_componentsCheckChangedCommand = new DelegateCommand(ComponentsCheckedAsync));
        //public DelegateCommand ViewCommandPageCommand => _viewCommandPageCommand ?? (_viewCommandPageCommand = new DelegateCommand(ViewCommandPageAsync));
        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(GoBackAsync));

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }
        public bool IsProceedEnabled
        {
            get => _isProceedEnabled;
            set => SetProperty(ref _isProceedEnabled, value);
        }
        public double TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public ObservableCollection<Subsection>? Subsections
        {
            get => _subsections;
            set => SetProperty(ref _subsections, value);
        }

        public ObservableCollection<Component>? Components
        {
            get => _components;
            set => SetProperty(ref _components, value);
        }
        public ObservableCollection<ExtraComponent>? ExtraComponents
        {
            get => _extraComponents;
            set => SetProperty(ref _extraComponents, value);
        }
        public ObservableCollection<SecondaryComponent>? SecondaryComponents
        {
            get => _secondaryComponents;
            set => SetProperty(ref _secondaryComponents, value);
        }
        public Order CartOrder
        {
            get => _cartorder;
            set => SetProperty(ref _cartorder, value);
        }
        public ObservableCollection<Grouping<string, Component>> ComponentGrouped
        {
            get => _componentGrouped;
            set => SetProperty(ref _componentGrouped, value);
        }
        public void AddItemInCurrentCart(Product product)
        {
            if (CartOrder.Products != null && CartOrder.Products.Contains(product))
            {
                CartOrder.Products.FirstOrDefault(x => x == product);
                return;
            }
            if (CartOrder?.Products == null || CartOrder.Products.Count.Equals(0))
            {
                CartOrder.Products = new List<Product>
                        {
                            product
                        };
                CartOrder.StartDate = DateTime.Now;

                //foreach (var secondComponent in CartOrder.Products)
                //{
                //    secondComponent.SecondComponents = new List<SecondComponent>(SelectedProduct.SecondComponents);
                //}
            }
            else
                CartOrder.Products.Add(product);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Components = new ObservableCollection<Component>();
            ComponentGrouped = new ObservableCollection<Grouping<string, Component>>();

            if (parameters.ContainsKey("ProductItemViewModelId"))
            {
                SelectedProduct = parameters.GetValue<Product>("ProductItemViewModelId");
                ExtraComponents = new ObservableCollection<ExtraComponent>(SelectedProduct.ExtraComponents);
                SecondaryComponents = new ObservableCollection<SecondaryComponent>(SelectedProduct.SecondaryComponents);
                Subsections = new ObservableCollection<Subsection>(SelectedProduct.Subsections);
                foreach (var i in Subsections.SelectMany(k => k.Components)) {
                    Components.Add(i);   
                }

                var sorted = from Component in Components
                             orderby Component.Description
                             group Component by Component.subsectionName into ComponentGroup
                             select new Grouping<string, Component>(ComponentGroup.Key, ComponentGroup);

                //create a new collection of groups
                ComponentGrouped = new ObservableCollection<Grouping<string, Component>>(sorted);

                //foreach (var item in Components)
                //{

                //    ComponentGrouped.Add(new Grouping<string, Component>(item.subsectionName, Components.Where(x => x.Description == item.Description)));
                //}
                if (SelectedProduct.Quantity == 0)
                {
                    SelectedProduct.Quantity = 1;

                    foreach (var extraComponents in ExtraComponents)
                    {
                        extraComponents.Quantity = 0;
                        extraComponents.Checked = false;
                    }

                    foreach (var secondaryComponent in SecondaryComponents)
                    {
                        secondaryComponent.Quantity = 0;
                        secondaryComponent.Checked = false;
                    }
                }
                LoadTotalPriceAsync();

            }
            if (parameters.ContainsKey("order"))
            {
                CartOrder = parameters.GetValue<Order>("order");
            }
            else
            {
                var EmptyTip = new Tip() { Amount = 0 };
                CartOrder = new Order() { Tip = EmptyTip };
            }
        }

        private void StepperValueChangedAsync()
        {
            LoadTotalPriceAsync();
        }
        private void LoadTotalPriceAsync()
        {
            if (ExtraComponents != null && SelectedProduct != null)
            {
                TotalPrice = SelectedProduct.OnSiteTotal;
            }
            else
            {
                return;
            }
        }
        private void ComponentsCheckedAsync()
        {
            IsProceedEnabled = Components.Any(x => x.Checked == true);
            for (int i = 0; i < Components.Count; i++)
            {
                var item = Components[i];

                if (item.Checked)
                {
                    if (item.Quantity == 0)
                    {
                        item.Quantity = 1;
                        TotalPrice += item.Price;
                    }
                }
                else
                {
                    if (item.Quantity == 1)
                    {
                        item.Quantity = 0;
                        TotalPrice -= item.Price;
                    }
                }
            }
        }
        private async void AddproductToCartAsync()
        {

            if (CartOrder == null)
                return;

            if (SelectedProduct.Quantity != 0)
            {
                SelectedProduct.Subsections = Subsections.Where(x => x.Components.Any(y => y.Quantity == 1)).ToList();
                SelectedProduct.SecondaryComponents = SecondaryComponents.ToList();
                SelectedProduct.ExtraComponents = ExtraComponents.ToList();
                AddItemInCurrentCart(SelectedProduct);

            }
            else
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Necesitas agregar un producto", "ok");
                return;
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "order", CartOrder }
            };
            await _navigationService.GoBackAsync(parameters);

            SelectedProduct = null;
            SecondaryComponents = null;
            ExtraComponents = null;
        }

        /*private async void ViewCommandPageAsync()
        {
            if (CartOrder.Products == null)
            {
                await _dialogService.DisplayAlertAsync("Carrito de Compras", "Aún no tienes productos en tu cesta", "ok");
                return;
            }

            //CartOrder.Establishment = Establishment;
            NavigationParameters parameters = new NavigationParameters
            {
                { "order", CartOrder }
            };

            await _navigationService.NavigateAsync(nameof(CommandPage), parameters);
        }*/

        private async void GoBackAsync()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
