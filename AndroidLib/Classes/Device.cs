using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib
{
    public class Device
    {
        #region Constructors

        /// <summary>
        /// Creates a device with the specific serial number
        /// </summary>
        /// <param name="serialNo">The serial number of the device as a String</param>
        public Device(String serialNo, String model, String productName)
        {
            mSerialNumber = serialNo;
            mProductName = productName;
            mModel = model;
            this.updateInfo();
        }

        #endregion

        #region Private Fields

        private String mSerialNumber;
        private String mModel;
        private String mProductName;

        #endregion

        #region Public Fields


        /// <summary>
        /// Returns the serial number of the device
        /// </summary>
        public String SerialNumber
        {
            get
            {
                return mSerialNumber;
            }
        }

        /// <summary>
        /// Returns the model number of the device
        /// </summary>
        public String Model
        {
            get
            {
                return mModel;
            }
        }

        /// <summary>
        /// Returns the product name of your device
        /// </summary>
        public String ProductName
        {
            get
            {
                return mProductName;
            }
        }

        #endregion

        #region Private Methods

        internal void updateInfo()
        {
            //TODO

        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Updates all values of this device
        /// </summary>
        public void Update()
        {
            this.updateInfo();
        }

        #endregion
    }
}
