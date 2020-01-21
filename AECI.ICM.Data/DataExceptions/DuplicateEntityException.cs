using System;

namespace AECI.ICM.Data.DataExceptions
{
    public class DuplicateEntityException:Exception
    {
        public DuplicateEntityException(object item, string message):
            base(
                $"An exception occurred for item {item}, {message}"
            )
        {}
    }
}
