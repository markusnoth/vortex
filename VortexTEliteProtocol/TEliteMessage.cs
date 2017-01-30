// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteMessage.cs $
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
    /// TEliteMessage
    /// </summary>
    public abstract class TEliteMessage
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************
        /// <summary>
        /// MessageOriginEnum
        /// </summary>
        public enum MessageOriginEnum
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Vortex
            /// </summary>
            Vortex = 1, 
            /// <summary>
            /// Client
            /// </summary>
            Client = 2, 
            /// <summary>
            /// Both
            /// </summary>
            Both = 3
        };
        /// <summary>
        /// MessageTypeCode
        /// </summary>
        public enum MessageTypeCode 
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0x00, 
            /// <summary>
            /// CommandResponse
            /// </summary>
            CommandResponse = 0x01, 
            /// <summary>
            /// PageRequest
            /// </summary>
            PageRequest = 0x02, 
            /// <summary>
            /// PageResponse
            /// </summary>
            PageResponse = 0x02,
            /// <summary>
            /// PageWithCommandRow
            /// </summary>
            PageWithCommandRow = 0x03, 
            /// <summary>
            /// DisconnectLink
            /// </summary>
            DisconnectLink = 0x03, 
            /// <summary>
            /// CommandLine
            /// </summary>
            CommandLine = 0x0A, 
            /// <summary>
            /// FileBlock
            /// </summary>
            FileBlock = 0x1D, 
            /// <summary>
            /// FileBrowse
            /// </summary>
            FileBrowse = 0x20, 
            /// <summary>
            /// UserAccessRequest
            /// </summary>
            UserAccessRequest = 0x22, 
            /// <summary>
            /// UserAccessResponse
            /// </summary>
            UserAccessResponse = 0x22, 
            /// <summary>
            /// FieldDefinitions
            /// </summary>
            FieldDefinitions = 0x30, 
            /// <summary>
            /// ErrorResponse
            /// </summary>
            ErrorResponse = 0xFF 
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

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        /// <summary>
        /// Origin of the protocol message
        /// </summary>
        protected MessageOriginEnum m_MessageOrigin = MessageOriginEnum.None;

        /// <summary>
        /// Message CMD code
        /// </summary>
        protected MessageTypeCode m_Cmd = MessageTypeCode.None;

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
        /// Gets the Origin of the protocol message
        /// </summary>
        public MessageOriginEnum MessageOrigin
        {
            get { return m_MessageOrigin; }
        }

        /// <summary>
        /// Gets the message CMD code
        /// </summary>
        public MessageTypeCode Cmd
        {
            get { return m_Cmd; }
        }

        /// <summary>
        /// Converts the message frame to a array of byte
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToArray();

        #endregion

        #endregion

        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteMessage.cs $
 * 
 * 7     16.07.09 14:41 Not
 * file block functionality (incomplete)
 * 
 * 6     15.07.09 14:15 san
 * Add new function: Get Page-Attribute EP1 from Vortex.
 * 
 * 5     2.07.09 15:35 san
 * 
 * 4     22.06.09 10:54 san
 * 
 * 3     19.06.09 15:01 san
 * 
 * 2     18.06.09 14:58 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */