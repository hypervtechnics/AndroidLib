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
        /// <param name="model">The model of the device</param>
        /// <param name="productName">The product name of the device</param>
        /// <param name="state">The state of the device</param>
        public Device(String serialNo, String model, String productName, String name, DeviceState state)
        {
            mSerialNumber = serialNo;
            mProductName = productName;
            mModel = model;
            mConnectionStatus = state;
            mName = name;
            this.updateInfo();
        }



        #endregion

        #region Private Fields

        private String mSerialNumber;
        private String mModel;
        private String mProductName;
        private DeviceState mConnectionStatus;
        private String mName;

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

        public String Name
        {
            get
            {
                return mName;
            }
        }

        /// <summary>
        /// Returns the state of the device
        /// </summary>
        public DeviceState ConnectionStatus
        {
            get
            {
                return mConnectionStatus;
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

    #region State of devices
    public enum DeviceState
    {
        Unknown,
        Online,
        Offline,
        Bootloader,
        Unauthorized
    }
    #endregion
}
