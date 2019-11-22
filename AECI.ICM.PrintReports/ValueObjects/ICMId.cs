using System;

namespace AECI.ICM.PrintReports.ValueObjects
{
    public class ICMId
    {
        private int Value { get; }

        #region Constructor

        public ICMId(int value)
        {
            if (value < 0)
                throw new ArgumentNullException(nameof(value), "ICM Id cannot be less than zero");

            Value = value;
        }

        #endregion

        #region Methods

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