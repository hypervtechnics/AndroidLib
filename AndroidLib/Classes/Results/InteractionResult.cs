using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Results
{
    public class InteractionResult<T>
    {
        private bool wasSuccessful;
        private T resultObject;
        private Exception error;

        /// <summary>
        /// Indicates whether the process was successfull
        /// </summary>
        public bool WasSuccessfull
        {
            get
            {
                return wasSuccessfull;
            }
        }

        /// <summary>
        /// The result of the process if the process was successful
        /// </summary>
        public T ResultObject
        {
            get
            {
                return resultObject;
            }
        }

        /// <summary>
        /// The error if any occured
        /// </summary>
        public Exception Error
        {
            get
            {
                return error;
            }
        }

        public InteractionResult(T resultObject, bool success, Exception exception)
        {
            this.error = exception;
            this.wasSuccessful = success;
            this.resultObject = resultObject;
        }
    }
}
