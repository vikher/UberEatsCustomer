using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class AnswersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Question _question;
        private Answer _selectedAnswer;
        private Order _shoppingCartOrder;
        private List<Answer>? _answers;
        private DelegateCommand _navigateToSurveyPageCommand;
        public DelegateCommand NavigateToSurveyPageCommand => _navigateToSurveyPageCommand ?? (_navigateToSurveyPageCommand = new DelegateCommand(NavigateToSurveyPageAsync));
        public AnswersPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Posibles respuestas";
        }
        
        public Question Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        public Answer SelectedAnswer
        {
            get => _selectedAnswer;
            set => SetProperty(ref _selectedAnswer, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        public List<Answer>? Answers
        {
            get => _answers;
            set => SetProperty(ref _answers, value);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("selectedQuestion"))
            {
                Question = parameters.GetValue<Question>("selectedQuestion");
                Answers = Question.Answers;

                if (Question.Quantity == 0)
                {
                    Question.Quantity = 1;
                }
            }
            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
            }
        }
        private async void NavigateToSurveyPageAsync()
        {
            if (SelectedAnswer == null)
                return;

            if (ShoppingCartOrder == null)
                return;

            Question.Checked = true;
            Question.SelectedAnswer = SelectedAnswer;
            if (Question.Quantity != 0)
            {
                AddQuestionInCurrentSurvey(Question);
            }
            //else
            //{
            //    await _dialogService.DisplayAlertAsync("Alerta", "Necesitas agregar un producto", "ok");
            //    return;
            //}

            NavigationParameters parameters = new NavigationParameters
            {
                { "order", ShoppingCartOrder }
            };
            await _navigationService.GoBackAsync(parameters);
        }

        public void AddQuestionInCurrentSurvey(Question question)
        {
            if (ShoppingCartOrder.Evaluation.Surveys[0].Questions != null && ShoppingCartOrder.Evaluation.Surveys[0].Questions.Contains(question))
            {
                ShoppingCartOrder.Evaluation.Surveys[0].Questions.FirstOrDefault(x => x == question);
                return;
            }
            if (ShoppingCartOrder?.Evaluation.Surveys[0].Questions == null || ShoppingCartOrder.Evaluation.Surveys[0].Questions.Count.Equals(0))
            {
                ShoppingCartOrder.Evaluation.Surveys[0].Questions = new ObservableCollection<Question>
                        {
                            question
                        };
                //ShoppingCartOrder.StartDate = DateTime.Now;
            }
            else
                ShoppingCartOrder.Evaluation.Surveys[0].Questions.Add(question);
        }
    }
}
