using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class SurveyPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;
        private bool _isProceedEnabled;
        private Evaluation _evaluation;
        private Question? _selectedQuestion;
        private ObservableCollection<Question> _questions;
        private DelegateCommand _answersPageCommand;
        private DelegateCommand _questionsCheckChangedCommand;
        private DelegateCommand _shareOptionsCommand;
        
        public SurveyPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Encuesta";
            LoadSurveysAsync();
        }
        public DelegateCommand AnswersPageCommand => _answersPageCommand ?? (_answersPageCommand = new DelegateCommand(AnswersPageAsync));
        public DelegateCommand QuestionsCheckChangedCommand => _questionsCheckChangedCommand ?? (_questionsCheckChangedCommand = new DelegateCommand(QuestionsCheckedAsync));
        public DelegateCommand ShareOptionsCommand => _shareOptionsCommand ?? (_shareOptionsCommand = new DelegateCommand(ShareOptionsAsync));
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }

        public Evaluation TheEvaluation
        {
            get => _evaluation;
            set => SetProperty(ref _evaluation, value);
        }
        public ObservableCollection<Question> Questions
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }

        //[AutoInitialize("question")]
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set => SetProperty(ref _selectedQuestion, value);
        }

        private async void ShareOptionsAsync()
        {
            if (ShoppingCartOrder.Evaluation == null)
                return;

            TheEvaluation.Surveys[0].Questions = Questions;
            ShoppingCartOrder.Evaluation = TheEvaluation;
            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            
            await _navigationService.NavigateAsync(nameof(ShareOptionsPage), parameters, useModalNavigation: true);
        }
        private async void AnswersPageAsync()
        {

            if (SelectedQuestion == null)
                return;


            if (ShoppingCartOrder == null)
            {
                NavigationParameters parameters = new NavigationParameters
            {
                { "selectedQuestion", SelectedQuestion }
            };

                await _navigationService.NavigateAsync(nameof(AnswersPage), parameters);
            }
            else
            {
                ShoppingCartOrder.Evaluation = TheEvaluation;
                NavigationParameters parameters = new NavigationParameters
            {
                { "selectedQuestion", SelectedQuestion },
                { "order", ShoppingCartOrder }

            };
                await _navigationService.NavigateAsync(nameof(AnswersPage), parameters);
                //SelectedProduct = null;
            }
            SelectedQuestion = null;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                if (ShoppingCartOrder.Evaluation == null)
                {
                    ShoppingCartOrder.Evaluation = TheEvaluation;
                }
                else
                {
                    TheEvaluation = ShoppingCartOrder.Evaluation;
                    Questions = TheEvaluation.Surveys[0].Questions;
                }
                    
            }
        }

        private async void LoadSurveysAsync()
        {
            Response response = await _apiService.GetAllSurveysAsync<Survey>("", " / api", "/Establishment", "bearer", "");
            var surveys = (List<Survey>)response.Result;
            TheEvaluation = new Evaluation() { Surveys = surveys };
            Questions = TheEvaluation.Surveys[0].Questions;
        }
        public bool IsProceedEnabled
        {
            get => _isProceedEnabled;
            set => SetProperty(ref _isProceedEnabled, value);
        }
        private void QuestionsCheckedAsync()
        {
            IsProceedEnabled = Questions.All(x => x.Checked == true);
            for (int i = 0; i < Questions.Count; i++)
            {
                var item = Questions[i];

                if (item.Checked)
                {
                    if (item.Quantity == 0)
                    {
                        item.Quantity = 1;
                    }
                }
                else
                {
                    if (item.Quantity == 1)
                    {
                        item.Quantity = 0;
                    }
                }
            }
        }


    }
}
