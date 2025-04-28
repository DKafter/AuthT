namespace AuthExample.Core.Abstraction.Enums
{
    /// <summary>
    /// {ТИП}_{НАЗНАЧЕНИЕ}_{МОДЕЛЬ}_{ОШИБКА}
    /// </summary>
    public enum ErrorNamesEnum
    {
        SUCCESS = 0,
        INVALID_FIELD_NAME = 1,
        UNKNOWN_FIELD = 2,
        INVALID_NULL_USERNAME = 3,
        INVALID_MIN_USERNAME_LENGTH = 4,
        INVALID_MAX_USERNAME_LENGTH = 5,
        INVALID_NULL_EMAIL = 6,
        INVALID_EMAIL_FORMAT = 7,
        INVALID_NULL_PASSWORD = 8,
        INVALID_MIN_PASSWORD_LENGTH = 9,
        INVALID_PASSWORD_COMPLEXITY = 10
    }
}
