namespace EDCompanionAPI.Models
{
    public enum LoginStatus
    {
        NotSet                  = -1,
        Ok                      =  0,
        PendingVerification     =  1,
        IncorrectCredentials    =  2,
        UnknownError            =  3
    }
}
