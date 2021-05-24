using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClubersCustomerMobile.Prism.Enums
{
    public enum AppStatus
    {
        Active = 1,
        Inactive = 2,
        Deleted = 3
    }

    public enum ResultCode
    {
        Success = 1,
        Alert = 2,
        Warning = 3,
        Fatal = 4,
        Unauthorized = 5
    }

    public enum MailTemplateType
    {
        Register = 1,
        ResetPassword = 2,
        AccountConfirm = 3,
        PasswordChanged = 4,
    }
    public enum ProcessNotificationType
    {
        Progress = 1,
        Alert = 2,
    }
    public enum ProcessStatus
    {
        Executing = 1,
        Finished = 2
    }
    public enum RoleSystem
    {
        Administrador = 1,
        Proveedor = 2,
        DeliveryMen = 3
    }
    public enum MembershipValidity
    {
        [Description("Anual")] Annual = 1,
        [Description("Mensual")] Monthly = 2,
        [Description("Semestral")] Biannual = 3
    }
    public enum TrialPeriod
    {
        [Description("Un Mes")] OneMonth = 1,
        [Description("Dos Meses")] TwoMonth = 2,
        [Description("Sin Vigencia")] NoValidity = 3
    }
    public enum ClubersCashVilidity
    {
        [Description("6 Meses")] SixMonth = 1,
        [Description("2 Meses")] TwoMonth = 2,
        [Description("1 Mes")] OneMonth = 3
    }

    public enum RefundsValidity
    {
        [Description("30 días")] ThirtyDays = 1,
        [Description("15 días")] FifteenDays = 2,
        [Description("Sin vigencia")] NoValidity = 3
    }
}
