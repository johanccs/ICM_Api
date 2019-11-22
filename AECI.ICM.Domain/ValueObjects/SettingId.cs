using System;

namespace AECI.ICM.Domain.ValueObjects
{
    public class SettingId
    {
        private int Value{get;}

        #region Constructor

        public SettingId(int value)
        {
            if (value < 0)
                throw new ArgumentNullException(
                    nameof(value), 
                    "Setting Id cannot be less than zero");

            Value = value;
        }

        #endregion

        #region Methods

        public int GetId() => Value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

    }
}
