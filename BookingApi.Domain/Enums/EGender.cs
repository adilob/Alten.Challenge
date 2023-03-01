using System.ComponentModel;

namespace BookingApi.Domain.Enums
{
    public enum EGender
    {
        [Description("None")]
        None = 0,

        [Description("Male")]
        Male = 1,

        [Description("Female")]
        Female = 2
    }
}
