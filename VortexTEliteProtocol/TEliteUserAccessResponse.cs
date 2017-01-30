// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteUserAccessResponse.cs $
// $Author: har $
// $Date: 2011-06-14 11:17:47 +0200 (Di, 14 Jun 2011) $
// $Revision: 656 $
//
// Description:
//
// -------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TEliteUserAccessResponse
    /// </summary>
    public class TEliteUserAccessResponse : TEliteMessage
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************

        /// <summary>
        /// User access parameter
        /// </summary>
        public enum UserAccessParamEnum
        {
            /// <summary>
            /// No
            /// </summary>
            No = 0x30, 
            /// <summary>
            /// Yes
            /// </summary>
            Yes = 0x31
        };

        #endregion


        #region Constants
        //**************************************************
        // Constants
        //**************************************************

        #endregion


        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Public fields
        //**************************************************
        // Public fields
        //**************************************************

        #endregion

        #region Protected fields
        //**************************************************
        // Protected fields
        //**************************************************

        #endregion

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        /// <summary>
        /// User access response message data as array
        /// </summary>
        private byte[] m_Data = null;

        /// <summary>
        /// User right, if user can execute batch commands
        /// </summary>
        private bool m_BatchAllowed = false;

        /// <summary>
        /// User right, if user can update the spell check db
        /// </summary>
        private bool m_UpdateSpellcheck = false;

        #endregion

        #endregion


        #region Properties
        //**************************************************
        // Properties
        //**************************************************

        #region Public properties
        //**************************************************
        // Public properties
        //**************************************************

        /// <summary>
        /// Gets the right, if a user can execute batch commands
        /// </summary>
        public bool BatchAllowed
        {
            get { return m_BatchAllowed; }
        }

        /// <summary>
        /// Gets the right, if a user can update the spell check db
        /// </summary>
        public bool UpdateSpellcheck
        {
            get { return m_UpdateSpellcheck; }
        }

        #endregion

        #region Protected properties
        //**************************************************
        // Protected properties
        //**************************************************

        #endregion

        #region Private properties
        //**************************************************
        // Private properties
        //**************************************************

        #endregion

        #endregion


        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Constructor
        //**************************************************
        // Constructor
        //**************************************************

        /// <summary>
        /// Initializes a new instance of the TEliteUserAccessResponse class.
        /// </summary>
        public TEliteUserAccessResponse()
        {
            this.m_MessageOrigin = MessageOriginEnum.Vortex;
            this.m_Cmd = MessageTypeCode.UserAccessResponse;
        }

        /// <summary>
        /// Initializes a new instance of the TEliteUserAccessResponse class.
        /// </summary>
        public TEliteUserAccessResponse(byte[] messageFrame)
            : this()
        {
            this.m_Data = messageFrame;

            if (messageFrame.Length >= 3)
            {
                m_BatchAllowed = (messageFrame[0] == (byte)UserAccessParamEnum.Yes);
                m_UpdateSpellcheck = (messageFrame[1] == (byte)UserAccessParamEnum.Yes);
            }
        }

        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteUserAccessResponse()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts UserAccessResponse message to a byte array
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            return this.m_Data;
        }

        #endregion

        #region Protected Methods
        //**************************************************
        // Protected Methods
        //**************************************************

        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************

        #endregion

        #region Web-Methods
        //**************************************************
        // Web-Methods
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteUserAccessResponse.cs $
 * 
 * 2     22.06.09 13:35 san
 * 
 * 1     22.06.09 10:54 san
 * -------------------------------------------------------------------
 */